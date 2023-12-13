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
    public class DealerController : Controller
    {
       
        private readonly IBrandService _brandService;
        private readonly IDealerService _dealerService;

        private readonly IMapper _mapper;
        public DealerController(IBrandService brandService, IMapper mapper, IDealerService dealerService)
        {
            
            _mapper = mapper;
			_brandService =brandService;
			_dealerService = dealerService;

        }

        public async Task<IActionResult> IndexDealer()
        {
            List<DealerDTO> list = new();

            var response = await _dealerService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<DealerDTO>>(Convert.ToString(response.Result));
            }
            return View(list);
        }
        //[Authorize(Roles = "admin")]
        public async Task<IActionResult> CreateDealer()
        {
			DealerCreateVM dealerVM = new();
            {
                var response = await _brandService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
                if (response != null && response.IsSuccess)
                {
					dealerVM.BrandList = JsonConvert.DeserializeObject<List<BrandDTO>>
                        (Convert.ToString(response.Result)).Select(i => new SelectListItem
                        {
                            Text = i.BrandName,
                            Value = i.Id.ToString()
                        });
                }
                

            }

            return View(dealerVM);


            //CarXColorCreateVM carXColorVM = new();
            //var response = await _carService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            //var response1 = await _colorService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            //if (response != null && response.IsSuccess && response1 != null && response1.IsSuccess)
            //{
            //    carXColorVM.CarList = JsonConvert.DeserializeObject<List<CarDTO>>
            //        (Convert.ToString(response.Result)).Select(i => new SelectListItem
            //        {
            //            Text = i.Name,
            //            Value = i.Id.ToString()
            //        });
            //    carXColorVM.ColorList = JsonConvert.DeserializeObject<List<ColorDTO>>
            //        (Convert.ToString(response.Result)).Select(i => new SelectListItem
            //        {
            //            Text = i.ColorName,
            //            Value = i.Id.ToString()
            //        }); 
            //}
            //return View(carXColorVM);
        }
        //[Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateDealer(DealerCreateVM model)
        {
            if (ModelState.IsValid)
            {

                var response = await _dealerService.CreateAsync<APIResponse>(model.Dealer, HttpContext.Session.GetString(SD.SessionToken));
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Dealer Created sucessfully.";
                    return RedirectToAction(nameof(IndexDealer));
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
        public async Task<IActionResult> UpdateDealer(int id)
        {
			DealerUpdateVM dealerVM = new();
            var response = await _dealerService.GetAsync<APIResponse>(id, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
				DealerDTO model = JsonConvert.DeserializeObject<DealerDTO>(Convert.ToString(response.Result));
				dealerVM.Dealer = _mapper.Map<DealerUpdateDTO>(model);
            }
            var response1 = await _dealerService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (response1 != null && response1.IsSuccess)
            {
				dealerVM.BrandList = JsonConvert.DeserializeObject<List<BrandDTO>>
                    (Convert.ToString(response1.Result)).Select(i => new SelectListItem
                    {
                        Text = i.BrandName,
                        Value = i.Id.ToString()
                    });
                return View(dealerVM);
            }
            
            return NotFound();
        }


        //[Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateDealer(DealerUpdateVM model)
        {
            if (ModelState.IsValid)
            {

                var response = await _dealerService.UpdateAsync<APIResponse>(model.Dealer, HttpContext.Session.GetString(SD.SessionToken));
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Dealer Updated sucessfully.";
                    return RedirectToAction(nameof(IndexDealer));
                }
                else
                {
                    if (response.ErrorMessages.Count > 0)
                    {
                        ModelState.AddModelError("ErrorMessages", response.ErrorMessages.FirstOrDefault());
                    }
                }
            }

            var response1 = await _brandService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (response1 != null && response1.IsSuccess)
            {
                model.BrandList = JsonConvert.DeserializeObject<List<BrandDTO>>
                    (Convert.ToString(response1.Result)).Select(i => new SelectListItem
                    {
                        Text = i.BrandName,
                        Value = i.Id.ToString()
                    });
            }
            
            return View(model);
        }
        //[Authorize(Roles = "admin")]
   //     public async Task<IActionResult> DeleteDealer(int id)
   //     {
			//DealerDeleteVM dealerVM = new();
   //         var response = await _dealerService.GetAsync<APIResponse>(id, HttpContext.Session.GetString(SD.SessionToken));
   //         if (response != null && response.IsSuccess)
   //         {
			//	DealerDTO model = JsonConvert.DeserializeObject<DealerDTO>(Convert.ToString(response.Result));
			//	dealerVM.Dealer = model;
   //         }

   //         var response1 = await _dealerService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
   //         if (response1 != null && response1.IsSuccess)
   //         {
			//	dealerVM.BrandList = JsonConvert.DeserializeObject<List<BrandDTO>>
   //                 (Convert.ToString(response1.Result)).Select(i => new SelectListItem
   //                 {
   //                     Text = i.BrandName,
   //                     Value = i.Id.ToString()
   //                 });
   //             return View(dealerVM);
   //         }
           
               
           


   //         return NotFound();
   //     }
        //[Authorize(Roles = "admin")]
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteDealer(int id)
        {

            var response = await _dealerService.DeleteAsync<APIResponse>(id, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Dealer Delated sucessfully.";
                return RedirectToAction(nameof(IndexDealer));
            }

			return RedirectToAction(nameof(IndexDealer));
		}



    }
}
