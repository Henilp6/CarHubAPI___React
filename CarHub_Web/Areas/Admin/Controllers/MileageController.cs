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
    public class MileageController : Controller
    {
        private readonly ICarService _carService;
        private readonly IMileageService _mileageService;
        private readonly IMapper _mapper;
        public MileageController(IMileageService mileageService, IMapper mapper, ICarService carService)
        {
            _carService = carService;
            _mapper = mapper;
            _mileageService = mileageService;


        }
        public async Task<IActionResult> MileageByPagination(string term = "", string orderBy = "", int currentPage = 1)
        {
            ViewData["CurrentFilter"] = term;
			//term = string.IsNullOrEmpty(term) ? "" : term.ToLower();

			MileageIndexVM mileageIndexVM = new MileageIndexVM();
            var response = await _mileageService.MileageByPagination<APIResponse>(term, orderBy, currentPage, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
				mileageIndexVM = JsonConvert.DeserializeObject<MileageIndexVM>(Convert.ToString(response.Result));
            }
            return View(mileageIndexVM);
        }

        public async Task<IActionResult> IndexMileage()
        {
            List<MileageDTO> list = new();

            var response = await _mileageService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<MileageDTO>>(Convert.ToString(response.Result));
            }
            return View(list);
        }

        //  [Authorize(Roles = "admin")]
        public async Task<IActionResult> CreateMileage(int carId)
        {
            MileageCreateVM mileageVM = new();
            {
				var response = await _carService.GetAsync<APIResponse>(carId, HttpContext.Session.GetString(SD.SessionToken));
				if (response != null &&
					response.IsSuccess)
				{

					CarDTO model = JsonConvert.DeserializeObject<CarDTO>(Convert.ToString(response.Result));
					mileageVM.Car = _mapper.Map<CarCreateDTO>(model);
				}
                var response1 = await _mileageService.GetAsync<APIResponse>(carId, HttpContext.Session.GetString(SD.SessionToken));
                if (response1 != null &&
                    response1.IsSuccess)
                {

                    MileageDTO model = JsonConvert.DeserializeObject<MileageDTO>(Convert.ToString(response1.Result));
                    mileageVM.Mileage = _mapper.Map<MileageCreateDTO>(model);
                }


            }
            return View(mileageVM);
        }
        //[Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateMileage(MileageCreateVM model)
        {

            if (ModelState.IsValid)
            {

                TempData["success"] = "Mileage created successfully";
                var response = await _mileageService.CreateAsync<APIResponse>(model, HttpContext.Session.GetString(SD.SessionToken));
                if (response != null && response.IsSuccess)
                {
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

            var resp = await _carService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));

            if (resp != null && resp.IsSuccess)
            {
                model.CarList = JsonConvert.DeserializeObject<List<CarDTO>>
                    (Convert.ToString(resp.Result)).Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    });
            }


            return View(model);
        }

        public async Task<IActionResult> UpdateMileage(int mileageId)
        {
            MileageUpdateVM mileageVM = new();
            var response = await _mileageService.GetAsync<APIResponse>(mileageId, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                MileageDTO model = JsonConvert.DeserializeObject<MileageDTO>(Convert.ToString(response.Result));
                mileageVM.Mileage = _mapper.Map<MileageUpdateDTO>(model);
            }

            
            response = await _carService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {

                mileageVM.CarList = JsonConvert.DeserializeObject<List<CarDTO>>
                    (Convert.ToString(response.Result)).Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    });
                return View(mileageVM);
            }


            return NotFound();
        }

        // [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateMileage(MileageUpdateVM model)
        {
            if (ModelState.IsValid)
            {
                TempData["success"] = "Mileage updated successfully";
                var response = await _mileageService.UpdateAsync<APIResponse>(model, HttpContext.Session.GetString(SD.SessionToken));
                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(IndexMileage));
                }
                else
                {
                    if (response.ErrorMessages.Count > 0)
                    {
                        ModelState.AddModelError("ErrorMessages", response.ErrorMessages.FirstOrDefault());
                    }
                }
            }

            var resp = await _carService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
          
            if (resp != null && resp.IsSuccess)
            {
                model.CarList = JsonConvert.DeserializeObject<List<CarDTO>>
                    (Convert.ToString(resp.Result)).Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    });
            }
            
            return View(model);
        }

        //[Authorize(Roles = "admin")]
        //public async Task<IActionResult> DeleteMileage(int mileageId)
        //{
        //    MileageDeleteVM mileageVM = new();
        //    var response = await _mileageService.GetAsync<APIResponse>(mileageId, HttpContext.Session.GetString(SD.SessionToken));
        //    if (response != null && response.IsSuccess)
        //    {
        //        MileageDTO model = JsonConvert.DeserializeObject<MileageDTO>(Convert.ToString(response.Result));
        //        mileageVM.Mileage = model;
        //    }

        //    response = await _carService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
           
        //    if (response != null && response.IsSuccess)
        //    {
        //        mileageVM.CarList = JsonConvert.DeserializeObject<List<CarDTO>>
        //            (Convert.ToString(response.Result)).Select(i => new SelectListItem
        //            {
        //                Text = i.Name,
        //                Value = i.Id.ToString()
        //            });
              
        //        return View(mileageVM);
        //    }


        //    return NotFound();
        //}

        //// [Authorize(Roles = "admin")]
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteMileage(int id)
        {

            var response = await _mileageService.DeleteAsync<APIResponse>(id, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Mileage deleted successfully";
                return RedirectToAction(nameof(IndexMileage));
            }
            TempData["error"] = "Error encountered.";
			return RedirectToAction(nameof(IndexMileage));
		}

    }

}