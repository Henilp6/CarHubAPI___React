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
    public class FeatureTypeController : Controller
    {
        private readonly IFeatureTypeService _featureTypeService;
        private readonly IMapper _mapper;
        public FeatureTypeController(IFeatureTypeService featureTypeService, IMapper mapper)
        {
			_featureTypeService = featureTypeService;
            _mapper = mapper;
        }

        public async Task<IActionResult> FeatureTypeByPagination(string term = "", string orderBy = "", int currentPage = 1)
        {
            ViewData["CurrentFilter"] = term;
            term = string.IsNullOrEmpty(term) ? "" : term.ToLower();

            FeatureTypeIndexVM featureTypeIndexVM = new FeatureTypeIndexVM();
            var response = await _featureTypeService.FeatureTypeByPagination<APIResponse>(term, orderBy, currentPage, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                featureTypeIndexVM = JsonConvert.DeserializeObject<FeatureTypeIndexVM>(Convert.ToString(response.Result));
            }
            return View(featureTypeIndexVM);
        }

        public async Task<IActionResult> IndexFeatureType()
        {
            List<FeatureTypeDTO> list = new();

            var response = await _featureTypeService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<FeatureTypeDTO>>(Convert.ToString(response.Result));
            }
            return View(list);
        }
        //[Authorize(Roles = "admin")]
        public async Task<IActionResult> CreateFeatureType()
        {
            return View();
        }
        //[Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateFeatureType(FeatureTypeCreateDTO model)
        {
            if (ModelState.IsValid)
            {

                var response = await _featureTypeService.CreateAsync<APIResponse>(model, HttpContext.Session.GetString(SD.SessionToken));
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "FeatureType created successfully";
					return RedirectToAction(nameof(IndexFeatureType));
				}
            }
            TempData["error"] = "Error encountered.";
            return View(model);
        }


        //[Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateFeatureType(int featureTypeId)
        {
            var response = await _featureTypeService.GetAsync<APIResponse>(featureTypeId, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                FeatureTypeDTO model = JsonConvert.DeserializeObject<FeatureTypeDTO>(Convert.ToString(response.Result));
                return View(_mapper.Map<FeatureTypeUpdateDTO>(model));
            }
            return NotFound();
        }
        //[Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateFeatureType(FeatureTypeUpdateDTO model)
        {
            if (ModelState.IsValid)
            {
                TempData["success"] = "FeatureType updated successfully";
                var response = await _featureTypeService.UpdateAsync<APIResponse>(model, HttpContext.Session.GetString(SD.SessionToken));
                if (response != null && response.IsSuccess)
                {
					return RedirectToAction(nameof(IndexFeatureType));
				}
            }
            TempData["error"] = "Error encountered.";
            return View(model);
        }

        //[Authorize(Roles = "admin")]
        //public async Task<IActionResult> DeleteFeatureType(int featureTypeId)
        //{
        //    var response = await _featureTypeService.GetAsync<APIResponse>(featureTypeId, HttpContext.Session.GetString(SD.SessionToken));
        //    if (response != null && response.IsSuccess)
        //    {
        //        FeatureTypeDTO model = JsonConvert.DeserializeObject<FeatureTypeDTO>(Convert.ToString(response.Result));
        //        return View(model);
        //    }
        //    return NotFound();
        //}
        //[Authorize(Roles = "admin")]
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteFeatureType(int FeatureTypeId)
        {

            var response = await _featureTypeService.DeleteAsync<APIResponse>(FeatureTypeId, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "FeatureType deleted successfully";
                return RedirectToAction(nameof(IndexFeatureType));
            }
            TempData["error"] = "Error encountered.";
			return RedirectToAction(nameof(IndexFeatureType));
		}
    }
}
