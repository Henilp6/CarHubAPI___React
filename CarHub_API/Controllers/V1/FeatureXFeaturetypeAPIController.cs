using AutoMapper;
using AutoMapper.Features;
using Azure;
using CarHub_API.Data;
using CarHub_API.Models;
using CarHub_API.Models.Dto;
using CarHub_API.Models.VM;
using CarHub_API.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Drawing;
using System.Net;
using System.Security.Claims;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CarHub_API.Controllers.v1
{
	[Route("api/v{version:apiVersion}/[controller]/[Action]")]
	[ApiController]
	[ApiVersion("1.0")]
	public class FeatureXFeaturetypeAPIController : ControllerBase
	{
		protected APIResponse _response;

		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		private readonly ApplicationDbContext _db;

		public FeatureXFeaturetypeAPIController(IUnitOfWork unitOfWork, IMapper mapper, ApplicationDbContext db)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_response = new();
			_db = db;
		}
		[HttpGet(Name = "GetFeatureXFeaturetypeData")]
		[ResponseCache(CacheProfileName = "Default30")]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<ActionResult<APIResponse>> GetFeatureXFeaturetypeData(string term, string orderBy, int currentPage = 1)
		{

			try
			{

				term = string.IsNullOrEmpty(term) ? "" : term.ToLower();

				FeatureXFeaturetypeIndexVM featureXFeaturetypeIndexVM = new FeatureXFeaturetypeIndexVM();

				{
					var list = _db.FeatureXFeaturetypes.Include("Feature").Include("FeatureType").ToList(); ;
					featureXFeaturetypeIndexVM.FeatureXFeaturetypes = _mapper.Map<List<FeatureXFeaturetypeDTO>>(list);
				}
				featureXFeaturetypeIndexVM.NameSortOrder = string.IsNullOrEmpty(orderBy) ? "typeName_desc" : "";

				var featureXFeaturetypes = (from data in featureXFeaturetypeIndexVM.FeatureXFeaturetypes.ToList()
								where term == "" ||
								   data.FeatureType.FeatureTypeName.ToLower().
								   Contains(term) || data.Feature.Name.ToLower().
								   Contains(term)


								select new  FeatureXFeaturetypeDTO
								{
									Id = data.Id,
									FeatureType = data.FeatureType,
									Feature = data.Feature,


								});

				switch (orderBy)
				{
					case "typeName_desc":
						featureXFeaturetypes = featureXFeaturetypes.OrderByDescending(a => a.FeatureType.FeatureTypeName);
						break;

					default:
						featureXFeaturetypes = featureXFeaturetypes.OrderBy(a => a.FeatureType.FeatureTypeName);
						break;
				}
				int totalRecords = featureXFeaturetypes.Count();
				int pageSize = 5;
				int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
				featureXFeaturetypes = featureXFeaturetypes.Skip((currentPage - 1) * pageSize).Take(pageSize);
				// current=1, skip= (1-1=0), take=5 
				// currentPage=2, skip (2-1)*5 = 5, take=5 ,
				featureXFeaturetypeIndexVM.FeatureXFeaturetypes = featureXFeaturetypes;
				featureXFeaturetypeIndexVM.CurrentPage = currentPage;
				featureXFeaturetypeIndexVM.TotalPages = totalPages;
				featureXFeaturetypeIndexVM.Term = term;
				featureXFeaturetypeIndexVM.PageSize = pageSize;
				featureXFeaturetypeIndexVM.OrderBy = orderBy;
				// return View(stateIndexVM);

				//  Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagination));
				_response.Result = _mapper.Map<FeatureXFeaturetypeIndexVM>(featureXFeaturetypeIndexVM);
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


		[HttpGet(Name = "GetFeatureXFeaturetypes")]
		[ResponseCache(CacheProfileName = "Default30")]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<ActionResult<APIResponse>> GetFeatureXFeaturetypes([FromQuery(Name = "filterDisplayOrder")] int? Id,
			[FromQuery] string? search, int pageSize = 0, int pageNumber = 1)
		{
			try
			{
				IEnumerable<FeatureXFeaturetype> featureXFeaturetypeList;
				if (Id > 0)
				{
					featureXFeaturetypeList = await _unitOfWork.FeatureXFeaturetype.GetAllAsync(u => u.Id == Id, includeProperties: "Feature,FeatureType", pageSize: pageSize,
						pageNumber: pageNumber);
				}
				else
				{
					featureXFeaturetypeList = await _unitOfWork.FeatureXFeaturetype.GetAllAsync(includeProperties: "Feature,FeatureType", pageSize: pageSize,
						pageNumber: pageNumber);
				}
				if (!string.IsNullOrEmpty(search))
				{
					featureXFeaturetypeList = featureXFeaturetypeList.Where(u => u.FeatureType.FeatureTypeName.ToLower().Contains(search));
				}
				Pagination pagination = new() { PageNumber = pageNumber, PageSize = pageSize };

				Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagination));
				_response.Result = _mapper.Map<List<FeatureXFeaturetypeDTO>>(featureXFeaturetypeList);
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

		[HttpGet("{id:int}", Name = "FeatureXFeaturetype")]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		//[ProducesResponseType(200, Type =typeof(VillaDTO))]
		//        [ResponseCache(Location =ResponseCacheLocation.None,NoStore =true)]
		public async Task<ActionResult<APIResponse>> GetFeatureXFeaturetype(int id)
		{
			try
			{
				if (id == 0)
				{
					_response.StatusCode = HttpStatusCode.BadRequest;
					return BadRequest(_response);
				}
				var featureXFeaturetype = await _unitOfWork.FeatureXFeaturetype.GetAsync(u => u.Id == id);
				if (featureXFeaturetype == null)
				{
					_response.StatusCode = HttpStatusCode.NotFound;
					return NotFound(_response);
				}
				_response.Result = _mapper.Map<FeatureXFeaturetypeDTO>(featureXFeaturetype);
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

		[HttpPost(Name = "CreateFeatureXFeaturetype")]
		//[Authorize(Roles = "admin")]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult<APIResponse>> CreateFeatureXFeaturetype([FromBody] FeatureXFeaturetypeCreateVM createDTO)
		{
			try
			{

				List<FeatureVM> featureVMList = _mapper.Map<List<FeatureVM>>(createDTO.Featurelist);
				
				//   List<FeatureXFeaturetype> featureXFeaturetypelist = await _unitOfWork.FeatureXFeaturetype.GetAllAsync(u => u.CarId == car.Id && u.FeatureTypeId== createDTO.FeatureXFeaturetype.FeatureTypeId, includeProperties: "Car,FeatureType,Feature");
				FeatureXFeaturetype featureXfeaturetype = _mapper.Map<FeatureXFeaturetype>(createDTO.FeatureXFeaturetype);


				await _unitOfWork.FeatureXFeaturetype.RemoveRangeAsync( u=> u.FeatureTypeId == createDTO.FeatureXFeaturetype.FeatureTypeId, false);

				foreach (var featurelist in featureVMList)
				{
					if (featurelist.IsChecked == true)
					{
						FeatureXFeaturetype featureXFeaturetype = new();

						
						featureXFeaturetype.FeatureId = featurelist.Id;
						featureXFeaturetype.FeatureTypeId = featureXfeaturetype.FeatureTypeId;

						await _unitOfWork.FeatureXFeaturetype.CreateAsync(featureXFeaturetype);

					}
					else
					{
						continue;
					}

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
		[HttpDelete("{id:int}", Name = "DeleteFeatureXFeaturetype")]
		//[Authorize(Roles = "admin")]
		public async Task<ActionResult<APIResponse>> DeleteFeatureXFeaturetype(int id)
		{
			try
			{
				if (id == 0)
				{
					return BadRequest();
				}
				var featureXFeaturetype = await _unitOfWork.FeatureXFeaturetype.GetAsync(u => u.Id == id);
				if (featureXFeaturetype == null)
				{
					return NotFound();
				}
				await _unitOfWork.FeatureXFeaturetype.RemoveAsync(featureXFeaturetype);
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
		[HttpPut(Name = "UpdateFeatureXFeaturetype")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<APIResponse>> UpdateFeatureXFeaturetype([FromBody] FeatureXFeaturetypeUpdateVM updateDTO)
		{
			try
			{
				List<FeatureVM> featureVMList = _mapper.Map<List<FeatureVM>>(updateDTO.Featurelist);
				FeatureXFeaturetype featureXFeaturetype = _mapper.Map<FeatureXFeaturetype>(updateDTO.FeatureXFeaturetype);

				List<FeatureXFeaturetype> featureXFeaturetypelist = await _unitOfWork.FeatureXFeaturetype.GetAllAsync(u => u.FeatureTypeId == featureXFeaturetype.FeatureTypeId, includeProperties: "Feature,FeatureType");
				foreach (var carlist in featureXFeaturetypelist)
				{
					FeatureXFeaturetype featureXfeaturetype = new();

					featureXfeaturetype.Id = carlist.Id;
					featureXfeaturetype.FeatureId = carlist.FeatureId;
					featureXfeaturetype.FeatureTypeId = carlist.FeatureTypeId;
					await _unitOfWork.FeatureXFeaturetype.RemoveAsync(featureXfeaturetype);
				}

				foreach (var featurelist in featureVMList)
				{

					if (featurelist.IsChecked == true)
					{
						FeatureXFeaturetype featureXfeaturetype = new();

						
						featureXfeaturetype.FeatureTypeId = featureXFeaturetype.FeatureTypeId;
						featureXfeaturetype.FeatureId = featurelist.Id;

						await _unitOfWork.FeatureXFeaturetype.CreateAsync(featureXfeaturetype);

					}
					else
					{
						continue;
					}

				}







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

		[HttpPatch("{id:int}", Name = "UpdatePartialFeatureXFeaturetype")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> UpdatePartialFeatureXFeaturetype(int id, JsonPatchDocument<FeatureXFeaturetypeUpdateDTO> patchDTO)
		{
			if (patchDTO == null || id == 0)
			{
				return BadRequest();
			}
			var brand = await _unitOfWork.FeatureXFeaturetype.GetAsync(u => u.Id == id, tracked: false);

			FeatureXFeaturetypeUpdateDTO featureXFeaturetypeDTO = _mapper.Map<FeatureXFeaturetypeUpdateDTO>(brand);


			if (brand == null)
			{
				return BadRequest();
			}
			patchDTO.ApplyTo(featureXFeaturetypeDTO, ModelState);
			FeatureXFeaturetype model = _mapper.Map<FeatureXFeaturetype>(featureXFeaturetypeDTO);

			await _unitOfWork.FeatureXFeaturetype.UpdateAsync(model);

			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			return NoContent();
		}


	}
}
