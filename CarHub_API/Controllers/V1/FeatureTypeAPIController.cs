using AutoMapper;
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

    public class FeatureTypeAPIController : ControllerBase
    {
        protected APIResponse _response;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public FeatureTypeAPIController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _response = new();
            
        }


        [HttpGet(Name = "FeatureTypeByPagination")]
        [ResponseCache(CacheProfileName = "Default30")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> FeatureTypeByPagination(string term, string orderBy, int currentPage = 1)
        {
            try
            {
                term = string.IsNullOrEmpty(term) ? "" : term.ToLower();

                FeatureTypeIndexVM featureTypeIndexVM = new FeatureTypeIndexVM();
                IEnumerable<FeatureType> list = await _unitOfWork.FeatureType.GetAllAsync();

                var List = _mapper.Map<List<FeatureTypeDTO>>(list);

                featureTypeIndexVM.NameSortOrder = string.IsNullOrEmpty(orderBy) ? "featureTypeName_desc" : "";

                if (!string.IsNullOrEmpty(term))
                {
                    List = List.Where(u => u.FeatureTypeName.ToLower().Contains(term)).ToList();
                }

                switch (orderBy)
                {
                    case "countryName_desc":
                        List = List.OrderByDescending(a => a.FeatureTypeName).ToList();
                        break;

                    default:
                        List = List.OrderBy(a => a.FeatureTypeName).ToList();
                        break;
                }
                int totalRecords = List.Count();
                int pageSize = 10;
                int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
                List = List.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
                // current=1, skip= (1-1=0), take=5 
                // currentPage=2, skip (2-1)*5 = 5, take=5 ,
                featureTypeIndexVM.FeatureTypes = List;
                featureTypeIndexVM.CurrentPage = currentPage;
                featureTypeIndexVM.TotalPages = totalPages;
                featureTypeIndexVM.Term = term;
                featureTypeIndexVM.PageSize = pageSize;
                featureTypeIndexVM.OrderBy = orderBy;

                _response.Result = _mapper.Map<FeatureTypeIndexVM>(featureTypeIndexVM);
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


        [HttpGet("GetString")]
        public IEnumerable<string> Get()
        {
            return new string[] { "String1", "string2" };
        }

        [HttpGet(Name = "GetFeatureTypes")]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetFeatureTypes()
        {
            try
            {

                IEnumerable<FeatureType> featureTypeList = await _unitOfWork.FeatureType.GetAllAsync();
                _response.Result = _mapper.Map<List<FeatureTypeDTO>>(featureTypeList);
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


        [HttpGet("{id:int}", Name = "GetFeatureType")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetFeatureType(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var featureType = await _unitOfWork.FeatureType.GetAsync(u => u.Id == id);
                if (featureType == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                _response.Result = _mapper.Map<FeatureTypeDTO>(featureType);
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
        [HttpPost(Name = "CreateFeatureType")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CreateFeatureType([FromBody] FeatureTypeCreateDTO createDTO)
        {
            try
            {

                if (await _unitOfWork.FeatureType.GetAsync(u => u.Id == createDTO.Id) != null)
                {
                    ModelState.AddModelError("ErrorMessages", "FeatureType  already Exists!");
                    return BadRequest(ModelState);
                }
                
                if (createDTO == null)
                {
                    return BadRequest(createDTO);
                }

				FeatureType featureType = _mapper.Map<FeatureType>(createDTO);


                await _unitOfWork.FeatureType.CreateAsync(featureType); 
                _response.Result = _mapper.Map<FeatureTypeDTO>(featureType);
                _response.StatusCode = HttpStatusCode.Created;
                return CreatedAtRoute("GetFeatureType", new { id = featureType.Id }, _response);
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
        [HttpDelete("{id:int}", Name = "DeleteFeatureType")]
        public async Task<ActionResult<APIResponse>> DeleteFeatureType(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }
                var featureType = await _unitOfWork.FeatureType.GetAsync(u => u.Id == id);
                if (featureType == null)
                {
                    return NotFound();
                }
                await _unitOfWork.FeatureType.RemoveAsync(featureType);
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
        
        [HttpPut("{id:int}", Name = "UpdateFeatureType")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> UpdateFeatureType(int id, [FromBody] FeatureTypeUpdateDTO updateDTO)
        {
            try
            {
                if (updateDTO == null || id != updateDTO.Id)
                {
                    return BadRequest();
                }
                if (await _unitOfWork.FeatureType.GetAsync(u => u.FeatureTypeName.ToLower() == updateDTO.FeatureTypeName.ToLower() && u.Id != updateDTO.Id) != null)
                {
                    ModelState.AddModelError("ErrorMessages", "Color already Exists!");
                    return BadRequest(ModelState);
                }

                FeatureType model = _mapper.Map<FeatureType>(updateDTO);

                await _unitOfWork.FeatureType.UpdateAsync(model);
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
