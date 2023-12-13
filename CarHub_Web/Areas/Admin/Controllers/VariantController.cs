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
    public class VariantController : Controller
    {
       
        private readonly ICarService _carService;
        private readonly IVariantService _variantService;

        private readonly IMapper _mapper;
        public VariantController(IVariantService variantService, IMapper mapper, ICarService carService)
        {
            
            _mapper = mapper;
            _carService = carService;
            _variantService = variantService;

        }
		public async Task<IActionResult> VariantByPagination(string term = "", string orderBy = "", int currentPage = 1)
		{
			ViewData["CurrentFilter"] = term;
			//term = string.IsNullOrEmpty(term) ? "" : term.ToLower();

			VariantIndexVM variantIndexVM = new VariantIndexVM();
			var response = await _variantService.VariantByPagination<APIResponse>(term, orderBy, currentPage, HttpContext.Session.GetString(SD.SessionToken));
			if (response != null && response.IsSuccess)
			{
				variantIndexVM = JsonConvert.DeserializeObject<VariantIndexVM>(Convert.ToString(response.Result));
			}
			return View(variantIndexVM);
		}
		public async Task<IActionResult> IndexVariant()
        {
            List<VariantDTO> list = new();

            var response = await _variantService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<VariantDTO>>(Convert.ToString(response.Result));
            }
            return View(list);
        }
		//[Authorize(Roles = "admin")]
		public async Task<IActionResult> CreateVariant(int carId)
		{
			VariantCreateVM variantVM = new();
			{
				var response = await _carService.GetAsync<APIResponse>(carId, HttpContext.Session.GetString(SD.SessionToken));
				if (response != null &&
					response.IsSuccess)
				{

					CarDTO model = JsonConvert.DeserializeObject<CarDTO>(Convert.ToString(response.Result));
					variantVM.Car = _mapper.Map<CarCreateDTO>(model);
				}
				var response1 = await _variantService.GetAsync<APIResponse>(carId, HttpContext.Session.GetString(SD.SessionToken));
				if (response1 != null &&
					response1.IsSuccess)
				{

					VariantDTO model = JsonConvert.DeserializeObject<VariantDTO>(Convert.ToString(response1.Result));
					variantVM.Variant = _mapper.Map<VariantCreateDTO>(model);
				}


			}
			return View(variantVM);
		}
		//[Authorize(Roles = "admin")]
		[HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateVariant(VariantCreateVM model)
        {
            if (ModelState.IsValid)
            {

                var response = await _variantService.CreateAsync<APIResponse>(model, HttpContext.Session.GetString(SD.SessionToken));
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Variant Created sucessfully.";
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
        public async Task<IActionResult> UpdateVariant(int id)
        {
            VariantUpdateVM variantVM = new();
            var response = await _variantService.GetAsync<APIResponse>(id, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                VariantDTO model = JsonConvert.DeserializeObject<VariantDTO>(Convert.ToString(response.Result));
                variantVM.Variant = _mapper.Map<VariantUpdateDTO>(model);
            }
            var response1 = await _carService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (response1 != null && response1.IsSuccess)
            {
                variantVM.CarList = JsonConvert.DeserializeObject<List<CarDTO>>
                    (Convert.ToString(response1.Result)).Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    });
                return View(variantVM);
            }
            
            return NotFound();
        }


        //[Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateVariant(VariantUpdateVM model)
        {
            if (ModelState.IsValid)
            {

                var response = await _variantService.UpdateAsync<APIResponse>(model.Variant, HttpContext.Session.GetString(SD.SessionToken));
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Variant Updated sucessfully.";
                    return RedirectToAction(nameof(IndexVariant));
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
        //public async Task<IActionResult> DeleteVariant(int id)
        //{
        //    VariantDeleteVM variantVM = new();
        //    var response = await _variantService.GetAsync<APIResponse>(id, HttpContext.Session.GetString(SD.SessionToken));
        //    if (response != null && response.IsSuccess)
        //    {
        //        VariantDTO model = JsonConvert.DeserializeObject<VariantDTO>(Convert.ToString(response.Result));
        //        variantVM.Variant = model;
        //    }

        //    var response1 = await _carService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
        //    if (response1 != null && response1.IsSuccess)
        //    {
        //        variantVM.CarList = JsonConvert.DeserializeObject<List<CarDTO>>
        //            (Convert.ToString(response1.Result)).Select(i => new SelectListItem
        //            {
        //                Text = i.Name,
        //                Value = i.Id.ToString()
        //            });
        //        return View(variantVM);
        //    }
           
               
           


        //    return NotFound();
        //}
        ////[Authorize(Roles = "admin")]
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteVariant(int id)
        {

            var response = await _variantService.DeleteAsync<APIResponse>(id, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Variant Delated sucessfully.";
                return RedirectToAction(nameof(IndexVariant));
            }

			return RedirectToAction(nameof(IndexVariant));
		}



    }
}
