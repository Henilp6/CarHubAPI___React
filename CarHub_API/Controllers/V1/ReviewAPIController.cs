using AutoMapper;
using Azure;
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

namespace ReviewHub_API.Controllers.v1
{
    [Route("api/v{version:apiVersion}/ReviewAPI")]
    [ApiController]
    [ApiVersion("1.0")]

    public class ReviewAPIController : ControllerBase
    {
        protected APIResponse _response;
        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;

        public ReviewAPIController(IUnitOfWork unitOfWork, IMapper mapper)
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
        public async Task<ActionResult<APIResponse>> GetReviews([FromQuery(Name = "filterDisplayOrder")] int? Id,
           [FromQuery] string? search, int pageSize = 0, int pageNumber = 1)
        {
            try
            {

                IEnumerable<Review> reviewList;

                if (Id > 0)
                {

                    reviewList = await _unitOfWork.Review.GetAllAsync(u => u.Id == Id, includeProperties: "Car", pageSize: pageSize,
                        pageNumber: pageNumber);
                }
                else
                {
                    reviewList = await _unitOfWork.Review.GetAllAsync(includeProperties: "Car", pageSize: pageSize,
                        pageNumber: pageNumber);
                }
                if (!string.IsNullOrEmpty(search))
                {
                    reviewList = reviewList.Where(u => u.Title.ToLower().Contains(search) ||
                                                 u.Car.Name.ToLower().Contains(search));

                }
                Pagination pagination = new() { PageNumber = pageNumber, PageSize = pageSize };

                Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagination));
                _response.Result = _mapper.Map<List<ReviewDTO>>(reviewList);
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
        //public async Task<ActionResult<APIResponse>> GetReviews()
        //{
        //    try
        //    {

        //        IEnumerable<Review> reviewList = await _unitOfWork.Review.GetAllAsync(includeProperties: "Brand,ReviewType");
        //        response.Result = mapper.Map<List<ReviewDTO>>(reviewList);
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



        [HttpGet("{id:int}", Name = "GetReview")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetReview(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var review = await _unitOfWork.Review.GetAsync(u => u.Id == id, includeProperties: "Car");
                if (review == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                _response.Result = _mapper.Map<ReviewDTO>(review);
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
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CreateReview([FromBody] ReviewCreateVM createDTO)
        {
            try
            {

            
                if (createDTO == null)
                {
                    return BadRequest(createDTO);
                }


                Review review = _mapper.Map<Review>(createDTO.Review);
                review.CarId = createDTO.Car.Id;


                await _unitOfWork.Review.CreateAsync(review);
                _response.Result = _mapper.Map<ReviewDTO>(review);
                _response.StatusCode = HttpStatusCode.Created;
                return Ok(_response);
               // return CreatedAtRoute("GetReview", new { id = review.Id }, _response);
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
        [HttpDelete("{id:int}", Name = "DeleteReview")]
        public async Task<ActionResult<APIResponse>> DeleteReview(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }
                var review = await _unitOfWork.Review.GetAsync(u => u.Id == id);
                if (review == null)
                {
                 return NotFound();
                }
                await _unitOfWork.Review.RemoveAsync(review);
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
        [HttpPut("{id:int}", Name = "UpdateReview")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> UpdateReview(int id, [FromBody] ReviewUpdateDTO updateDTO)
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
                Review model = _mapper.Map<Review>(updateDTO);


                await _unitOfWork.Review.UpdateAsync(model);
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


        [HttpPatch("{id:int}", Name = "UpdatePartialReview")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePartialReview(int id, JsonPatchDocument<ReviewUpdateDTO> patchDTO)
        {
            if (patchDTO == null || id == 0)
            {
                return BadRequest();
            }
            var review = await _unitOfWork.Review.GetAsync(u => u.Id == id, tracked: false);

            ReviewUpdateDTO reviewDTO = _mapper.Map<ReviewUpdateDTO>(review);


            if (review == null)
            {
                return BadRequest();
            }
            patchDTO.ApplyTo(reviewDTO, ModelState);
            Review model = _mapper.Map<Review>(reviewDTO);

            await _unitOfWork.Review.UpdateAsync(model);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return NoContent();
        }
    }
}

