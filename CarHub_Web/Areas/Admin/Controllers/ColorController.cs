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
    public class ColorController : Controller
    {
        private readonly IColorService _colorService;
        private readonly IMapper _mapper;
        public ColorController(IColorService colorService, IMapper mapper)
        {
            _colorService = colorService;
            _mapper = mapper;
        }
		public async Task<IActionResult> IndexColor(string term, string orderBy, int currentPage = 1)
		{
			List<ColorDTO> list = new();
			ColorIndexVM colorIndexVM = new();

			var response = await _colorService.AllDataAsync<APIResponse>(term, orderBy, currentPage, HttpContext.Session.GetString(SD.SessionToken));
			if (response != null && response.IsSuccess)
			{
				colorIndexVM = JsonConvert.DeserializeObject<ColorIndexVM>(Convert.ToString(response.Result));
			}
			return View(colorIndexVM);
		}

		//public async Task<IActionResult> IndexColor()
  //      {
  //          List<ColorDTO> list = new();

  //          var response = await _colorService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
  //          if (response != null && response.IsSuccess)
  //          {
  //              list = JsonConvert.DeserializeObject<List<ColorDTO>>(Convert.ToString(response.Result));
  //          }
  //          return View(list);
  //      }
        //[Authorize(Roles = "admin")]
        public async Task<IActionResult> CreateColor()
        {
            return View();
        }
        //[Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateColor(ColorCreateDTO model)
        {
            if (ModelState.IsValid)
            {

                var response = await _colorService.CreateAsync<APIResponse>(model, HttpContext.Session.GetString(SD.SessionToken));
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Color created successfully";
					return RedirectToAction(nameof(IndexColor));
				}
            }
            TempData["error"]
                = "Error encountered.";
            return View(model);
        }


        //[Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateColor(int colorId)
        {
            var response = await _colorService.GetAsync<APIResponse>(colorId, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                ColorDTO model = JsonConvert.DeserializeObject<ColorDTO>(Convert.ToString(response.Result));
                return View(_mapper.Map<ColorUpdateDTO>(model));
            }
            return NotFound();
        }
        //[Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateColor(ColorUpdateDTO model)
        {
            if (ModelState.IsValid)
            {
                TempData["success"] = "Color updated successfully";
                var response = await _colorService.UpdateAsync<APIResponse>(model, HttpContext.Session.GetString(SD.SessionToken));
                if (response != null && response.IsSuccess)
                {
					return RedirectToAction(nameof(IndexColor));
				}
            }
            TempData["error"] = "Error encountered.";
            return View(model);
        }

        //[Authorize(Roles = "admin")]
        //public async Task<IActionResult> DeleteColor(int colorId)
        //{
        //    var response = await _colorService.GetAsync<APIResponse>(colorId, HttpContext.Session.GetString(SD.SessionToken));
        //    if (response != null && response.IsSuccess)
        //    {
        //        ColorDTO model = JsonConvert.DeserializeObject<ColorDTO>(Convert.ToString(response.Result));
        //        return View(model);
        //    }
        //    return NotFound();
        //}
        ////[Authorize(Roles = "admin")]
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteColor(int colorId)
        {

            var response = await _colorService.DeleteAsync<APIResponse>(colorId, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Color deleted successfully";
                return RedirectToAction(nameof(IndexColor));
            }
            TempData["error"] = "Error encountered.";
			return RedirectToAction(nameof(IndexColor));
		}
    }
}
