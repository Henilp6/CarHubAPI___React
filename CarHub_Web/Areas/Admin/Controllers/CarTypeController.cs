using AutoMapper;
using CarHub_Utility;
using CarHub_Web.Models;
using CarHub_Web.Models.Dto;
using CarHub_Web.Models.VM;
using CarHub_Web.Service;
using CarHub_Web.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CarHub_Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class CarTypeController : Controller
    {
        private readonly ICarTypeService _carTypeService;
        private readonly IMapper _mapper;
        public CarTypeController(ICarTypeService carTypeService, IMapper mapper)
        {
            _carTypeService = carTypeService;
            _mapper = mapper;
        }

        //public async Task<IActionResult> IndexCarType()
        //{
        //    List<CarTypeDTO> list = new();


        //    var response = await _carTypeService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
        //    if (response != null && response.IsSuccess)
        //    {
        //        list = JsonConvert.DeserializeObject<List<CarTypeDTO>>(Convert.ToString(response.Result));
        //    }
        //    return View(list);
        //}
        public async Task<IActionResult> IndexCarType(string term, string orderBy, int currentPage = 1)
        {
            List<CarTypeDTO> list = new();
            CarTypeIndexVM carTypeIndexVM = new();

            var response = await _carTypeService.AllDataAsync<APIResponse>(term, orderBy, currentPage, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
				carTypeIndexVM = JsonConvert.DeserializeObject<CarTypeIndexVM>(Convert.ToString(response.Result));
            }
            return View(carTypeIndexVM);
        }
        //[Authorize(Roles = "admin")]
        public async Task<IActionResult> CreateCarType()
        {
            return View();
        }
        //[Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCarType(CarTypeCreateDTO model)
        {
            if (ModelState.IsValid)
            {

                var response = await _carTypeService.CreateAsync<APIResponse>(model, HttpContext.Session.GetString(SD.SessionToken));
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "CarType created successfully";
					return RedirectToAction(nameof(IndexCarType));
				}
            }
            TempData["error"] = "Error encountered.";
            return View(model);
        }


        //[Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateCarType(int carTypeId)
        {
            var response = await _carTypeService.GetAsync<APIResponse>(carTypeId, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                CarTypeDTO model = JsonConvert.DeserializeObject<CarTypeDTO>(Convert.ToString(response.Result));
                return View(_mapper.Map<CarTypeUpdateDTO>(model));
            }
            return NotFound();
        }
        //[Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateCarType(CarTypeUpdateDTO model)
        {
            if (ModelState.IsValid)
            {
                TempData["success"] = "CarType updated successfully";
                var response = await _carTypeService.UpdateAsync<APIResponse>(model, HttpContext.Session.GetString(SD.SessionToken));
                if (response != null && response.IsSuccess)
                {
					return RedirectToAction(nameof(IndexCarType));
				}
            }
            TempData["error"] = "Error encountered.";
            return View(model);
        }

		//[Authorize(Roles = "admin")]
		//public async Task<IActionResult> DeleteCarType(int carTypeId)
		//{
		//    var response = await _carTypeService.GetAsync<APIResponse>(carTypeId, HttpContext.Session.GetString(SD.SessionToken));
		//    if (response != null && response.IsSuccess)
		//    {
		//        CarTypeDTO model = JsonConvert.DeserializeObject<CarTypeDTO>(Convert.ToString(response.Result));
		//        return View(model);
		//    }
		//    return NotFound();
		//}
		////[Authorize(Roles = "admin")]
		//[HttpPost]
		//[ValidateAntiForgeryToken]
		
		public async Task<IActionResult> DeleteCarType(int id)
        {

            var response = await _carTypeService.DeleteAsync<APIResponse>(id, HttpContext.Session.GetString(SD.SessionToken));
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
