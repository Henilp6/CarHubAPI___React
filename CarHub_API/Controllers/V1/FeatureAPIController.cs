using AutoMapper;
using Azure;
using CarHub_API.Data;
using CarHub_API.Models;
using CarHub_API.Models.Dto;
using CarHub_API.Models.VM;
using CarHub_API.Repository.IRepository;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Net;

namespace CarHub_API.Controllers.v1
{
	[Route("api/v{version:apiVersion}/[controller]/[Action]")]
	[ApiController]
    [ApiVersion("1.0")]

    public class FeatureAPIController : ControllerBase
    {
        protected APIResponse _response;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
		private readonly ApplicationDbContext _db;
		public FeatureAPIController(IUnitOfWork unitOfWork, IMapper mapper, ApplicationDbContext db)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _response = new();
            _db = db;

        }
		[HttpGet(Name = "GetFeatureData")]
		[ResponseCache(CacheProfileName = "Default30")]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<ActionResult<APIResponse>> GetFeatureData(string term, string orderBy, int currentPage = 1)
		{

			try
			{

				term = string.IsNullOrEmpty(term) ? "" : term.ToLower();

				FeatureIndexVM featureIndexVM = new FeatureIndexVM();

				{
					var list = _db.Features.ToList();
					featureIndexVM.Features = _mapper.Map<List<FeatureDTO>>(list);
				}
				featureIndexVM.NameSortOrder = string.IsNullOrEmpty(orderBy) ? "typeName_desc" : "";

				var features = (from data in featureIndexVM.Features.ToList()
								where term == "" ||
								   data.Name.ToLower().
								   Contains(term)


								select new FeatureDTO
								{
									Id = data.Id,
									Name = data.Name,


								});

				switch (orderBy)
				{
					case "typeName_desc":
						features = features.OrderByDescending(a => a.Name);
						break;

					default:
						features = features.OrderBy(a => a.Name);
						break;
				}
				int totalRecords = features.Count();
				int pageSize = 5;
				int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
				features = features.Skip((currentPage - 1) * pageSize).Take(pageSize);
				// current=1, skip= (1-1=0), take=5 
				// currentPage=2, skip (2-1)*5 = 5, take=5 ,
				featureIndexVM.Features = features;
				featureIndexVM.CurrentPage = currentPage;
				featureIndexVM.TotalPages = totalPages;
				featureIndexVM.Term = term;
				featureIndexVM.PageSize = pageSize;
				featureIndexVM.OrderBy = orderBy;
				// return View(stateIndexVM);

				//  Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagination));
				_response.Result = _mapper.Map<FeatureIndexVM>(featureIndexVM);
				_response.StatusCode = HttpStatusCode.OK;
				return Ok(_response);
			}
			catch (Exception ex)
			{
				_response.IsSuccess = false;
				_response.ErrorMessages
					 = new List<string>() { ex.ToString() };
			}
			return _response;

		}




		[HttpGet(Name = "GetFeatures")]
		[MapToApiVersion("1.0")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetFeatures()
        {
            try
            {

                IEnumerable<Feature> featureList = await _unitOfWork.Feature.GetAllAsync(includeProperties: "FeatureType");
                _response.Result = _mapper.Map<List<FeatureDTO>>(featureList);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                     = new List<string>() { ex.ToString() };
            }
            return _response;

        }


        [HttpGet("{id:int}", Name = "GetFeature")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetFeature(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var feature = await _unitOfWork.Feature.GetAsync(u => u.Id == id);
                if (feature == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                _response.Result = _mapper.Map<FeatureDTO>(feature);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                     = new List<string>() { ex.ToString() };
            }
            return _response;
        }
		//[Authorize(Roles = "admin")]
		[HttpPost(Name = "CreateFeature")]
		[ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CreateFeature([FromForm] FeatureCreateDTO createDTO)
        {
            try
            {
                if (await _unitOfWork.Feature.GetAsync(u => u.Name.ToLower() == createDTO.Name.ToLower()) != null)
                {
                    ModelState.AddModelError("ErrorMessages", "Feature already Exists!");
                    return BadRequest(ModelState);
                }

             

                if (createDTO == null)
                {
                    return BadRequest(createDTO);
                }

                Feature feature = _mapper.Map<Feature>(createDTO);


                await _unitOfWork.Feature.CreateAsync(feature);
                _response.Result = _mapper.Map<FeatureDTO>(feature);
                _response.StatusCode = HttpStatusCode.Created;
                return CreatedAtRoute("GetFeature", new { id = feature.Id }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                     = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete("{id:int}", Name = "DeleteFeature")]
        public async Task<ActionResult<APIResponse>> DeleteFeature(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }
                var feature = await _unitOfWork.Feature.GetAsync(u => u.Id == id);
                if (feature == null)
                {
                    return NotFound();
                }
                await _unitOfWork.Feature.RemoveAsync(feature);
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                     = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpPut("{id:int}", Name = "UpdateFeature")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> UpdateFeature(int id, [FromForm] FeatureUpdateDTO updateDTO)
        {
            try
            {
                if (updateDTO == null || id != updateDTO.Id)
                {
                    return BadRequest();
                }

                Feature model = _mapper.Map<Feature>(updateDTO);

                await _unitOfWork.Feature.UpdateAsync(model);
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                     = new List<string>() { ex.ToString() };
            }
            return _response;
        }


    }
}
