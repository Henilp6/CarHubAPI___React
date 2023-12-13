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
using System.ComponentModel.Design;
using System.Data;
using System.Diagnostics.Eventing.Reader;

namespace CarHub_Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class CarImageController : Controller
	{
		private readonly ICarService _carService;
		private readonly ICarImageService _carImageService;
		private readonly IWebHostEnvironment _webHostEnvironment;
		private readonly IMapper _mapper;
		public CarImageController(ICarImageService carImageService, IWebHostEnvironment webHostEnvironment, IMapper mapper, IColorService colorService, ICarService carService)
		{
			_carService = carService;
			_carImageService = carImageService;
			_mapper = mapper;
            _webHostEnvironment = webHostEnvironment;


		}


	


		//  [Authorize(Roles = "admin")]
		public async Task<IActionResult> CreateCarImage(int carId)
		{
			CarImagesCreateVM carImagesCreateVM = new();
			{
				var response = await _carService.GetAsync<APIResponse>(carId, HttpContext.Session.GetString(SD.SessionToken));
				if (response != null &&
					response.IsSuccess)
				{

					CarDTO model = JsonConvert.DeserializeObject<CarDTO>(Convert.ToString(response.Result));
                    carImagesCreateVM.Car = _mapper.Map<CarCreateDTO>(model);
				}

                var CarImagelist = await _carImageService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
                if (CarImagelist != null && CarImagelist.IsSuccess)
                {
                    List<CarImageDTO> model1 = JsonConvert.DeserializeObject<List<CarImageDTO>>(Convert.ToString(CarImagelist.Result));
                    var imageList = _mapper.Map<List<CarImageCreateDTO>>(model1);
                    carImagesCreateVM.CarImagelist = imageList.Where(x => x.CarId == carId).ToList();
                }

            }
			return View(carImagesCreateVM);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		
		public async Task<IActionResult> CreateCarImage(CarImagesCreateVM carImagesCreateVM, List<IFormFile> files)
        {



			if (ModelState.IsValid)
			{
               
				if (files != null)
				{
                    string wwwRootPath = _webHostEnvironment.WebRootPath;

                    foreach (IFormFile file in files)
					{
						string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
						string carPath = @"images\cars\car-" + carImagesCreateVM.Car.Id;
						string finalPath = Path.Combine(wwwRootPath, carPath);

						if (!Directory.Exists(finalPath))
							Directory.CreateDirectory(finalPath);

						using (var fileStream = new FileStream(Path.Combine(finalPath, fileName), FileMode.Create))
						{
							file.CopyTo(fileStream);
						}

						CarImageCreateDTO carImageDTO = new()
						{
							ImageUrl = @"\" + carPath + @"\" + fileName,
							CarId = carImagesCreateVM.Car.Id,
						};

						if (carImagesCreateVM.CarImagelist== null)
                            carImagesCreateVM.CarImagelist = new List<CarImageCreateDTO>();

                        carImagesCreateVM.CarImagelist.Add(carImageDTO);

					}
				}








               
				var response = await _carImageService.CreateAsync<APIResponse>(carImagesCreateVM, HttpContext.Session.GetString(SD.SessionToken));
				if (response != null && response.IsSuccess)
				{
                    TempData["success"] = "carImagesCreate Added successfully";
                    //	return RedirectToAction(nameof(IndexCarXColor));
                    return RedirectToAction("IndexCar", "Car");
                }
				else
				{
					if (response.ErrorMessages.Count > 0)
					{
						ModelState.AddModelError("ErrorMessages", response.ErrorMessages.FirstOrDefault());
					}
				}
			}


			

			return View(carImagesCreateVM);
		}

        public async Task<IActionResult> DeleteCarImage(int imageId, int carId)
        {
            CarImageDTO carImageDTO = new();

            var imageToBeDeleted = await _carImageService.GetAsync<APIResponse>(imageId, HttpContext.Session.GetString(SD.SessionToken));
            if (imageToBeDeleted != null && imageToBeDeleted.IsSuccess)
            {
                carImageDTO = JsonConvert.DeserializeObject<CarImageDTO>(Convert.ToString(imageToBeDeleted.Result));
            }

            if (carImageDTO != null)
            {
                if (!string.IsNullOrEmpty(carImageDTO.ImageUrl))
                {
                    var oldImagePath =
                                   Path.Combine(_webHostEnvironment.WebRootPath,
                                   carImageDTO.ImageUrl.TrimStart('\\'));

                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }
                var response = await _carImageService.DeleteAsync<APIResponse>(carImageDTO.Id, HttpContext.Session.GetString(SD.SessionToken));
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Image Delated sucessfully.";
                    // return RedirectToAction(nameof(IndexCompanyXAmenity));
                    return RedirectToAction("IndexCar", "Car");
                }
            }

            return View();
        }




        //public async Task<IActionResult> DeleteImage(int id)
        //{
        //    CarImageDTO carImageDTO = new();

        //    var imageToBeDeleted = await _carImageService.GetAsync<APIResponse>(id, HttpContext.Session.GetString(SD.SessionToken));
        //    if (imageToBeDeleted != null && imageToBeDeleted.IsSuccess)
        //    {
        //        carImageDTO = JsonConvert.DeserializeObject<CarImageDTO>(Convert.ToString(imageToBeDeleted.Result));
        //    }

        //    if (carImageDTO != null)
        //    {
        //        if (!string.IsNullOrEmpty(carImageDTO.ImageUrl))
        //        {
        //            var oldImagePath =
        //                           Path.Combine(_webHostEnvironment.WebRootPath,
        //                           carImageDTO.ImageUrl.TrimStart('\\'));

        //            if (System.IO.File.Exists(oldImagePath))
        //            {
        //                System.IO.File.Delete(oldImagePath);
        //            }
        //        }
        //        var response = await _carImageService.DeleteAsync<APIResponse>(carImageDTO.Id, HttpContext.Session.GetString(SD.SessionToken));
        //        if (response != null && response.IsSuccess)
        //        {
        //            TempData["success"] = "Data Delated sucessfully.";
        //            // return RedirectToAction(nameof(IndexCompanyXAmenity));
        //            return RedirectToAction("IndexCar", "Car");
        //        }
        //    }
        //    return View();
        //}
        public async Task<IActionResult> SetCarImage(int imageId, int carId)
        {
            if (ModelState.IsValid)
            {
                var response = await _carImageService.SetAsync<APIResponse>(imageId, carId, HttpContext.Session.GetString(SD.SessionToken));
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Set default image sucessfully.";
                    return RedirectToAction("IndexCar", "Car");

                }
            }
            TempData["error"] = "Error encountered.";
            return RedirectToAction("CreateCarImage", "CarImage", new { carId = carId });
        }


       


    }

}