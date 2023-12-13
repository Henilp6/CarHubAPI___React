using AutoMapper;
using Azure;
using CarHub_API.Models;
using CarHub_API.Models.Dto;
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
    public class DealerAPIController : ControllerBase
    {
        protected APIResponse _response;

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
		public DealerAPIController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _response = new();
        }


        [HttpGet]
        [ResponseCache(CacheProfileName = "Default30")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetDealers([FromQuery(Name = "filterDisplayOrder")] int? Id,
            [FromQuery] string? search, int pageSize = 0, int pageNumber = 1)
        {
            try
            {

                IEnumerable<Dealer> dealerList;

                if (Id > 0)
                {
					dealerList = await _unitOfWork.Dealer.GetAllAsync(u => u.Id == Id, includeProperties: "Brand", pageSize: pageSize,
                        pageNumber: pageNumber);
                }
                else
                {
					dealerList = await _unitOfWork.Dealer.GetAllAsync(includeProperties: "Brand", pageSize: pageSize,
                        pageNumber: pageNumber);
                }
                if (!string.IsNullOrEmpty(search))
                {
					dealerList = dealerList.Where(u => u.Brand.BrandName.ToLower().Contains(search));
                }
                Pagination pagination = new() { PageNumber = pageNumber, PageSize = pageSize };

                Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagination));
                _response.Result = _mapper.Map<List<DealerDTO>>(dealerList);
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

        [HttpGet("{id:int}", Name = "GetDealer")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(200, Type =typeof(VillaDTO))]
        //        [ResponseCache(Location =ResponseCacheLocation.None,NoStore =true)]
        public async Task<ActionResult<APIResponse>> GetDealer(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var dealer = await _unitOfWork.Dealer.GetAsync(u => u.Id == id);
                if (dealer == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                _response.Result = _mapper.Map<DealerDTO>(dealer);
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

        [HttpPost]
        //[Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CreateDealer([FromBody] DealerCreateDTO createDTO)
        {
            try
            {
                //var claimsIdentity = (ClaimsIdentity)User.Identity;
                //var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
                //string ApplicationUserId = userId;
                //if (!ModelState.IsValid)
                //{
                //    return BadRequest(ModelState);
                //}
                //if (await _unitOfWork.CarSpecification.GetAsync(u => u.BrandName.ToLower() == createDTO.BrandName.ToLower()) != null)
                //{
                //    ModelState.AddModelError("ErrorMessages", "Category already Exists!");
                //    return BadRequest(ModelState);
                //}
                if (await _unitOfWork.Dealer.GetAsync(u => u.BrandId == createDTO.BrandId) != null)
                {
                    ModelState.AddModelError("ErrorMessages", "Dealer already Exists!");
                    return BadRequest(ModelState);
                }


                if (await _unitOfWork.Brand.GetAsync(u => u.Id == createDTO.BrandId) == null)
                {
                    ModelState.AddModelError("ErrorMessages", "Brand ID is Invalid!");
                    return BadRequest(ModelState);
                }

                if (createDTO == null)
                {
                    return BadRequest(createDTO);
                }

				Dealer dealer = _mapper.Map<Dealer>(createDTO);



                await _unitOfWork.Dealer.CreateAsync(dealer);
                _response.Result = _mapper.Map<DealerDTO>(dealer);
                _response.StatusCode = HttpStatusCode.Created;
                return CreatedAtRoute("GetDealer", new { id = dealer.Id }, _response);
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
        [HttpDelete("{id:int}", Name = "DeleteDealer")]
        //[Authorize(Roles = "admin")]
        public async Task<ActionResult<APIResponse>> DeleteDealer(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }
                var dealer = await _unitOfWork.Dealer.GetAsync(u => u.Id == id);
                if (dealer == null)
                {
                    return NotFound();
                }
                await _unitOfWork.Dealer.RemoveAsync(dealer);
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
        [HttpPut("{id:int}", Name = "UpdateDealer")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> UpdateDealer(int id, [FromBody] DealerUpdateDTO updateDTO)
        {
            try
            {
                if (updateDTO == null || id != updateDTO.Id)
                {
                    return BadRequest();
                }

				Dealer model = _mapper.Map<Dealer>(updateDTO);

                await _unitOfWork.Dealer.UpdateAsync(model);
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

        [HttpPatch("{id:int}", Name = "UpdatePartialDealer")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePartialDealer(int id, JsonPatchDocument<DealerUpdateDTO> patchDTO)
        {
            if (patchDTO == null || id == 0)
            {
                return BadRequest();
            }
            var brand = await _unitOfWork.Dealer.GetAsync(u => u.Id == id, tracked: false);

			DealerUpdateDTO dealerDTO = _mapper.Map<DealerUpdateDTO>(brand);


            if (brand == null)
            {
                return BadRequest();
            }
            patchDTO.ApplyTo(dealerDTO, ModelState);
			Dealer model = _mapper.Map<Dealer>(dealerDTO);

            await _unitOfWork.Dealer.UpdateAsync(model);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return NoContent();
        }


    }
}
