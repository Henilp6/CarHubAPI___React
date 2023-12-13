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
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Data;
using System.Net;
using System.Security.Claims;
using System.Text.Json;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace CarHub_API.Controllers.v1
{
	[Route("api/v{version:apiVersion}/[controller]/[Action]")]
	[ApiController]
	[ApiVersion("1.0")]

	public class CarAPIController : ControllerBase
	{
		protected APIResponse _response;
		private readonly IUnitOfWork _unitOfWork;

		private readonly IMapper _mapper;
		private readonly ApplicationDbContext _db;

		public CarAPIController(IUnitOfWork unitOfWork, IMapper mapper, ApplicationDbContext db)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_response = new();
			_db = db;

		}
		[HttpGet(Name = "GetDataIndex")]
		[ResponseCache(CacheProfileName = "Default30")]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<ActionResult<APIResponse>> GetDataIndex(string term, string orderBy, int currentPage = 1)
		{

			try
			{

				term = string.IsNullOrEmpty(term) ? "" : term.ToLower();

				CarIndexVM carIndexVM = new CarIndexVM();

				{
					var list = _db.Cars.Include("Brand").Include("CarType").ToList(); ;
					carIndexVM.Cars = _mapper.Map<List<CarDTO>>(list);
				}
				carIndexVM.NameSortOrder = string.IsNullOrEmpty(orderBy) ? "typeName_desc" : "";

				var cars = (from data in carIndexVM.Cars.ToList()
							where term == "" ||
							   data.Name.ToLower().
							   Contains(term) || data.Brand.BrandImage.ToLower().
							   Contains(term)


							select new CarDTO
							{
								Id = data.Id,
								Name = data.Name,

								Details = data.Details,

								BrandId = data.BrandId,

								Brand = data.Brand,

								CarType = data.CarType,

								CarTypeId = data.CarTypeId,
								StartingPrice = data.StartingPrice,

								EndPrice = data.EndPrice,

								ManufacturingYear = data.ManufacturingYear,
								IsActive = data.IsActive,

								ImageURL = data.ImageURL,




							});

				switch (orderBy)
				{
					case "typeName_desc":
						cars = cars.OrderByDescending(a => a.Name);
						break;

					default:
						cars = cars.OrderBy(a => a.Name);
						break;
				}
				int totalRecords = cars.Count();
				int pageSize = 5;
				int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
				cars = cars.Skip((currentPage - 1) * pageSize).Take(pageSize);
				// current=1, skip= (1-1=0), take=5 
				// currentPage=2, skip (2-1)*5 = 5, take=5 ,
				carIndexVM.Cars = cars;
				carIndexVM.CurrentPage = currentPage;
				carIndexVM.TotalPages = totalPages;
				carIndexVM.Term = term;
				carIndexVM.PageSize = pageSize;
				carIndexVM.OrderBy = orderBy;
				// return View(stateIndexVM);

				//  Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagination));
				_response.Result = _mapper.Map<CarIndexVM>(carIndexVM);
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

		[HttpGet(Name = "GetCarData")]
		[ResponseCache(CacheProfileName = "Default30")]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<ActionResult<APIResponse>> GetCarData(int pageNum)
		{
			try
			{
				const int RecordsPerPage = 10;

				IEnumerable<Car> carList = await _unitOfWork.Car.GetAllAsync();

				int skip = pageNum * RecordsPerPage;
				var tempList = carList.Skip(skip).Take(RecordsPerPage).ToList();


				_response.Result = _mapper.Map<List<CarDTO>>(tempList);

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



		[HttpGet(Name = "GetCars")]
		[ResponseCache(CacheProfileName = "Default30")]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<ActionResult<APIResponse>> GetCars([FromQuery(Name = "filterDisplayOrder")] int? Id,
		   [FromQuery] string? search, int pageSize = 0, int pageNumber = 1)
		{
			
			
            try
			{

                //IEnumerable<Car> carList;
                IEnumerable<Car> carList = await _unitOfWork.Car.GetAllAsync(includeProperties: "Brand,CarType");

                if (!string.IsNullOrEmpty(search))
                {
                    string datasearch = search.ToLower();
                    carList = carList.Where(u => u.Name.ToLower().Contains(datasearch) || u.Brand.BrandName.ToLower().Contains(datasearch));
                }
                //Pagination pagination = new() { PageNumber = pageNumber, PageSize = pageSize };
                carList = carList.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

                Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(carList));
				_response.Result = _mapper.Map<List<CarDTO>>(carList);
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



		//[HttpGet]
		//[ResponseCache(CacheProfileName = "Default30")]
		//[ProducesResponseType(StatusCodes.Status403Forbidden)]
		//[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		//[ProducesResponseType(StatusCodes.Status200OK)]
		//public async Task<ActionResult<APIResponse>> GetCars()
		//{
		//    try
		//    {

		//        IEnumerable<Car> carList = await _unitOfWork.Car.GetAllAsync(includeProperties: "Brand,CarType");
		//        response.Result = mapper.Map<List<CarDTO>>(carList);
		//        _response.StatusCode = HttpStatusCode.OK;
		//        return Ok(_response);

		//    }
		//    catch (Exception ex)
		//    {
		//        _response.IsSuccess = false;
		//        _response.ErrorMessages
		//             = new List<string>() { ex.ToString() };
		//    }
		//    return _response;

		//}



		[HttpGet("{id:int}", Name = "GetCar")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<APIResponse>> GetCar(int id)
		{
			try
			{
				if (id == 0)
				{
					_response.StatusCode = HttpStatusCode.BadRequest;
					return BadRequest(_response);
				}
				var car = await _unitOfWork.Car.GetAsync(u => u.Id == id, includeProperties: "Brand,CarType");
				if (car == null)
				{
					_response.StatusCode = HttpStatusCode.NotFound;
					return NotFound(_response);
				}
				_response.Result = _mapper.Map<CarDTO>(car);
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
		[HttpPost(Name = "CreateCar")]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult<APIResponse>> CreateCar([FromForm] CarCreateDTO createDTO)
		{
			try
			{

				
				if (await _unitOfWork.Car.GetAsync(u => u.Name.ToLower() == createDTO.Name.ToLower()) != null)
				{
					ModelState.AddModelError("ErrorMessages", "Car already Exists!");
					return BadRequest(ModelState);
				}


				if (await _unitOfWork.Brand.GetAsync(u => u.Id == createDTO.BrandId) == null)
				{
					ModelState.AddModelError("ErrorMessages", "Brand ID is Invalid!");
					return BadRequest(ModelState);
				}
				if (await _unitOfWork.CarType.GetAsync(u => u.Id == createDTO.CarTypeId) == null)
				{
					ModelState.AddModelError("ErrorMessages", "Cartype ID is Invalid!");
					return BadRequest(ModelState);
				}
				if (createDTO == null)
				{
					return BadRequest(createDTO);
				}

				Car car = _mapper.Map<Car>(createDTO);



				await _unitOfWork.Car.CreateAsync(car);
				_response.Result = _mapper.Map<CarDTO>(car);
				_response.StatusCode = HttpStatusCode.Created;
				return CreatedAtRoute("GetCar", new { id = car.Id }, _response);
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
		[HttpDelete("{id:int}", Name = "DeleteCar")]
		public async Task<ActionResult<APIResponse>> DeleteCar(int id)
		{
			try
			{
				if (id == 0)
				{
					return BadRequest();
				}
				var car = await _unitOfWork.Car.GetAsync(u => u.Id == id);
				if (car == null)
				{
					return NotFound();
				}
				await _unitOfWork.Car.RemoveAsync(car);
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
		[HttpPut("{id:int}", Name = "UpdateCar")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<APIResponse>> UpdateCar(int id, [FromForm] CarUpdateDTO updateDTO)
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
				Car model = _mapper.Map<Car>(updateDTO);


				await _unitOfWork.Car.UpdateAsync(model);
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


		[HttpPatch("{id:int}", Name = "UpdatePartialCar")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> UpdatePartialCar(int id, JsonPatchDocument<CarUpdateDTO> patchDTO)
		{
			if (patchDTO == null || id == 0)
			{
				return BadRequest();
			}
			var car = await _unitOfWork.Car.GetAsync(u => u.Id == id, tracked: false);

			CarUpdateDTO carDTO = _mapper.Map<CarUpdateDTO>(car);


			if (car == null)
			{
				return BadRequest();
			}
			patchDTO.ApplyTo(carDTO, ModelState);
			Car model = _mapper.Map<Car>(carDTO);

			await _unitOfWork.Car.UpdateAsync(model);

			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			return NoContent();
		}

		[HttpGet(Name = "CarSearchByLazyLoading")]
		[ResponseCache(CacheProfileName = "Default30")]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<ActionResult<APIResponse>> CarSearchByLazyLoading([FromQuery] int pageNum, string? search)
	{
			


            try
			{
				const int RecordsPerPage = 3;

				IEnumerable<Car> carDTOList = await _unitOfWork.Car.GetAllAsync(includeProperties: "Brand,CarType");

				if (!string.IsNullOrEmpty(search))
				{
					carDTOList = carDTOList.Where(x => x.Name.Contains(search, StringComparison.OrdinalIgnoreCase)
					|| x.Brand.BrandName.Contains(search, StringComparison.OrdinalIgnoreCase));
				}
				int skip = pageNum * RecordsPerPage;
				var tempList = carDTOList.Skip(skip).Take(RecordsPerPage).ToList();

				if (pageNum == 0 && tempList.Count == 0)
				{
					_response.IsSuccess = false;
					_response.ErrorMessages
						 = new List<string>() { "Data does not exists" };
				}
				else
				{
					_response.Result = _mapper.Map<List<CarDTO>>(tempList);

					_response.StatusCode = HttpStatusCode.OK;
					return Ok(_response);
				}

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

