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

namespace CarHub_Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class CarController : Controller
    {
        private readonly ICarService _carService;
      
        private readonly IBrandService _brandService;
        private readonly ICarTypeService _carTypeService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IMapper _mapper;
        public CarController(ICarService carService,  IWebHostEnvironment webHostEnvironment, IMapper mapper, IBrandService brandService, ICarTypeService carTypeService)
        {
            _carService = carService;
           
            _mapper = mapper;
            _brandService = brandService;
            _carTypeService = carTypeService;
            _webHostEnvironment = webHostEnvironment;
        }

        //public async Task<IActionResult> IndexCar()
        //{
        //    List<CarDTO> list = new();

        //    var response = await _carService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
        //    if (response != null && response.IsSuccess)
        //    {
        //        list = JsonConvert.DeserializeObject<List<CarDTO>>(Convert.ToString(response.Result));
        //    }
        //    return View(list);
        //}
		public async Task<IActionResult> IndexCar(string term, string orderBy, int currentPage = 1)
		{
			List<CarDTO> list = new();
			CarIndexVM carIndexVM = new();

			var response = await _carService.AllDataAsync<APIResponse>(term, orderBy, currentPage, HttpContext.Session.GetString(SD.SessionToken));
			if (response != null && response.IsSuccess)
			{
				carIndexVM = JsonConvert.DeserializeObject<CarIndexVM>(Convert.ToString(response.Result));
			}
			return View(carIndexVM);
		}

		//  [Authorize(Roles = "admin")]
		public async Task<IActionResult> CreateCar()
        {
            CarCreateVM carVM = new();
            {
                var response = await _brandService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
                if (response != null && response.IsSuccess)
                {
                    carVM.BrandList = JsonConvert.DeserializeObject<List<BrandDTO>>
                        (Convert.ToString(response.Result)).Select(i => new SelectListItem
                        {
                            Text = i.BrandName,
                            Value = i.Id.ToString()
                        });
                }
                var response1 = await _carTypeService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
                if (response1 != null && response1.IsSuccess)
                {
                    carVM.CarTypeList = JsonConvert.DeserializeObject<List<CarTypeDTO>>
                        (Convert.ToString(response1.Result)).Select(i => new SelectListItem
                        {
                            Text = i.TypeName,
                            Value = i.Id.ToString()
                        });
                }

            }
            return View(carVM);
        }
        //[Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCar(CarCreateVM model)
        {

            if (ModelState.IsValid)
            {


                    
                  
                    var response = await _carService.CreateAsync<APIResponse>(model.Car, HttpContext.Session.GetString(SD.SessionToken));
                    if (response != null && response.IsSuccess)
                    {
                    TempData["success"] = "Car created successfully";
                    return RedirectToAction(nameof(IndexCar));
                    }
                    else
                    {
                        if (response.ErrorMessages.Count > 0)
                        {
                            ModelState.AddModelError("ErrorMessages", response.ErrorMessages.FirstOrDefault());
                        }
                    }
                

                var resp = await _brandService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
                var resp1 = await _carTypeService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
                if (resp != null && resp.IsSuccess)
                {
                    model.BrandList = JsonConvert.DeserializeObject<List<BrandDTO>>
                        (Convert.ToString(resp.Result)).Select(i => new SelectListItem
                        {
                            Text = i.BrandName,
                            Value = i.Id.ToString()
                        });
                }
                if (resp1 != null && resp1.IsSuccess)
                {
                    model.CarTypeList = JsonConvert.DeserializeObject<List<CarTypeDTO>>
                        (Convert.ToString(resp1.Result)).Select(i => new SelectListItem
                        {
                            Text = i.TypeName,
                            Value = i.Id.ToString()
                        });
                }

                return View(model);
            }
            return View(model);
        }

        // [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateCar(int carid)
        {
            CarUpdateVM carVM = new();
            var response = await _carService.GetAsync<APIResponse>(carid, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                CarDTO model = JsonConvert.DeserializeObject<CarDTO>(Convert.ToString(response.Result));
                carVM.Car = _mapper.Map<CarUpdateDTO>(model);
            }

            var response1 = await _brandService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (response1 != null && response1.IsSuccess)
            {
                carVM.BrandList = JsonConvert.DeserializeObject<List<BrandDTO>>
                    (Convert.ToString(response1.Result)).Select(i => new SelectListItem
                    {
                        Text = i.BrandName,
                        Value = i.Id.ToString()
                    });
            }
           var response2 = await _carTypeService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (response2 != null && response2.IsSuccess)
            {
                
                carVM.CarTypeList = JsonConvert.DeserializeObject<List<CarTypeDTO>>
                    (Convert.ToString(response2.Result)).Select(i => new SelectListItem
                    {
                        Text = i.TypeName,
                        Value = i.Id.ToString()
                    });
                return View(carVM);
            }


            return NotFound();
        }

        // [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateCar(CarUpdateVM model)
        {
            if (ModelState.IsValid)
            {
              
                var response = await _carService.UpdateAsync<APIResponse>(model.Car, HttpContext.Session.GetString(SD.SessionToken));
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Car updated successfully";
                    return RedirectToAction(nameof(IndexCar));
                }
                else
                {
                    if (response.ErrorMessages.Count > 0)
                    {
                        ModelState.AddModelError("ErrorMessages", response.ErrorMessages.FirstOrDefault());
                    }
                }
            }

            var resp = await _brandService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            var resp1 = await _carTypeService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (resp != null && resp.IsSuccess)
            {
                model.BrandList = JsonConvert.DeserializeObject<List<BrandDTO>>
                    (Convert.ToString(resp.Result)).Select(i => new SelectListItem
                    {
                        Text = i.BrandName,
                        Value = i.Id.ToString()
                    });
            }
            if (resp1 != null && resp1.IsSuccess)
            {
                model.CarTypeList = JsonConvert.DeserializeObject<List<CarTypeDTO>>
                    (Convert.ToString(resp.Result)).Select(i => new SelectListItem
                    {
                        Text = i.TypeName,
                        Value = i.Id.ToString()
                    });
            }
            return View(model);
        }

        //[Authorize(Roles = "admin")]
        //public async Task<IActionResult> DeleteCar(int carId)
        //{
        //    CarDeleteVM carVM = new();
        //    var response = await _carService.GetAsync<APIResponse>(carId, HttpContext.Session.GetString(SD.SessionToken));
        //    if (response != null && response.IsSuccess)
        //    {
        //        CarDTO model = JsonConvert.DeserializeObject<CarDTO>(Convert.ToString(response.Result));
        //        carVM.Car = model;
        //    }

        //    response = await _brandService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
        //    response = await _carTypeService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
        //    if (response != null && response.IsSuccess)
        //    {
        //        carVM.BrandList = JsonConvert.DeserializeObject<List<BrandDTO>>
        //            (Convert.ToString(response.Result)).Select(i => new SelectListItem
        //            {
        //                Text = i.BrandName,
        //                Value = i.Id.ToString()
        //            });
        //        carVM.CarTypeList = JsonConvert.DeserializeObject<List<CarTypeDTO>>
        //           (Convert.ToString(response.Result)).Select(i => new SelectListItem
        //           {
        //               Text = i.TypeName,
        //               Value = i.Id.ToString()
        //           });
        //        return View(carVM);
        //    }


        //    return NotFound();
        //}

        // [Authorize(Roles = "admin")]
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCar(int carId)
        {

            var response = await _carService.DeleteAsync<APIResponse>(carId, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Car deleted successfully";
                return RedirectToAction(nameof(IndexCar));
            }
            TempData["error"] = "Error encountered.";
			return RedirectToAction(nameof(IndexCar));
		}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        
        //public async Task<IActionResult> DeleteImage(int imageId)
        //{
            
         

        //    var imageToBeDeleted = await _carImageService.GetAsync<APIResponse>(imageId, HttpContext.Session.GetString(SD.SessionToken));
        //    if (imageToBeDeleted != null && imageToBeDeleted.IsSuccess)
        //    {
        //        CarImageDTO model = JsonConvert.DeserializeObject<CarImageDTO>(Convert.ToString(imageToBeDeleted.Result));

        //    }
        //    int id = ;
        //    if (imageToBeDeleted != null)
        //    {
        //        if (!string.IsNullOrEmpty(imageToBeDeleted.ImageUrl))
        //        {
        //            var oldImagePath =
        //                           Path.Combine(_webHostEnvironment.WebRootPath,
        //                           imageToBeDeleted.ImageUrl.TrimStart('\\'));

        //            if (System.IO.File.Exists(oldImagePath))
        //            {
        //                System.IO.File.Delete(oldImagePath);
        //            }
        //        }

        //        _unitOfWork.ProductImage.Remove(imageToBeDeleted);
        //        _unitOfWork.Save();

        //        TempData["success"] = "Deleted successfully";
        //    }

        //    return RedirectToAction(nameof(Upsert), new { id = productId });
        //}


    }
}
