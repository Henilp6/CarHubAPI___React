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
    [Route("api/v{version:apiVersion}/ReviewXCommentAPI")]
    [ApiController]
    [ApiVersion("1.0")]
    public class ReviewXCommentAPIController : ControllerBase
    {
        protected APIResponse _response;

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
		public ReviewXCommentAPIController(IUnitOfWork unitOfWork, IMapper mapper)
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
        public async Task<ActionResult<APIResponse>> GetReviewXComments([FromQuery(Name = "filterDisplayOrder")] int? Id,
            [FromQuery] string? search, int pageSize = 0, int pageNumber = 1)
        {
            try
            {

                IEnumerable<ReviewXComment> reviewXCommentList;

                if (Id > 0)
                {
					reviewXCommentList = await _unitOfWork.ReviewXComment.GetAllAsync(u => u.Id == Id, includeProperties: "Review,ApplicationUser", pageSize: pageSize,
                        pageNumber: pageNumber);
                }
                else
                {
					reviewXCommentList = await _unitOfWork.ReviewXComment.GetAllAsync(includeProperties: "Review,ApplicationUser", pageSize: pageSize,
                        pageNumber: pageNumber);
                }
     //           if (!string.IsNullOrEmpty(search))
     //           {
					//reviewXCommentList = reviewXCommentList.Where(u => u.Review.ReviewName.ToLower().Contains(search));
     //           }
                Pagination pagination = new() { PageNumber = pageNumber, PageSize = pageSize };

                Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagination));
                _response.Result = _mapper.Map<List<ReviewXCommentDTO>>(reviewXCommentList);
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

        [HttpGet("{id:int}", Name = "GetReviewXComment")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(200, Type =typeof(VillaDTO))]
        //        [ResponseCache(Location =ResponseCacheLocation.None,NoStore =true)]
        public async Task<ActionResult<APIResponse>> GetReviewXComment(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var reviewXComment = await _unitOfWork.ReviewXComment.GetAsync(u => u.Id == id);
                if (reviewXComment == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                _response.Result = _mapper.Map<ReviewXCommentDTO>(reviewXComment);
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
        public async Task<ActionResult<APIResponse>> CreateReviewXComment([FromBody] ReviewXCommentCreateDTO createDTO)
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
                //if (await _unitOfWork.CarSpecification.GetAsync(u => u.ReviewName.ToLower() == createDTO.ReviewName.ToLower()) != null)
                //{
                //    ModelState.AddModelError("ErrorMessages", "Category already Exists!");
                //    return BadRequest(ModelState);
                //}
                //if (await _unitOfWork.ReviewXComment.GetAsync(u => u.ReviewId == createDTO.ReviewId) != null)
                //{
                //    ModelState.AddModelError("ErrorMessages", "ReviewXComment already Exists!");
                //    return BadRequest(ModelState);
                //}


                if (await _unitOfWork.Review.GetAsync(u => u.Id == createDTO.ReviewId) == null)
                {
                    ModelState.AddModelError("ErrorMessages", "Review ID is Invalid!");
                    return BadRequest(ModelState);
                }

                if (createDTO == null)
                {
                    return BadRequest(createDTO);
                }

				ReviewXComment reviewXComment = _mapper.Map<ReviewXComment>(createDTO);



                await _unitOfWork.ReviewXComment.CreateAsync(reviewXComment);
                _response.Result = _mapper.Map<ReviewXCommentDTO>(reviewXComment);
                _response.StatusCode = HttpStatusCode.Created;
                return CreatedAtRoute("GetReviewXComment", new { id = reviewXComment.Id }, _response);
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
        [HttpDelete("{id:int}", Name = "DeleteReviewXComment")]
        //[Authorize(Roles = "admin")]
        public async Task<ActionResult<APIResponse>> DeleteReviewXComment(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }
                var reviewXComment = await _unitOfWork.ReviewXComment.GetAsync(u => u.Id == id);
                if (reviewXComment == null)
                {
                    return NotFound();
                }
                await _unitOfWork.ReviewXComment.RemoveAsync(reviewXComment);
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
        [HttpPut("{id:int}", Name = "UpdateReviewXComment")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> UpdateReviewXComment(int id, [FromBody] ReviewXCommentUpdateDTO updateDTO)
        {
            try
            {
                if (updateDTO == null || id != updateDTO.Id)
                {
                    return BadRequest();
                }

				ReviewXComment model = _mapper.Map<ReviewXComment>(updateDTO);

                await _unitOfWork.ReviewXComment.UpdateAsync(model);
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

        [HttpPatch("{id:int}", Name = "UpdatePartialReviewXComment")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePartialReviewXComment(int id, JsonPatchDocument<ReviewXCommentUpdateDTO> patchDTO)
        {
            if (patchDTO == null || id == 0)
            {
                return BadRequest();
            }
            var brand = await _unitOfWork.ReviewXComment.GetAsync(u => u.Id == id, tracked: false);

			ReviewXCommentUpdateDTO reviewXCommentDTO = _mapper.Map<ReviewXCommentUpdateDTO>(brand);


            if (brand == null)
            {
                return BadRequest();
            }
            patchDTO.ApplyTo(reviewXCommentDTO, ModelState);
			ReviewXComment model = _mapper.Map<ReviewXComment>(reviewXCommentDTO);

            await _unitOfWork.ReviewXComment.UpdateAsync(model);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return NoContent();
        }


    }
}
