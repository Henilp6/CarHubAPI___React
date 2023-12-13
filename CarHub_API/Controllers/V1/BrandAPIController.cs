using AutoMapper;
using Azure;
using CarHub_API.Data;
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
    public class BrandAPIController : ControllerBase
    {
        protected APIResponse _response;

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
		private readonly ApplicationDbContext _db;
		public BrandAPIController(IUnitOfWork unitOfWork, IMapper mapper, ApplicationDbContext db)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
			_db = db;
			_response = new();
        }
		[HttpGet(Name = "GetBrandIndex")]
		[ResponseCache(CacheProfileName = "Default30")]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<ActionResult<APIResponse>> GetBrandIndex(string term, string orderBy, int currentPage = 1)
		{

			try
			{

				term = string.IsNullOrEmpty(term) ? "" : term.ToLower();

				BrandIndexVM brandIndexVM = new BrandIndexVM();

				{
					var list = _db.Brands.ToList();
					brandIndexVM.Brands = _mapper.Map<List<BrandDTO>>(list);
				}
				brandIndexVM.NameSortOrder = string.IsNullOrEmpty(orderBy) ? "brandName_desc" : "";

				var brands = (from data in brandIndexVM.Brands.ToList()
							  where term == "" ||
								 data.BrandName.ToLower().
								 Contains(term)


							  select new BrandDTO
							  {
								  Id = data.Id,
								  BrandName = data.BrandName,
                                  BrandImage = data.BrandImage,
                                  IsDelete = data.IsDelete,
								  IsActive = data.IsActive,
								 
							  });

				switch (orderBy)
				{
					case "brandName_desc":
						brands = brands.OrderByDescending(a => a.BrandName);
						break;

					default:
						brands = brands.OrderBy(a => a.BrandName);
						break;
				}
				int totalRecords = brands.Count();
				int pageSize = 5;
				int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
				brands = brands.Skip((currentPage - 1) * pageSize).Take(pageSize);
				// current=1, skip= (1-1=0), take=5 
				// currentPage=2, skip (2-1)*5 = 5, take=5 ,
				brandIndexVM.Brands = brands;
				brandIndexVM.CurrentPage = currentPage;
				brandIndexVM.TotalPages = totalPages;
				brandIndexVM.Term = term;
				brandIndexVM.PageSize = pageSize;
				brandIndexVM.OrderBy = orderBy;
				// return View(stateIndexVM);

				//  Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagination));
				_response.Result = _mapper.Map<BrandIndexVM>(brandIndexVM);
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

		[HttpGet(Name ="GetBrands")]
        [ResponseCache(CacheProfileName = "Default30")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetBrands([FromQuery(Name = "filterDisplayOrder")] int? Id,
            [FromQuery] string? search, int pageSize = 0, int pageNumber = 1)
        {
            try
            {

                IEnumerable<Brand> brandList;

                if (Id > 0)
                {
                    brandList = await _unitOfWork.Brand.GetAllAsync(u => u.Id == Id, pageSize: pageSize,
                        pageNumber: pageNumber);
                }
                else
                {
                    brandList = await _unitOfWork.Brand.GetAllAsync(pageSize: pageSize,
                        pageNumber: pageNumber);
                }
                if (!string.IsNullOrEmpty(search))
                {
                    brandList = brandList.Where(u => u.BrandName.ToLower().Contains(search));
                }
                Pagination pagination = new() { PageNumber = pageNumber, PageSize = pageSize };

                Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagination));
                _response.Result = _mapper.Map<List<BrandDTO>>(brandList);
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

        [HttpGet("{id:int}", Name = "GetBrand")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(200, Type =typeof(VillaDTO))]
        //        [ResponseCache(Location =ResponseCacheLocation.None,NoStore =true)]
        public async Task<ActionResult<APIResponse>> GetBrand(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var brand = await _unitOfWork.Brand.GetAsync(u => u.Id == id);
                if (brand == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
               _response.Result = _mapper.Map<BrandDTO>(brand);
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

        [HttpPost(Name ="CreateBrand")]
        //[Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CreateBrand([FromForm] BrandCreateDTO createDTO)
        {
            try
            {
                if (await _unitOfWork.Brand.GetAsync(u => u.BrandName.ToLower() == createDTO.BrandName.ToLower()) != null)
                {
                    ModelState.AddModelError("ErrorMessages", "Category already Exists!");
                    return BadRequest(ModelState);
                }

                if (createDTO == null)
                {
                    return BadRequest(createDTO);
                }
                var file = createDTO.BrandImage;
                if (file != null && file.Length > 0)
                {
                    var filePath = "/uploads/" + Guid.NewGuid().ToString() + "_" + file;
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                       
                    }

                    // Update the DTO with the file path
                    createDTO.BrandImage = filePath;
                }

                Brand brand = _mapper.Map<Brand>(createDTO);

               
                await _unitOfWork.Brand.CreateAsync(brand);
                _response.Result = _mapper.Map<BrandDTO>(brand);
                _response.StatusCode = HttpStatusCode.Created;
                return CreatedAtRoute("GetBrand", new { id = brand.Id }, _response);
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
        [HttpDelete("{id:int}", Name = "DeleteBrand")]
        //[Authorize(Roles = "admin")]
        public async Task<ActionResult<APIResponse>> DeleteBrand(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }
                var category = await _unitOfWork.Brand.GetAsync(u => u.Id == id);
                if (category == null)
                {
                    return NotFound();
                }
                await _unitOfWork.Brand.RemoveAsync(category);
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
        [HttpPut("{id:int}", Name = "UpdateBrand")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> UpdateBrand(int id, [FromForm] BrandUpdateDTO updateDTO)
        {
            try
            {
                if (updateDTO == null || id != updateDTO.Id)
                {
                    return BadRequest();
                }
                if (await _unitOfWork.Brand.GetAsync(u => u.BrandName.ToLower() == updateDTO.BrandName.ToLower() && u.Id != updateDTO.Id) != null)
                {
                    ModelState.AddModelError("ErrorMessages", "Brand already Exists!");
                    return BadRequest(ModelState);
                }

                Brand model = _mapper.Map<Brand>(updateDTO);

                await _unitOfWork.Brand.UpdateAsync(model);
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

        [HttpPatch("{id:int}", Name = "UpdatePartialBrand")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePartialCategory(int id, JsonPatchDocument<BrandUpdateDTO> patchDTO)
        {
            if (patchDTO == null || id == 0)
            {
                return BadRequest();
            }
            var brand = await _unitOfWork.Brand.GetAsync(u => u.Id == id, tracked: false);

            BrandUpdateDTO brandDTO = _mapper.Map<BrandUpdateDTO>(brand);


            if (brand == null)
            {
                return BadRequest();
            }
            patchDTO.ApplyTo(brandDTO, ModelState);
            Brand model = _mapper.Map<Brand>(brandDTO);

            await _unitOfWork.Brand.UpdateAsync(model);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return NoContent();
        }


    }
}

