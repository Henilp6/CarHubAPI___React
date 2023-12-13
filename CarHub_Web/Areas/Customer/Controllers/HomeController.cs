using AutoMapper;
using CarHub_Utility;
using CarHub_Web.Models;
using CarHub_Web.Models.Dto;
using CarHub_Web.Models.VM;
using CarHub_Web.Service;
using CarHub_Web.Service.IService;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;

namespace CarHub_Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICarService _carService;
        private readonly ICarImageService _carImageService;
        private readonly IBrandService _brandService;
        private readonly ICarTypeService _carTypeService;
        private readonly IMileageService _mileageService;
        private readonly ICarXFeatureService _carXFeatureService;
        private readonly ICarSpecificationService _carSpecificationService;
        private readonly IFeatureTypeService _featureTypeService;
        private readonly IFeatureXFeaturetypeService _featureXFeaturetypeService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IMapper _mapper;
        private readonly IReviewService _reviewService;
        private readonly IReviewXCommentService _reviewXCommentService;

        public List<CarDTO> list;


        public HomeController(ILogger<HomeController> logger, IFeatureTypeService featureTypeService,ICarXFeatureService carXFeatureService, ICarSpecificationService carSpecificationService, IMileageService mileageService, ICarService carService, ICarImageService carImageService, IWebHostEnvironment webHostEnvironment, IMapper mapper, IBrandService brandService, ICarTypeService carTypeService, IFeatureXFeaturetypeService featureXFeaturetypeService, IReviewService reviewService, IReviewXCommentService reviewXCommentService)
        {
            _logger = logger;
            _featureTypeService = featureTypeService;
            _carService = carService;
            _carXFeatureService = carXFeatureService;
            _carImageService = carImageService;
            _brandService = brandService;
            _carTypeService = carTypeService;
            _mileageService = mileageService;
            _carSpecificationService = carSpecificationService;
            _webHostEnvironment = webHostEnvironment;
            _mapper = mapper;
            _featureXFeaturetypeService = featureXFeaturetypeService;
            _reviewService = reviewService;
            _reviewXCommentService = reviewXCommentService;
        }
        [HttpPost]
        public async Task<IActionResult> Search(string searchText)
        {
            // Perform search and populate the view model
            var viewModel = await GetSearchResults(searchText);
            if(viewModel.CarList.Count== 0)
            {
                TempData["error"] = "Search Data DoesNot Exists.";
                return RedirectToAction("Index");

            }
            return View("Searchindex", viewModel);
        }

        private async Task<HomeVM> GetSearchResults(string searchText)
        {
            var viewModel = new HomeVM();
            if (!string.IsNullOrEmpty(searchText))
            {
                var carListResponse = await _carService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
                if (carListResponse != null && carListResponse.IsSuccess)
                {
                    var carDTOList = JsonConvert.DeserializeObject<List<CarDTO>>(Convert.ToString(carListResponse.Result));
                    viewModel.CarList = carDTOList.Where(x => x.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase)
                    || x.Brand.BrandName.Contains(searchText, StringComparison.OrdinalIgnoreCase)
                    ).ToList();

                }
                else
                {
                    TempData["error"] = "Failed to retrieve car list.";
                    
                }
            }
            return viewModel;
        }
        //Search Car by lazyloading
      
        [HttpPost]
        public async Task<IActionResult> CarSearchByLazyLoading(string searchText)
        {
            ViewBag.searchText = searchText; // or ViewData["SearchTerm"] = searchTerm;
            return RedirectToAction("GetSearchCarResults", new { searchText = searchText });
        }

        public async Task<IActionResult> GetSearchCarResults(int pageNum, string searchText)
        {
            //pageNum = pageNum ?? 0;
            //ViewBag.IsEndOfRecords = false;
            //if (Request.IsAjaxRequest())
            if (pageNum == null)
            {
                pageNum = 0;
            }

            ViewBag.IsEndOfRecords = false;
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                var response = await _carService.CarSearchByLazyLoading<APIResponse>(pageNum, searchText, HttpContext.Session.GetString(SD.SessionToken));
                if (response != null && response.IsSuccess)
                {
                    list = JsonConvert.DeserializeObject<List<CarDTO>>(Convert.ToString(response.Result));
                }

                ViewBag.IsEndOfRecords = (list.Any());
                ViewBag.list = list;
                ViewBag.searchText = searchText;
                return PartialView("_CarSearch", list);
            }
            else
            {

                var response = await _carService.CarSearchByLazyLoading<APIResponse>(pageNum, searchText, HttpContext.Session.GetString(SD.SessionToken));
                if (response != null && response.IsSuccess)
                {
                    list = JsonConvert.DeserializeObject<List<CarDTO>>(Convert.ToString(response.Result));
                }
                else
                {
                    TempData["error"] = "Failed to retrieve car list.";
                    return RedirectToAction("Index");
                }

                ViewBag.TotalNumberProjects =list.Count;
                ViewBag.list = list;
                ViewBag.searchText = searchText;

                return View("CarSearchByLazyLoading", list);
            }
        }


        public async Task<IActionResult> Index()
        {
            HomeVM homeVM = new();
            var carlist = await _carService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (carlist != null && carlist.IsSuccess)
            {
                homeVM.CarList = JsonConvert.DeserializeObject<List<CarDTO>>(Convert.ToString(carlist.Result));
            }
            var cartypelist = await _carTypeService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (cartypelist != null && cartypelist.IsSuccess)
            {
                homeVM.CarTypeList = JsonConvert.DeserializeObject<List<CarTypeDTO>>(Convert.ToString(cartypelist.Result));
            }
            var brandlist = await _brandService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (brandlist != null && brandlist.IsSuccess)
            {
                homeVM.BrandList = JsonConvert.DeserializeObject<List<BrandDTO>>(Convert.ToString(brandlist.Result));
            }
            var carimagelist = await _carImageService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (carimagelist != null && carimagelist.IsSuccess)
            {
                homeVM.CarImagelist = JsonConvert.DeserializeObject<List<CarImageDTO>>(Convert.ToString(brandlist.Result));
            }

            return View(homeVM);
        }

        

        public async Task<IActionResult> Details(int carId)
        {
            HomeVM homeVM = new();
            //var carlist = await _carService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            //if (carlist != null && carlist.IsSuccess)
            //{
            //    homeVM.CarList = JsonConvert.DeserializeObject<List<CarDTO>>(Convert.ToString(carlist.Result));
            //}
            var cartypelist = await _carTypeService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (cartypelist != null && cartypelist.IsSuccess)
            {
                homeVM.CarTypeList = JsonConvert.DeserializeObject<List<CarTypeDTO>>(Convert.ToString(cartypelist.Result));
            }
            var brandlist = await _brandService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (brandlist != null && brandlist.IsSuccess)
            {
                homeVM.BrandList = JsonConvert.DeserializeObject<List<BrandDTO>>(Convert.ToString(brandlist.Result));
            }
            var carimagelist = await _carImageService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (carimagelist != null && carimagelist.IsSuccess)
            {
                var carImagies = JsonConvert.DeserializeObject<List<CarImageDTO>>(Convert.ToString(carimagelist.Result));
                homeVM.CarImagelist = carImagies.Where(x => x.Car.Id == carId).ToList();

            }
            var mileagelist = await _mileageService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (mileagelist != null && mileagelist.IsSuccess)
            {
                var mileagel = JsonConvert.DeserializeObject<List<MileageDTO>>(Convert.ToString(mileagelist.Result));
                homeVM.MileageList = mileagel.Where(x => x.CarId == carId).ToList();
            }


            var response = await _carService.GetAsync<APIResponse>(carId, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                homeVM.Car = JsonConvert.DeserializeObject<CarDTO>(Convert.ToString(response.Result));
            }
            var carspeclist = await _carSpecificationService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (carspeclist != null && carspeclist.IsSuccess)
            {
                var carspecl = JsonConvert.DeserializeObject<List<CarSpecificationDTO>>(Convert.ToString(carspeclist.Result));
                homeVM.CarSpecificationList = carspecl.Where(x => x.CarId == carId).ToList();
            }
            var ReviewList = await _reviewService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (ReviewList != null && ReviewList.IsSuccess)
            {
                var reviewList = JsonConvert.DeserializeObject<List<ReviewDTO>>(Convert.ToString(ReviewList.Result));
                homeVM.ReviewList = reviewList.Where(x => x.CarId == carId).OrderByDescending(X => X.Id).ToList();
            }
            var ReviewXcommentList = await _reviewXCommentService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (ReviewXcommentList != null && ReviewXcommentList.IsSuccess)
            {
                var reviewList = JsonConvert.DeserializeObject<List<ReviewXCommentDTO>>(Convert.ToString(ReviewXcommentList.Result));
                homeVM.ReviewXCommentList = reviewList.OrderByDescending(p => p.Id).ToList(); ;

            }



            return View(homeVM);
        }

        public async Task<IActionResult> BrandIndex(int brandId)
        {
            HomeVM homeVM = new();
            var carlist = await _carService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (carlist != null && carlist.IsSuccess)
            {
                var carl = JsonConvert.DeserializeObject<List<CarDTO>>(Convert.ToString(carlist.Result));
                homeVM.CarList = carl.Where(x => x.Brand.Id == brandId).ToList();
            }

            var cartypelist = await _carTypeService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (cartypelist != null && cartypelist.IsSuccess)
            {
                homeVM.CarTypeList = JsonConvert.DeserializeObject<List<CarTypeDTO>>(Convert.ToString(cartypelist.Result));
            }
            var brandlist = await _brandService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (brandlist != null && brandlist.IsSuccess)
            {
                homeVM.BrandList = JsonConvert.DeserializeObject<List<BrandDTO>>(Convert.ToString(brandlist.Result));
            }
            var carimagelist = await _carImageService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (carimagelist != null && carimagelist.IsSuccess)
            {
                homeVM.CarImagelist = JsonConvert.DeserializeObject<List<CarImageDTO>>(Convert.ToString(brandlist.Result));
            }

            return View(homeVM);
        }
        public async Task<IActionResult> SpecificationIndex(int carId,int featureTypeId)
        {
            HomeVM homeVM = new();

            var featuretypelist = await _featureTypeService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (featuretypelist != null && featuretypelist.IsSuccess)
            {
                homeVM.FeatureTypeList = JsonConvert.DeserializeObject<List<FeatureTypeDTO>>(Convert.ToString(featuretypelist.Result));
            }
            var cartypelist = await _carTypeService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (cartypelist != null && cartypelist.IsSuccess)
            {
                homeVM.CarTypeList = JsonConvert.DeserializeObject<List<CarTypeDTO>>(Convert.ToString(cartypelist.Result));
            }
            var brandlist = await _brandService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (brandlist != null && brandlist.IsSuccess)
            {
                homeVM.BrandList = JsonConvert.DeserializeObject<List<BrandDTO>>(Convert.ToString(brandlist.Result));
            }
            var carimagelist = await _carImageService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (carimagelist != null && carimagelist.IsSuccess)
            {
                var carImagies = JsonConvert.DeserializeObject<List<CarImageDTO>>(Convert.ToString(carimagelist.Result));
                homeVM.CarImagelist = carImagies.Where(x => x.Car.Id == carId).ToList();

            }
            var mileagelist = await _mileageService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (mileagelist != null && mileagelist.IsSuccess)
            {
                var mileagel = JsonConvert.DeserializeObject<List<MileageDTO>>(Convert.ToString(mileagelist.Result));
                homeVM.MileageList = mileagel.Where(x => x.CarId == carId).ToList();
            }


            var response = await _carService.GetAsync<APIResponse>(carId, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                homeVM.Car = JsonConvert.DeserializeObject<CarDTO>(Convert.ToString(response.Result));

            }
            var carspeclist = await _carSpecificationService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (carspeclist != null && carspeclist.IsSuccess)
            {
                var carspecl = JsonConvert.DeserializeObject<List<CarSpecificationDTO>>(Convert.ToString(carspeclist.Result));
                homeVM.CarSpecificationList = carspecl.Where(x => x.CarId == carId).ToList();
            }
            var carXfeature = await _carXFeatureService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (carXfeature != null && carXfeature.IsSuccess)
            {
                var carf = JsonConvert.DeserializeObject<List<CarXFeatureDTO>>(Convert.ToString(carXfeature.Result));
                homeVM.CarXFeatureList = carf.Where(x => x.FeatureTypeId == featureTypeId &&
                 x.CarId == carId
                ).ToList();
            }
            var featureXFeatureType = await _featureXFeaturetypeService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (featureXFeatureType != null && featureXFeatureType.IsSuccess)
            {
                var carf = JsonConvert.DeserializeObject<List<FeatureXFeaturetypeDTO>>(Convert.ToString(featureXFeatureType.Result));
                //homeVM.FeatureXFeaturetypeList = carf.Where(x => x.FeatureTypeId == featureTypeId
                //).ToList();
                homeVM.FeatureXFeaturetypeList = carf;
            }

            return View(homeVM);
        }

        public async Task<IActionResult> AllImages(int carId)
        {
            HomeVM homeVM = new();
            var carimagelist = await _carImageService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (carimagelist != null && carimagelist.IsSuccess)
            {
                var carImagies = JsonConvert.DeserializeObject<List<CarImageDTO>>(Convert.ToString(carimagelist.Result));
                homeVM.CarImagelist = carImagies.Where(x => x.Car.Id == carId).ToList();

            }



            return View(homeVM);
        }



        public async Task<IActionResult> RatingIndex(int carId)
        {
            HomeVM homeVM = new();
          
            var cartypelist = await _carTypeService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (cartypelist != null && cartypelist.IsSuccess)
            {
                homeVM.CarTypeList = JsonConvert.DeserializeObject<List<CarTypeDTO>>(Convert.ToString(cartypelist.Result));
            }
            var brandlist = await _brandService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (brandlist != null && brandlist.IsSuccess)
            {
                homeVM.BrandList = JsonConvert.DeserializeObject<List<BrandDTO>>(Convert.ToString(brandlist.Result));
            }
            var carimagelist = await _carImageService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (carimagelist != null && carimagelist.IsSuccess)
            {
                var carImagies = JsonConvert.DeserializeObject<List<CarImageDTO>>(Convert.ToString(carimagelist.Result));
                homeVM.CarImagelist = carImagies.Where(x => x.Car.Id == carId).ToList();

            }
         

            var response = await _carService.GetAsync<APIResponse>(carId, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                homeVM.Car = JsonConvert.DeserializeObject<CarDTO>(Convert.ToString(response.Result));

            }
            
     
            return View(homeVM);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}