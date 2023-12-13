using AutoMapper;
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

    public class CarTypeAPIController : ControllerBase
    {
        protected APIResponse _response;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _db;
        public CarTypeAPIController(IUnitOfWork unitOfWork, IMapper mapper, ApplicationDbContext db)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _response = new();
            _db = db;
            
        }
        [HttpGet(Name = "GetCarTypeData")]
        [ResponseCache(CacheProfileName = "Default30")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetCarTypeData(string term, string orderBy, int currentPage = 1)
        {

            try
            {

                term = string.IsNullOrEmpty(term) ? "" : term.ToLower();

                CarTypeIndexVM carTypIndexVM = new CarTypeIndexVM();

                {
                    var list = _db.CarTypes.ToList();
                    carTypIndexVM.CarTypes = _mapper.Map<List<CarTypeDTO>>(list);
                }
                carTypIndexVM.NameSortOrder = string.IsNullOrEmpty(orderBy) ? "typeName_desc" : "";

                var carTypes = (from data in carTypIndexVM.CarTypes.ToList()
                              where term == "" ||
                                 data.TypeName.ToLower().
                                 Contains(term) 


                              select new CarTypeDTO
                              {
                                  Id = data.Id,
                                  TypeName = data.TypeName,

                                  
                              });

                switch (orderBy)
                {
                    case "typeName_desc":
                        carTypes = carTypes.OrderByDescending(a => a.TypeName);
                        break;

                    default:
                        carTypes = carTypes.OrderBy(a => a.TypeName);
                        break;
                }
                int totalRecords = carTypes.Count();
                int pageSize = 5;
                int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
                carTypes = carTypes.Skip((currentPage - 1) * pageSize).Take(pageSize);
                // current=1, skip= (1-1=0), take=5 
                // currentPage=2, skip (2-1)*5 = 5, take=5 ,
                carTypIndexVM.CarTypes = carTypes;
                carTypIndexVM.CurrentPage = currentPage;
                carTypIndexVM.TotalPages = totalPages;
                carTypIndexVM.Term = term;
                carTypIndexVM.PageSize = pageSize;
                carTypIndexVM.OrderBy = orderBy;
                // return View(stateIndexVM);

                //  Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagination));
                _response.Result = _mapper.Map<CarTypeIndexVM>(carTypIndexVM);
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

        [HttpGet(Name = "GetCarTypes")]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetCarTypes()
        {
            try
            {

                IEnumerable<CarType> carTypeList = await _unitOfWork.CarType.GetAllAsync();
                _response.Result = _mapper.Map<List<CarTypeDTO>>(carTypeList);
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


        [HttpGet("{id:int}", Name = "GetCarType")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetCarType(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var carType = await _unitOfWork.CarType.GetAsync(u => u.Id == id);
                if (carType == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                _response.Result = _mapper.Map<CarTypeDTO>(carType);
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
        [HttpPost(Name = "CreateCarType")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CreateCarType([FromForm] CarTypeCreateDTO createDTO)
        {
            try
            {
                if (await _unitOfWork.CarType.GetAsync(u => u.TypeName.ToLower() == createDTO.TypeName.ToLower()) != null)
                {
                    ModelState.AddModelError("ErrorMessages", "CarType already Exists!");
                    return BadRequest(ModelState);
                }
                
                if (createDTO == null)
                {
                    return BadRequest(createDTO);
                }

                CarType carType = _mapper.Map<CarType>(createDTO);


                await _unitOfWork.CarType.CreateAsync(carType); 
                _response.Result = _mapper.Map<CarTypeDTO>(carType);
                _response.StatusCode = HttpStatusCode.Created;
                return CreatedAtRoute("GetCarType", new { id = carType.Id }, _response);
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
        [HttpDelete("{id:int}", Name = "DeleteCarType")]
        public async Task<ActionResult<APIResponse>> DeleteCarType(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }
                var carType = await _unitOfWork.CarType.GetAsync(u => u.Id == id);
                if (carType == null)
                {
                    return NotFound();
                }
                await _unitOfWork.CarType.RemoveAsync(carType);
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
        
        [HttpPut("{id:int}", Name = "UpdateCarType")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> UpdateCarType(int id, [FromForm] CarTypeUpdateDTO updateDTO)
        {
            try
            {
                if (updateDTO == null || id != updateDTO.Id)
                {
                    return BadRequest();
                }
                if (await _unitOfWork.CarType.GetAsync(u => u.TypeName.ToLower() == updateDTO.TypeName.ToLower() && u.Id != updateDTO.Id) != null)
                {
                    ModelState.AddModelError("ErrorMessages", "CarType already Exists!");
                    return BadRequest(ModelState);
                }

                CarType model = _mapper.Map<CarType>(updateDTO);

                await _unitOfWork.CarType.UpdateAsync(model);
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
