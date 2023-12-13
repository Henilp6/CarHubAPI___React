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
using System.Security.Claims;
using System.Text.Json;

namespace CarHub_API.Controllers.v1
{
    [Route("api/v{version:apiVersion}/[controller]/[Action]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class CarSpecificationAPIController : ControllerBase
    {
        protected APIResponse _response;

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _db;
        public CarSpecificationAPIController(IUnitOfWork unitOfWork, IMapper mapper,ApplicationDbContext db)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _response = new();
			_db = db;


		}
        [HttpGet(Name = "GetCarSpecificationData")]
        [ResponseCache(CacheProfileName = "Default30")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetCarSpecificationData(string term, string orderBy, int currentPage = 1)
        {

            try
            {

                term = string.IsNullOrEmpty(term) ? "" : term.ToLower();

                CarSpecificationIndexVM carSpecificationIndexVM = new CarSpecificationIndexVM();

                {
                    var list = _db.CarSpecifications.Include("Car").ToList();
                    carSpecificationIndexVM.CarSpecifications = _mapper.Map<List<CarSpecificationDTO>>(list);
                }
                carSpecificationIndexVM.NameSortOrder = string.IsNullOrEmpty(orderBy) ? "typeName_desc" : "";

                var carSpecifications = (from data in carSpecificationIndexVM.CarSpecifications.ToList()
                                where term == "" ||
                                   data.Car.Name.ToLower().
                                   Contains(term)


                                select new CarSpecificationDTO
                                {
                                    Id = data.Id,
                                  Displacement = data.Displacement,
                                    MaxPower = data.MaxPower,
                                    Cylinder = data.Cylinder,
                                    Length = data.Length,
                                    Car = data.Car,
                                    
               

                                });

                switch (orderBy)
                {
                    case "typeName_desc":
                        carSpecifications = carSpecifications.OrderByDescending(a => a.Car.Name);
                        break;

                    default:
                        carSpecifications = carSpecifications.OrderBy(a => a.Car.Name);
                        break;
                }
                int totalRecords = carSpecifications.Count();
                int pageSize = 5;
                int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
                carSpecifications = carSpecifications.Skip((currentPage - 1) * pageSize).Take(pageSize);
                // current=1, skip= (1-1=0), take=5 
                // currentPage=2, skip (2-1)*5 = 5, take=5 ,
                carSpecificationIndexVM.CarSpecifications = carSpecifications;
                carSpecificationIndexVM.CurrentPage = currentPage;
                carSpecificationIndexVM.TotalPages = totalPages;
                carSpecificationIndexVM.Term = term;
                carSpecificationIndexVM.PageSize = pageSize;
                carSpecificationIndexVM.OrderBy = orderBy;
                // return View(stateIndexVM);

                //  Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagination));
                _response.Result = _mapper.Map<CarSpecificationIndexVM>(carSpecificationIndexVM);
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

        [HttpGet(Name = "GetCarSpecifications")]
        [ResponseCache(CacheProfileName = "Default30")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetCarSpecifications([FromQuery(Name = "filterDisplayOrder")] int? Id,
            [FromQuery] string? search, int pageSize = 0, int pageNumber = 1)
        {
            try
            {

                IEnumerable<CarSpecification> carSpecificationList;

                if (Id > 0)
                {
                    carSpecificationList = await _unitOfWork.CarSpecification.GetAllAsync(u => u.Id == Id, includeProperties: "Car", pageSize: pageSize,
                        pageNumber: pageNumber);
                }
                else
                {
                    carSpecificationList = await _unitOfWork.CarSpecification.GetAllAsync(includeProperties: "Car", pageSize: pageSize,
                        pageNumber: pageNumber);
                }
                if (!string.IsNullOrEmpty(search))
                {
                    carSpecificationList = carSpecificationList.Where(u => u.Car.Name.ToLower().Contains(search));
                }
                Pagination pagination = new() { PageNumber = pageNumber, PageSize = pageSize };

                Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagination));
                _response.Result = _mapper.Map<List<CarSpecificationDTO>>(carSpecificationList);
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

        [HttpGet("{id:int}", Name = "GetCarSpecification")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(200, Type =typeof(VillaDTO))]
        //        [ResponseCache(Location =ResponseCacheLocation.None,NoStore =true)]
        public async Task<ActionResult<APIResponse>> GetCarSpecification(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var carSpecification = await _unitOfWork.CarSpecification.GetAsync(u => u.CarId == id);
                if (carSpecification == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                _response.Result = _mapper.Map<CarSpecificationDTO>(carSpecification);
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

        [HttpPost(Name = "CreateCarSpecification")]
        //[Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CreateCarSpecification([FromBody] CarSpecificationCreateVM createDTO)
        {
            try
            {


                Car car = _mapper.Map<Car>(createDTO.Car);
                //   List<CarXFeature> carXFeaturelist = await _unitOfWork.CarXFeature.GetAllAsync(u => u.CarId == car.Id && u.FeatureTypeId== createDTO.CarXFeature.FeatureTypeId, includeProperties: "Car,FeatureType,Feature");
                CarSpecification carSpecification = _mapper.Map<CarSpecification>(createDTO.CarSpecification);


                await _unitOfWork.CarSpecification.RemoveRangeAsync(u => u.CarId == createDTO.Car.Id, false);





                CarSpecification carSpecification1 = new();

                carSpecification1.CarId = car.Id;
                carSpecification1.Displacement = carSpecification.Displacement;
                carSpecification1.MaxPower = carSpecification.MaxPower;
                carSpecification1.MaxTorque = carSpecification.MaxTorque;
                carSpecification1.Cylinder = carSpecification.Cylinder;
                carSpecification1.SeatingCapacity = carSpecification.SeatingCapacity;
                carSpecification1.FrontSuspension = carSpecification.FrontSuspension;
                carSpecification1.RearSuspension = carSpecification.RearSuspension;
                carSpecification1.ShockAbsorbers = carSpecification.ShockAbsorbers;
                carSpecification1.Length = carSpecification.Length;
                carSpecification1.Width = carSpecification.Width;
                carSpecification1.Height = carSpecification.Height;
                carSpecification1.AirbagNo = carSpecification.AirbagNo;
                carSpecification1.WheelBase = carSpecification.WheelBase;
                carSpecification1.GearBox = carSpecification.GearBox;
                carSpecification1.DriveType = carSpecification.DriveType;
                carSpecification1.BootSpace = carSpecification.BootSpace;

				



				await _unitOfWork.CarSpecification.CreateAsync(carSpecification1);












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
        [HttpDelete("{id:int}", Name = "DeleteCarSpecification")]
        //[Authorize(Roles = "admin")]
        public async Task<ActionResult<APIResponse>> DeleteCarSpecification(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }
                var carSpecification = await _unitOfWork.CarSpecification.GetAsync(u => u.Id == id);
                if (carSpecification == null)
                {
                    return NotFound();
                }
                await _unitOfWork.CarSpecification.RemoveAsync(carSpecification);
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
        [HttpPut("{id:int}", Name = "UpdateCarSpecification")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> UpdateCarSpecification(int id, [FromBody] CarSpecificationUpdateDTO updateDTO)
        {
            try
            {
                if (updateDTO == null || id != updateDTO.Id)
                {
                    return BadRequest();
                }

                CarSpecification model = _mapper.Map<CarSpecification>(updateDTO);

                await _unitOfWork.CarSpecification.UpdateAsync(model);
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

        [HttpPatch("{id:int}", Name = "UpdatePartialCarSpecification")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePartialCarSpecification(int id, JsonPatchDocument<CarSpecificationUpdateDTO> patchDTO)
        {
            if (patchDTO == null || id == 0)
            {
                return BadRequest();
            }
            var brand = await _unitOfWork.CarSpecification.GetAsync(u => u.Id == id, tracked: false);

            CarSpecificationUpdateDTO carSpecificationDTO = _mapper.Map<CarSpecificationUpdateDTO>(brand);


            if (brand == null)
            {
                return BadRequest();
            }
            patchDTO.ApplyTo(carSpecificationDTO, ModelState);
            CarSpecification model = _mapper.Map<CarSpecification>(carSpecificationDTO);

            await _unitOfWork.CarSpecification.UpdateAsync(model);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return NoContent();
        }


    }
}
