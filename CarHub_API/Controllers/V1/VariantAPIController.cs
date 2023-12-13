using AutoMapper;
using Azure;
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
using System.Security.Claims;
using System.Text.Json;

namespace CarHub_API.Controllers.v1
{
	[Route("api/v{version:apiVersion}/[controller]/[Action]")]
	[ApiController]
    [ApiVersion("1.0")]
    public class VariantAPIController : ControllerBase
    {
        protected APIResponse _response;

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public VariantAPIController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _response = new();
        }
		[HttpGet(Name = "VariantByPagination")]
		[ResponseCache(CacheProfileName = "Default30")]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<ActionResult<APIResponse>> VariantByPagination(string term, string orderBy, int currentPage = 1)
		{
			try
			{
				term = string.IsNullOrEmpty(term) ? "" : term.ToLower();

				VariantIndexVM variantIndexVM = new VariantIndexVM();
				IEnumerable<Variant> list = await _unitOfWork.Variant.GetAllAsync(includeProperties: "Car");

				var List = _mapper.Map<List<VariantDTO>>(list);

				variantIndexVM.NameSortOrder = string.IsNullOrEmpty(orderBy) ? "carName_desc" : "";

				if (!string.IsNullOrEmpty(term))
				{
					List.Where(u => u.Car.Name.ToLower().Contains(term)).ToList();

				}

				switch (orderBy)
				{
					case "carName_desc":
						List = List.OrderByDescending(a => a.Car.Name).ToList();
						break;

					default:
						List = List.OrderBy(a => a.Car.Name).ToList();
						break;
				}
				int totalRecords = List.Count();
				int pageSize = 10;
				int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
				List = List.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
				// current=1, skip= (1-1=0), take=5 
				// currentPage=2, skip (2-1)*5 = 5, take=5 ,
				variantIndexVM.Variants = List;
				variantIndexVM.CurrentPage = currentPage;
				variantIndexVM.TotalPages = totalPages;
				variantIndexVM.Term = term;
				variantIndexVM.PageSize = pageSize;
				variantIndexVM.OrderBy = orderBy;

				_response.Result = _mapper.Map<VariantIndexVM>(variantIndexVM);
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


		[HttpGet(Name = "GetVariants")]
		[ResponseCache(CacheProfileName = "Default30")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetVariants([FromQuery(Name = "filterDisplayOrder")] int? Id,
            [FromQuery] string? search, int pageSize = 0, int pageNumber = 1)
        {
            try
            {

                IEnumerable<Variant> variantList;

                if (Id > 0)
                {
                    variantList = await _unitOfWork.Variant.GetAllAsync(u => u.Id == Id, includeProperties: "Car", pageSize: pageSize,
                        pageNumber: pageNumber);
                }
                else
                {
                    variantList = await _unitOfWork.Variant.GetAllAsync(includeProperties: "Car", pageSize: pageSize,
                        pageNumber: pageNumber);
                }
                if (!string.IsNullOrEmpty(search))
                {
                    variantList = variantList.Where(u => u.Car.Name.ToLower().Contains(search));
                }
                Pagination pagination = new() { PageNumber = pageNumber, PageSize = pageSize };

                Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagination));
                _response.Result = _mapper.Map<List<VariantDTO>>(variantList);
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

        [HttpGet("{id:int}", Name = "GetVariant")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(200, Type =typeof(VillaDTO))]
        //        [ResponseCache(Location =ResponseCacheLocation.None,NoStore =true)]
        public async Task<ActionResult<APIResponse>> GetVariant(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var variant = await _unitOfWork.Variant.GetAsync(u => u.CarId == id);
                if (variant == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                _response.Result = _mapper.Map<VariantDTO>(variant);
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

		[HttpPost(Name = "CreateVariant")]
		//[Authorize(Roles = "admin")]
		[ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult<APIResponse>> CreateVariant([FromBody] VariantCreateVM createDTO)
		{
			try
			{


				Car car = _mapper.Map<Car>(createDTO.Car);
				//   List<CarXFeature> carXFeaturelist = await _unitOfWork.CarXFeature.GetAllAsync(u => u.CarId == car.Id && u.FeatureTypeId== createDTO.CarXFeature.FeatureTypeId, includeProperties: "Car,FeatureType,Feature");
				Variant variant = _mapper.Map<Variant>(createDTO.Variant);

                var data = await _unitOfWork.Variant.GetAsync(u => u.CarId == createDTO.Car.Id);
                if (data != null)
                {
                    await _unitOfWork.Variant.RemoveRangeAsync(u => u.CarId == createDTO.Car.Id, false);
                    Variant variant1 = new();

                    variant1.CarId = car.Id;
                    variant1.VariantName = variant.VariantName;
                    variant1.Transmission = variant.Transmission;
                    variant1.Price = variant.Price;



                    await _unitOfWork.Variant.CreateAsync(variant1);
                }
                else
                {
                    Variant variant1 = new();

                    variant1.CarId = car.Id;
                    variant1.VariantName = variant.VariantName;
                    variant1.Transmission = variant.Transmission;
                    variant1.Price = variant.Price;



                    await _unitOfWork.Variant.CreateAsync(variant1);

                }










				//if (await _unitOfWork.Car.GetAsync(u => u.Id == createDTO.CarXColor.CarId) == null)
				//{
				//    ModelState.AddModelError("ErrorMessages", "Car ID is Invalid!");
				//    return BadRequest(ModelState);
				//}





				//CarXColor carXColor = _mapper.Map<CarXColor>(createDTO);


				//  await _unitOfWork.CarXColor.CreateAsync(carXColor);
				// response.Result = mapper.Map<CarXColorDTO>(carxColor);
				_response.StatusCode = HttpStatusCode.Created;
				return _response;
			}
			catch (Exception ex)
			{
				_response.IsSuccess = false;
				_response.ErrorMessages
					 = new List<string>() { ex.ToString() };
			}
			return _response;
		}

		[ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete("{id:int}", Name = "DeleteVariant")]
        //[Authorize(Roles = "admin")]
        public async Task<ActionResult<APIResponse>> DeleteVariant(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }
                var variant = await _unitOfWork.Variant.GetAsync(u => u.Id == id);
                if (variant == null)
                {
                    return NotFound();
                }
                await _unitOfWork.Variant.RemoveAsync(variant);
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

        // [Authorize(Roles = "admin")]
        [HttpPut("{id:int}", Name = "UpdateVariant")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> UpdateVariant(int id, [FromBody] VariantUpdateDTO updateDTO)
        {
            try
            {
                if (updateDTO == null || id != updateDTO.Id)
                {
                    return BadRequest();
                }

                Variant model = _mapper.Map<Variant>(updateDTO);

                await _unitOfWork.Variant.UpdateAsync(model);
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

        [HttpPatch("{id:int}", Name = "UpdatePartialVariant")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePartialVariant(int id, JsonPatchDocument<VariantUpdateDTO> patchDTO)
        {
            if (patchDTO == null || id == 0)
            {
                return BadRequest();
            }
            var brand = await _unitOfWork.Variant.GetAsync(u => u.Id == id, tracked: false);

            VariantUpdateDTO variantDTO = _mapper.Map<VariantUpdateDTO>(brand);


            if (brand == null)
            {
                return BadRequest();
            }
            patchDTO.ApplyTo(variantDTO, ModelState);
            Variant model = _mapper.Map<Variant>(variantDTO);

            await _unitOfWork.Variant.UpdateAsync(model);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return NoContent();
        }


    }
}
