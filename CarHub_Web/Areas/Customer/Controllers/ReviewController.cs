using AutoMapper;
using CarHub_Utility;
using CarHub_Web.Models;
using CarHub_Web.Models.Dto;
using CarHub_Web.Models.VM;
using CarHub_Web.Service;
using CarHub_Web.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Security.Claims;

namespace CarHub_Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    //[Authorize(Roles = "SuperAdmin,Admin")]
    public class ReviewController : Controller
    {
        private readonly ICarService _carService;
        private readonly IReviewService _reviewService;
        private readonly IReviewXCommentService _reviewXCommentService;




        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IMapper _mapper;
        public ReviewController(ICarService carService, IWebHostEnvironment webHostEnvironment, IMapper mapper, IReviewService reviewService,
          IReviewXCommentService reviewXCommentService)
        {
            _carService = carService;

            _mapper = mapper;
            _reviewService = reviewService;
            _webHostEnvironment = webHostEnvironment;
            _reviewXCommentService = reviewXCommentService;



        }

        public async Task<IActionResult> IndexReview()
        {
            List<ReviewDTO> list = new();

            var response = await _reviewService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<ReviewDTO>>(Convert.ToString(response.Result));
            }
            return View(list);
        }

        // [Authorize(Roles = "admin")]
        
        public async Task<IActionResult> CreateReview(int carId)
        {
            ReviewCreateVM reviewVM = new();

            var response = await _carService.GetAsync<APIResponse>(carId, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                reviewVM.Car = JsonConvert.DeserializeObject<CarDTO>(Convert.ToString(response.Result));

            }
            return View(reviewVM);
        }

        //[Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateReview(ReviewCreateVM model)
        {
            ReviewCreateVM reviewVM = new();
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            string ApplicationUserId = userId;

            model.Review.ApplicationUserId = ApplicationUserId;

            var response = await _reviewService.CreateAsync<APIResponse>(model, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                return RedirectToAction("Details", "Home", new { carId = model.Car.Id, area = "Customer" });

            }
            return View(reviewVM);
        }


        public async Task<IActionResult> LikeCount(int reviewid)
        {
            ReviewUpdateVM reviewUpdateVM = new();

            var response = await _reviewService.GetAsync<APIResponse>(reviewid, HttpContext.Session.GetString(SD.SessionToken));

            if (response != null && response.IsSuccess)
            {
                var model = JsonConvert.DeserializeObject<ReviewDTO>(Convert.ToString(response.Result));
                reviewUpdateVM.Review = _mapper.Map<ReviewUpdateDTO>(model);
                reviewUpdateVM.Review.LikeCount += 1;

            }


            var response1 = await _reviewService.UpdateAsync<APIResponse>(reviewUpdateVM.Review, HttpContext.Session.GetString(SD.SessionToken));
            if (response1 != null && response1.IsSuccess)
            {
                TempData["success"] = " Like successfully";
                return RedirectToAction("Details", "Home", new { carId = reviewUpdateVM.Review.CarId, area = "Customer" });

            }
            return NotFound();

        }
        public async Task<IActionResult> DisLikeCount(int reviewid)
        {
            ReviewUpdateVM reviewUpdateVM = new();

            var response = await _reviewService.GetAsync<APIResponse>(reviewid, HttpContext.Session.GetString(SD.SessionToken));

            if (response != null && response.IsSuccess)
            {
                var model = JsonConvert.DeserializeObject<ReviewDTO>(Convert.ToString(response.Result));
                reviewUpdateVM.Review = _mapper.Map<ReviewUpdateDTO>(model);
                reviewUpdateVM.Review.DisLikeCount += 1;

            }


            var response1 = await _reviewService.UpdateAsync<APIResponse>(reviewUpdateVM.Review, HttpContext.Session.GetString(SD.SessionToken));
            if (response1 != null && response1.IsSuccess)
            {
                TempData["success"] = " DisLike successfully";
                return RedirectToAction("Details", "Home", new { carId = reviewUpdateVM.Review.CarId, area = "Customer" });

            }
            return NotFound();

        }
        
        public async Task<IActionResult> ReviewXComment(int reviewId, int carId)
        {
            HomeVM homeVM = new HomeVM();

            homeVM.ReviewXComment.ReviewId = reviewId;
            var response = await _carService.GetAsync<APIResponse>(carId, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                homeVM.Car = JsonConvert.DeserializeObject<CarDTO>(Convert.ToString(response.Result));

            }
            var response1 = await _reviewService.GetAsync<APIResponse>(reviewId, HttpContext.Session.GetString(SD.SessionToken));
            if (response1 != null && response1.IsSuccess)
            {
                homeVM.Review = JsonConvert.DeserializeObject<ReviewDTO>(Convert.ToString(response1.Result));
            }
            return View(homeVM);
        }



            

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReviewXComment(HomeVM homeVM)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            string ApplicationUserId = userId;

            HomeVM homeVM1 = new HomeVM();
            homeVM1.ReviewXComment.ReviewId= homeVM.Review.Id;
            homeVM1.ReviewXComment.Comment = homeVM.ReviewXComment.Comment;
            
            homeVM1.ReviewXComment.ApplicationUserId = ApplicationUserId;
            var response = await _reviewXCommentService.CreateAsync<APIResponse>(homeVM1.ReviewXComment, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                return RedirectToAction("Details", "Home", new { carId = homeVM.Car.Id, area = "Customer" });

            }
            return NotFound();

        }



        public async Task<IActionResult> DeleteReview(int reviewid)
        {

            var response = await _reviewService.DeleteAsync<APIResponse>(reviewid, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Review deleted successfully";
                return RedirectToAction(nameof(IndexReview));
            }
            TempData["error"] = "Error encountered.";
            return RedirectToAction(nameof(IndexReview));
        }



    }
}
