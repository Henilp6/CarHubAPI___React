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
    public class FeatureController : Controller
    {
        private readonly IFeatureService _featureService;
        private readonly IFeatureTypeService _featureTypeService;
        private readonly IMapper _mapper;
        public FeatureController(IFeatureService featureService,IFeatureTypeService featureTypeService, IMapper mapper)
        {
            _featureService = featureService;
            _mapper = mapper;
            _featureTypeService = featureTypeService;
        }

        //public async Task<IActionResult> IndexFeature()
        //{
        //    List<FeatureDTO> list = new();

        //    var response = await _featureService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
        //    if (response != null && response.IsSuccess)
        //    {
        //        list = JsonConvert.DeserializeObject<List<FeatureDTO>>(Convert.ToString(response.Result));
        //    }
        //    return View(list);
        //}
		public async Task<IActionResult> IndexFeature(string term, string orderBy, int currentPage = 1)
		{
			List<FeatureDTO> list = new();
			FeatureIndexVM featureIndexVM = new();

			var response = await _featureService.AllDataAsync<APIResponse>(term, orderBy, currentPage, HttpContext.Session.GetString(SD.SessionToken));
			if (response != null && response.IsSuccess)
			{
				featureIndexVM = JsonConvert.DeserializeObject<FeatureIndexVM>(Convert.ToString(response.Result));
			}
			return View(featureIndexVM);
		}
		//[Authorize(Roles = "admin")]
		public async Task<IActionResult> CreateFeature()
        {
            FeatureCreateVM featureCreateVM = new();
            {
                
                var response1 = await _featureTypeService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
                if (response1 != null && response1.IsSuccess)
                {
                    featureCreateVM.FeatureTypeList = JsonConvert.DeserializeObject<List<FeatureTypeDTO>>
                        (Convert.ToString(response1.Result)).Select(i => new SelectListItem
                        {
                            Text = i.FeatureTypeName,
                            Value = i.Id.ToString()
                        });
                }

            }
            return View(featureCreateVM);
        }
        //[Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateFeature(FeatureCreateVM model)
        {
            if (ModelState.IsValid)
            {



                var response = await _featureService.CreateAsync<APIResponse>(model.Feature, HttpContext.Session.GetString(SD.SessionToken));
                if (response != null && response.IsSuccess)
                {

                    TempData["success"] = "Car created successfully";
                    return RedirectToAction(nameof(IndexFeature));
                }
                else
                {
                    if (response.ErrorMessages.Count > 0)
                    {
                        ModelState.AddModelError("ErrorMessages", response.ErrorMessages.FirstOrDefault());
                    }
                }


              
                var resp = await _featureTypeService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
                if (resp != null && resp.IsSuccess)
                {
                    model.FeatureTypeList = JsonConvert.DeserializeObject<List<FeatureTypeDTO>>
                        (Convert.ToString(resp.Result)).Select(i => new SelectListItem
                        {
                            Text = i.FeatureTypeName,
                            Value = i.Id.ToString()
                        });
                }
             

                return View(model);
            }
            return View(model);
        }

        public async Task<IActionResult> UpdateFeature(int featureId)
        {
            FeatureUpdateVM featureVM = new();
            var response = await _featureService.GetAsync<APIResponse>(featureId, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                FeatureDTO model = JsonConvert.DeserializeObject<FeatureDTO>(Convert.ToString(response.Result));
                featureVM.Feature = _mapper.Map<FeatureUpdateDTO>(model);
            }

            var response1 = await _featureTypeService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (response1 != null && response1.IsSuccess)
            {
                featureVM.FeatureTypeList = JsonConvert.DeserializeObject<List<FeatureTypeDTO>>
                    (Convert.ToString(response1.Result)).Select(i => new SelectListItem
                    {
                        Text = i.FeatureTypeName,
                        Value = i.Id.ToString()
                    });
                return View(featureVM);
            }
         


            return NotFound();
        }

        //[Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateFeature(FeatureUpdateVM model)
        {
            if (ModelState.IsValid)
            {
               
                var response = await _featureService.UpdateAsync<APIResponse>(model.Feature, HttpContext.Session.GetString(SD.SessionToken));
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Feature updated successfully";
                    return RedirectToAction(nameof(IndexFeature));
                }
                else
                {
                    if (response.ErrorMessages.Count > 0)
                    {
                        ModelState.AddModelError("ErrorMessages", response.ErrorMessages.FirstOrDefault());
                    }
                }
            }

           
            var resp = await _featureTypeService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
           
            if (resp != null && resp.IsSuccess)
            {
                model.FeatureTypeList = JsonConvert.DeserializeObject<List<FeatureTypeDTO>>
                    (Convert.ToString(resp.Result)).Select(i => new SelectListItem
                    {
                        Text = i.FeatureTypeName,
                        Value = i.Id.ToString()
                    });
            }
            return View(model);
        }

        //[Authorize(Roles = "admin")]
        //public async Task<IActionResult> DeleteFeature(int featureId)
        //{
        //    var response = await _featureService.GetAsync<APIResponse>(featureId, HttpContext.Session.GetString(SD.SessionToken));
        //    if (response != null && response.IsSuccess)
        //    {
        //        FeatureDTO model = JsonConvert.DeserializeObject<FeatureDTO>(Convert.ToString(response.Result));
        //        return View(model);
        //    }
        //    return NotFound();
        //}
        ////[Authorize(Roles = "admin")]

        public async Task<IActionResult> DeleteFeature(int id)
        {

            var response = await _featureService.DeleteAsync<APIResponse>(id, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Feature deleted successfully";
                return RedirectToAction(nameof(IndexFeature));
            }
            TempData["error"] = "Error encountered.";
			return RedirectToAction(nameof(IndexFeature));
		}
    }
}
