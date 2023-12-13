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
using System.Data;


namespace CarHub_Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class CarXColorController : Controller
	{
		private readonly ICarService _carService;
		private readonly IColorService _colorService;
		private readonly ICarXColorService _carXColorService;
		private readonly IMapper _mapper;
		public CarXColorController(ICarXColorService carXColorService, IMapper mapper, IColorService colorService, ICarService carService)
		{
			_carService = carService;
			_colorService = colorService;
			_mapper = mapper;
			_carXColorService = carXColorService;


		}


		public async Task<IActionResult> IndexCarXColor()
		{
			List<CarXColorDTO> list = new();

			var response = await _carXColorService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
			if (response != null && response.IsSuccess)
			{
				list = JsonConvert.DeserializeObject<List<CarXColorDTO>>(Convert.ToString(response.Result));
			}
			return View(list);
		}


		//  [Authorize(Roles = "admin")]
		public async Task<IActionResult> CreateCarXColor(int carId)
		{
			CarXColorCreateVM carXColorVM = new();
			{
				var response = await _carService.GetAsync<APIResponse>(carId, HttpContext.Session.GetString(SD.SessionToken));
				if (response != null &&
					response.IsSuccess)
				{

					CarDTO model = JsonConvert.DeserializeObject<CarDTO>(Convert.ToString(response.Result));
					carXColorVM.Car = _mapper.Map<CarCreateDTO>(model);
				}
				var colorlist = await _carXColorService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
				if (colorlist != null && colorlist.IsSuccess)
				{

					List<CarXColorDTO> model1 = JsonConvert.DeserializeObject<List<CarXColorDTO>>(Convert.ToString(colorlist.Result));
                    var categories = (from data in model1
                              where data.CarId == carId


                              select new CarXColorDTO
                              {
                                  Id = data.Id,
                                  ColorId = data.ColorId,
                                  CarId = data.CarId

                              });
                    carXColorVM.CarXColorlist = _mapper.Map<List<CarXColorCreateDTO>>(categories);
				}

				var colorxList = await _colorService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
				if (colorxList != null && colorxList.IsSuccess)
				{
					carXColorVM.Colorlist = JsonConvert.DeserializeObject<List<ColorVM>>
					  (Convert.ToString(colorxList.Result)).Select(i => new ColorVM
					  {
						  ColorName = i.ColorName,
						  Id = i.Id,
						  IsChecked = carXColorVM.CarXColorlist.Where(x => x.ColorId == i.Id && x.CarId == carId).Any()

					  }).ToList();
				}
			};
			return View(carXColorVM);
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		
		public async Task<IActionResult> CreateCarXColor(CarXColorCreateVM carXColorVM)
		{
			if (ModelState.IsValid)
			{
				TempData["success"] = "CarXColor Added successfully";
				var response = await _carXColorService.CreateAsync<APIResponse>(carXColorVM, HttpContext.Session.GetString(SD.SessionToken));
				if (response != null && response.IsSuccess)
				{
                    //	return RedirectToAction(nameof(IndexCarXColor));
                    return RedirectToAction("IndexCar", "Car", new { area = "Admin" });
                }
				else
				{
					if (response.ErrorMessages.Count > 0)
					{
						ModelState.AddModelError("ErrorMessages", response.ErrorMessages.FirstOrDefault());
					}
				}
			}

			var colorlist = await _carXColorService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
			if (colorlist != null && colorlist.IsSuccess)
			{

				List<CarXColorDTO> model1 = JsonConvert.DeserializeObject<List<CarXColorDTO>>(Convert.ToString(colorlist.Result));
				carXColorVM.CarXColorlist = _mapper.Map<List<CarXColorCreateDTO>>(model1);
			}

			var colorxList = await _colorService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
			if (colorxList != null && colorxList.IsSuccess)
			{
				carXColorVM.Colorlist = JsonConvert.DeserializeObject<List<ColorVM>>
				  (Convert.ToString(colorxList.Result)).Select(i => new ColorVM
				  {
					  ColorName = i.ColorName,
					  Id = i.Id,
					  IsChecked = carXColorVM.CarXColorlist.Where(x => x.ColorId == i.Id).Any()

				  }).ToList();
			}

			return View(carXColorVM);
		}

		//// [Authorize(Roles = "admin")]
		//[HttpPost]
		//[ValidateAntiForgeryToken]
		//public async Task<IActionResult> UpdateCarXColor(CarXColorUpdateVM model)
		//{
		//	if (ModelState.IsValid)
		//	{
		//		TempData["success"] = "CarXColor updated successfully";
		//		var response = await _carXColorService.UpdateAsync<APIResponse>(model, HttpContext.Session.GetString(SD.SessionToken));
		//		if (response != null && response.IsSuccess)
		//		{
		//			return RedirectToAction(nameof(IndexCarXColor));
		//		}
		//		else
		//		{
		//			if (response.ErrorMessages.Count > 0)
		//			{
		//				ModelState.AddModelError("ErrorMessages", response.ErrorMessages.FirstOrDefault());
		//			}
		//		}
		//	}

		//	var resp = await _carService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));

		//	if (resp != null && resp.IsSuccess)
		//	{
		//		model.CarList = JsonConvert.DeserializeObject<List<CarDTO>>
		//			(Convert.ToString(resp.Result)).Select(i => new SelectListItem
		//			{
		//				Text = i.Name,
		//				Value = i.Id.ToString()
		//			});

		//	}
		//	var colorList = await _colorService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));

		//	if (colorList != null && colorList.IsSuccess)
		//	{
		//		model.Colorlist = JsonConvert.DeserializeObject<List<ColorVM>>
		//				  (Convert.ToString(colorList.Result)).Select(i => new ColorVM
		//				  {
		//					  ColorName = i.ColorName,
		//					  Id = i.Id,
		//					  IsChecked = true

		//				  }).ToList();

		//	}

		//	return View(model);
		//}



		// [Authorize(Roles = "admin")]

		public async Task<IActionResult> DeleteCarXColor(int carXColorId)
		{

			var response = await _carXColorService.DeleteAsync<APIResponse>(carXColorId, HttpContext.Session.GetString(SD.SessionToken));
			if (response != null && response.IsSuccess)
			{
				return Json(new { success = true, message = "Delete Successful" });
			}
			else
			{
				TempData["error"] = "Error encountered.";
				return Json(new { success = false, message = "Error while deleting" });
			}
		}

	}

}