using AutoMapper;
using Azure;
using CarHub_API.Models;
using CarHub_API.Models.Dto;
using CarHub_API.Models.VM;
using CarHub_API.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Net;
using System.Text.Json;

namespace CarHub_API.Controllers.v1
{
    //[Route("api/[controller]")]
    [Route("api/v{version:apiVersion}/[controller]/[Action]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class CountryAPIController : ControllerBase
    {
        protected APIResponse _response;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CountryAPIController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _response = new();
        }


		[HttpGet(Name = "CountryByPagination")]
		[ResponseCache(CacheProfileName = "Default30")]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<ActionResult<APIResponse>> CountryByPagination(string term, string orderBy, int currentPage = 1)
		{
			try
			{
				term = string.IsNullOrEmpty(term) ? "" : term.ToLower();

				CountryIndexVM countryIndexVM = new CountryIndexVM();
				IEnumerable<Country> list = await _unitOfWork.Country.GetAllAsync();

				var List = _mapper.Map<List<CountryDTO>>(list);

				countryIndexVM.NameSortOrder = string.IsNullOrEmpty(orderBy) ? "countryName_desc" : "";

				if (!string.IsNullOrEmpty(term))
				{
					List = List.Where(u => u.CountryName.ToLower().Contains(term)).ToList();
				}

				switch (orderBy)
				{
					case "countryName_desc":
						List = List.OrderByDescending(a => a.CountryName).ToList();
						break;

					default:
						List = List.OrderBy(a => a.CountryName).ToList();
						break;
				}
				int totalRecords = List.Count();
				int pageSize = 10;
				int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
				List = List.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
				// current=1, skip= (1-1=0), take=5 
				// currentPage=2, skip (2-1)*5 = 5, take=5 ,
				countryIndexVM.Countries = List;
				countryIndexVM.CurrentPage = currentPage;
				countryIndexVM.TotalPages = totalPages;
				countryIndexVM.Term = term;
				countryIndexVM.PageSize = pageSize;
				countryIndexVM.OrderBy = orderBy;

                _response.Result = _mapper.Map<CountryIndexVM>(countryIndexVM);

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

		[HttpGet(Name = "GetCountryData")]
        [ResponseCache(CacheProfileName = "Default30")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetCountryData(int pageNum)
        {
            try
            {
                const int RecordsPerPage = 10;

                IEnumerable<Country> countryList = await _unitOfWork.Country.GetAllAsync();

                int skip = pageNum * RecordsPerPage;
                var tempList = countryList.Skip(skip).Take(RecordsPerPage).ToList();


                _response.Result = _mapper.Map<List<CountryDTO>>(tempList);

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

        [HttpGet(Name = "GetCountrys")]
        [ResponseCache(CacheProfileName = "Default30")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetCountrys(
            [FromQuery] string search, int pageSize = 0, int pageNumber = 1)
        {
            try
            {
                IEnumerable<Country> countryList = await _unitOfWork.Country.GetAllAsync();

                if (!string.IsNullOrEmpty(search))
                {
                    string datasearch = search.ToLower();
                    countryList = countryList.Where(u => u.CountryName.ToLower().Contains(datasearch));
                }
                //Pagination pagination = new() { PageNumber = pageNumber, PageSize = pageSize };
                countryList = countryList.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

                Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(countryList));
                _response.Result = _mapper.Map<List<CountryDTO>>(countryList);
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


        [HttpGet("{id:int}", Name = "GetCountry")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(200, Type =typeof(CategoryDTO))]
        //        [ResponseCache(Location =ResponseCacheLocation.None,NoStore =true)]
        public async Task<ActionResult<APIResponse>> GetCountry(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var country = await _unitOfWork.Country.GetAsync(u => u.Id == id);
                if (country == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                _response.Result = _mapper.Map<CountryDTO>(country);
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

        [HttpPost(Name = "CreateCountry")]
       // [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CreateCountry([FromForm] CountryCreateDTO createDTO)
        {

            try
            {
             
                if (await _unitOfWork.Country.GetAsync(u => u.CountryName.ToLower() == createDTO.CountryName.ToLower()) != null)
                {
                    ModelState.AddModelError("ErrorMessages", "Country already Exists!");
                    return BadRequest(ModelState);
                }

                if (createDTO == null)
                {
                    return BadRequest(createDTO);
                }
              
                Country country = _mapper.Map<Country>(createDTO);
            
                await _unitOfWork.Country.CreateAsync(country);
                _response.Result = _mapper.Map<CountryDTO>(country);
                _response.StatusCode = HttpStatusCode.Created;
                return CreatedAtRoute("GetCountry", new { id = country.Id }, _response);
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
        [HttpDelete("{id:int}", Name = "DeleteCountry")]
     //   [Authorize(Roles = "admin")]
        public async Task<ActionResult<APIResponse>> DeleteCountry(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }
                var country = await _unitOfWork.Country.GetAsync(u => u.Id == id);
                if (country == null)
                {
                    return NotFound();
                }
                await _unitOfWork.Country.RemoveAsync(country);
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
    //    [Authorize(Roles = "admin")]
        [HttpPut("{id:int}", Name = "UpdateCountry")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> UpdateCountry(int id, [FromForm] CountryUpdateDTO updateDTO)
        {
            try
            {
                if (updateDTO == null || id != updateDTO.Id)
                {
                    return BadRequest();
                }

                Country model = _mapper.Map<Country>(updateDTO);
                await _unitOfWork.Country.UpdateAsync(model);
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

        [HttpPatch("{id:int}", Name = "UpdatePartialCountry")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePartialCountry(int id, JsonPatchDocument<CountryUpdateDTO> patchDTO)
        {
            if (patchDTO == null || id == 0)
            {
                return BadRequest();
            }
            var country = await _unitOfWork.Country.GetAsync(u => u.Id == id, tracked: false);

            CountryUpdateDTO countryDTO = _mapper.Map<CountryUpdateDTO>(country);


            if (country == null)
            {
                return BadRequest();
            }
            patchDTO.ApplyTo(countryDTO, ModelState);
            Country model = _mapper.Map<Country>(countryDTO);

            await _unitOfWork.Country.UpdateAsync(model);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return NoContent();
        }


    }
}
