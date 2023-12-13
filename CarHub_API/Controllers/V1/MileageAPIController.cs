using AutoMapper;
using Azure;
using CarHub_API.Models;
using CarHub_API.Models.Dto;
using CarHub_API.Models.VM;
using CarHub_API.Repository.IRepository;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace CarHub_API.Controllers.V1
{
    [Route("api/v{version:apiVersion}/[controller]/[Action]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class MileageAPIController : ControllerBase
    {
        protected APIResponse _response;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MileageAPIController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _response = new();
            



        }
        [HttpGet(Name = "MileageByPagination")]
        [ResponseCache(CacheProfileName = "Default30")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> MileageByPagination(string term, string orderBy, int currentPage = 1)
        {
            try
            {
                term = string.IsNullOrEmpty(term) ? "" : term.ToLower();

                MileageIndexVM mileageIndexVM = new MileageIndexVM();
                IEnumerable<Mileage> list = await _unitOfWork.Mileage.GetAllAsync(includeProperties: "Car");

                var List = _mapper.Map<List<MileageDTO>>(list);

                mileageIndexVM.NameSortOrder = string.IsNullOrEmpty(orderBy) ? "carName_desc" : "";

                if (!string.IsNullOrEmpty(term))
                {
                    List = List.Where(u => u.Car.Name.ToLower().Contains(term)).ToList();
                   
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
                mileageIndexVM.Mileages = List;
                mileageIndexVM.CurrentPage = currentPage;
                mileageIndexVM.TotalPages = totalPages;
                mileageIndexVM.Term = term;
                mileageIndexVM.PageSize = pageSize;
                mileageIndexVM.OrderBy = orderBy;

                _response.Result = _mapper.Map<MileageIndexVM>(mileageIndexVM);
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




        [HttpGet(Name = "GetMileages")]
        [ResponseCache(CacheProfileName = "Default30")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetMileages()
        {
            try
            {

                IEnumerable<Mileage> mileageList = await _unitOfWork.Mileage.GetAllAsync(includeProperties: "Car");
                _response.Result = _mapper.Map<List<MileageDTO>>(mileageList);
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



        [HttpGet("{id:int}", Name = "GetMileage")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetMileage(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var mileage = await _unitOfWork.Mileage.GetAsync(u => u.CarId == id, includeProperties: "Car");
                if (mileage == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                _response.Result = _mapper.Map<MileageDTO>(mileage);
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
        [HttpPost(Name = "CreateMileage")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CreateMileage([FromBody] MileageCreateVM createDTO)
        {
            try
            {

               
                Car car = _mapper.Map<Car>(createDTO.Car);
                //   List<CarXFeature> carXFeaturelist = await _unitOfWork.CarXFeature.GetAllAsync(u => u.CarId == car.Id && u.FeatureTypeId== createDTO.CarXFeature.FeatureTypeId, includeProperties: "Car,FeatureType,Feature");
                Mileage mileage = _mapper.Map<Mileage>(createDTO.Mileage);


                await _unitOfWork.Mileage.RemoveRangeAsync(u=>u.CarId == createDTO.Car.Id, false);





                Mileage mileage1 = new();

                mileage1.CarId = car.Id;
                mileage1.Transmission = mileage.Transmission;
                mileage1.FuelType = mileage.FuelType;
                mileage1.Average = mileage.Average;


                await _unitOfWork.Mileage.CreateAsync(mileage1);

                  
                   
    
                        
                   

                




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

        //[Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete("{id:int}", Name = "DeleteMileage")]
        public async Task<ActionResult<APIResponse>> DeleteMileage(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }
                var mileage = await _unitOfWork.Mileage.GetAsync(u => u.Id == id);
                if (mileage == null)
                {
                    return NotFound();
                }
                await _unitOfWork.Mileage.RemoveAsync(mileage);
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

        //[Authorize(Roles = "admin")]
        [HttpPut("{id:int}", Name = "UpdateMileage")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> UpdateMileage(int id, [FromBody] MileageUpdateDTO updateDTO)
        {
            try
            {
                if (updateDTO == null || id != updateDTO.Id)
                {
                    return BadRequest();
                }
                //if (await _dbCategory.GetAsync(u => u.Id == updateDTO.CategoryId,false) == null)
                //{
                //	ModelState.AddModelError("ErrorMessages", "Category ID is Invalid!");
                //	return BadRequest(ModelState);
                //}
                Mileage model = _mapper.Map<Mileage>(updateDTO);


                await _unitOfWork.Mileage.UpdateAsync(model);
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

