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

namespace CarHub_Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class CarSpecificationController : Controller
    {
       
        private readonly ICarService _carService;
        private readonly ICarSpecificationService _carSpecificationService;

        private readonly IMapper _mapper;
        public CarSpecificationController(ICarSpecificationService carSpecificationService, IMapper mapper, ICarService carService)
        {
            
            _mapper = mapper;
            _carService = carService;
            _carSpecificationService = carSpecificationService;

        }
		public async Task<IActionResult> IndexCarSpecification(string term, string orderBy, int currentPage = 1)
		{
			List<CarSpecificationDTO> list = new();
			CarSpecificationIndexVM carSpecificationIndexVM = new();

			var response = await _carSpecificationService.AllDataAsync<APIResponse>(term, orderBy, currentPage, HttpContext.Session.GetString(SD.SessionToken));
			if (response != null && response.IsSuccess)
			{
				carSpecificationIndexVM = JsonConvert.DeserializeObject<CarSpecificationIndexVM>(Convert.ToString(response.Result));
			}
			return View(carSpecificationIndexVM);
		}
		//public async Task<IActionResult> IndexCarSpecification()
  //      {
  //          List<CarSpecificationDTO> list = new();

  //          var response = await _carSpecificationService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
  //          if (response != null && response.IsSuccess)
  //          {
  //              list = JsonConvert.DeserializeObject<List<CarSpecificationDTO>>(Convert.ToString(response.Result));
  //          }
  //          return View(list);
  //      }
        //[Authorize(Roles = "admin")]
        public async Task<IActionResult> CreateCarSpecification(int carId)
        {
            CarSpecificationCreateVM carSpecificationVM = new();
            {
                var response = await _carService.GetAsync<APIResponse>(carId, HttpContext.Session.GetString(SD.SessionToken));
                if (response != null &&
                    response.IsSuccess)
                {

                    CarDTO model = JsonConvert.DeserializeObject<CarDTO>(Convert.ToString(response.Result));
                    carSpecificationVM.Car = _mapper.Map<CarCreateDTO>(model);
                }
                var response1 = await _carSpecificationService.GetAsync<APIResponse>(carId, HttpContext.Session.GetString(SD.SessionToken));
                if (response1 != null &&
                    response1.IsSuccess)
                {

                    CarSpecificationDTO model = JsonConvert.DeserializeObject<CarSpecificationDTO>(Convert.ToString(response1.Result));
                    carSpecificationVM.CarSpecification = _mapper.Map<CarSpecificationCreateDTO>(model);
                }


            }
            return View(carSpecificationVM);
        }
        //[Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCarSpecification(CarSpecificationCreateVM model)
        {
            if (ModelState.IsValid)
            {

                var response = await _carSpecificationService.CreateAsync<APIResponse>(model, HttpContext.Session.GetString(SD.SessionToken));
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "CarSpecification Created sucessfully.";
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

            
            return View(model);
        }



        //[Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateCarSpecification(int id)
        {
            CarSpecificationUpdateVM carSpecificationVM = new();
            var response = await _carSpecificationService.GetAsync<APIResponse>(id, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                CarSpecificationDTO model = JsonConvert.DeserializeObject<CarSpecificationDTO>(Convert.ToString(response.Result));
                carSpecificationVM.CarSpecification = _mapper.Map<CarSpecificationUpdateDTO>(model);
            }
            var response1 = await _carService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (response1 != null && response1.IsSuccess)
            {
                carSpecificationVM.CarList = JsonConvert.DeserializeObject<List<CarDTO>>
                    (Convert.ToString(response1.Result)).Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    });
                return View(carSpecificationVM);
            }
            
            return NotFound();
        }


        //[Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateCarSpecification(CarSpecificationUpdateVM model)
        {
            if (ModelState.IsValid)
            {

                var response = await _carSpecificationService.UpdateAsync<APIResponse>(model.CarSpecification, HttpContext.Session.GetString(SD.SessionToken));
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "CarSpecification Updated sucessfully.";
                    return RedirectToAction(nameof(IndexCarSpecification));
                }
                else
                {
                    if (response.ErrorMessages.Count > 0)
                    {
                        ModelState.AddModelError("ErrorMessages", response.ErrorMessages.FirstOrDefault());
                    }
                }
            }

            var response1 = await _carService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (response1 != null && response1.IsSuccess)
            {
                model.CarList = JsonConvert.DeserializeObject<List<CarDTO>>
                    (Convert.ToString(response1.Result)).Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    });
            }
            
            return View(model);
        }
        //[Authorize(Roles = "admin")]
        //public async Task<IActionResult> DeleteCarSpecification(int id)
        //{
        //    CarSpecificationDeleteVM carSpecificationVM = new();
        //    var response = await _carSpecificationService.GetAsync<APIResponse>(id, HttpContext.Session.GetString(SD.SessionToken));
        //    if (response != null && response.IsSuccess)
        //    {
        //        CarSpecificationDTO model = JsonConvert.DeserializeObject<CarSpecificationDTO>(Convert.ToString(response.Result));
        //        carSpecificationVM.CarSpecification = model;
        //    }

        //    var response1 = await _carService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
        //    if (response1 != null && response1.IsSuccess)
        //    {
        //        carSpecificationVM.CarList = JsonConvert.DeserializeObject<List<CarDTO>>
        //            (Convert.ToString(response1.Result)).Select(i => new SelectListItem
        //            {
        //                Text = i.Name,
        //                Value = i.Id.ToString()
        //            });
        //        return View(carSpecificationVM);
        //    }
           
               
           


        //    return NotFound();
        //}
        ////[Authorize(Roles = "admin")]
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCarSpecification(int id)
        {

            var response = await _carSpecificationService.DeleteAsync<APIResponse>(id, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "CarSpecification Delated sucessfully.";
                return RedirectToAction(nameof(IndexCarSpecification));
            }

			return RedirectToAction(nameof(IndexCarSpecification));
		}



    }
}
