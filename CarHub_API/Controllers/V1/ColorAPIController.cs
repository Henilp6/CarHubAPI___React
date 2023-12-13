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

    public class ColorAPIController : ControllerBase
    {
        protected APIResponse _response;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
		private readonly ApplicationDbContext _db;
		public ColorAPIController(IUnitOfWork unitOfWork, IMapper mapper, ApplicationDbContext db)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _response = new();
            _db = db;

        }
		[HttpGet(Name = "GetColorsData")]
		[ResponseCache(CacheProfileName = "Default30")]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<ActionResult<APIResponse>> GetColorsData(string term, string orderBy, int currentPage = 1)
		{

			try
			{

				term = string.IsNullOrEmpty(term) ? "" : term.ToLower();

				ColorIndexVM colorIndexVM = new ColorIndexVM();

				{
					var list = _db.Colors.ToList();
					colorIndexVM.Colors = _mapper.Map<List<ColorDTO>>(list);
				}
				colorIndexVM.NameSortOrder = string.IsNullOrEmpty(orderBy) ? "colorName_desc" : "";

                var colors = (from data in colorIndexVM.Colors.ToList()
                              where term == "" ||
								 data.ColorName.ToLower().
								 Contains(term)


							  select new ColorDTO
                              {
                                  Id = data.Id,
                                  ColorName = data.ColorName,


                              }); ;

				switch (orderBy)
				{
					case "colorName_desc":
						colors = colors.OrderByDescending(a => a.ColorName);
						break;

					default:
						colors = colors.OrderBy(a => a.ColorName);
						break;
				}
				int totalRecords = colors.Count();
				int pageSize = 5;
				int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
				colors = colors.Skip((currentPage - 1) * pageSize).Take(pageSize);
				// current=1, skip= (1-1=0), take=5 
				// currentPage=2, skip (2-1)*5 = 5, take=5 ,
				colorIndexVM.Colors = colors;
				colorIndexVM.CurrentPage = currentPage;
				colorIndexVM.TotalPages = totalPages;
				colorIndexVM.Term = term;
				colorIndexVM.PageSize = pageSize;
				colorIndexVM.OrderBy = orderBy;
				// return View(stateIndexVM);

				//  Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagination));
				_response.Result = _mapper.Map<ColorIndexVM>(colorIndexVM);
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




		[HttpGet(Name = "GetColors")]
		[MapToApiVersion("1.0")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetColors()
        {
            try
            {

                IEnumerable<Color> colorList = await _unitOfWork.Color.GetAllAsync();
                _response.Result = _mapper.Map<List<ColorDTO>>(colorList);
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


        [HttpGet("{id:int}", Name = "GetColor")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetColor(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var color = await _unitOfWork.Color.GetAsync(u => u.Id == id);
                if (color == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                _response.Result = _mapper.Map<ColorDTO>(color);
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
		[HttpPost(Name = "CreateColor")]
		[ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CreateColor([FromForm] ColorCreateDTO createDTO)
        {
            try
            {
                if (await _unitOfWork.Color.GetAsync(u => u.ColorName.ToLower() == createDTO.ColorName.ToLower()) != null)
                {
                    ModelState.AddModelError("ErrorMessages", "Color already Exists!");
                    return BadRequest(ModelState);
                }



                if (createDTO == null)
                {
                    return BadRequest(createDTO);
                }

                Color color = _mapper.Map<Color>(createDTO);


                await _unitOfWork.Color.CreateAsync(color);
                _response.Result = _mapper.Map<ColorDTO>(color);
                _response.StatusCode = HttpStatusCode.Created;
                return CreatedAtRoute("GetColor", new { id = color.Id }, _response);
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
        [HttpDelete("{id:int}", Name = "DeleteColor")]
        public async Task<ActionResult<APIResponse>> DeleteColor(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }
                var color = await _unitOfWork.Color.GetAsync(u => u.Id == id);
                if (color == null)
                {
                    return NotFound();
                }
                await _unitOfWork.Color.RemoveAsync(color);
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

        [HttpPut("{id:int}", Name = "UpdateColor")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> UpdateColor(int id, [FromForm] ColorUpdateDTO updateDTO)
        {
            try
            {
                if (updateDTO == null || id != updateDTO.Id)
                {
                    return BadRequest();
                }

                if (await _unitOfWork.Color.GetAsync(u => u.ColorName.ToLower() == updateDTO.ColorName.ToLower() && u.Id != updateDTO.Id) != null)
                {
                    ModelState.AddModelError("ErrorMessages", "Color already Exists!");
                    return BadRequest(ModelState);
                }

                Color model = _mapper.Map<Color>(updateDTO);

                await _unitOfWork.Color.UpdateAsync(model);
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
