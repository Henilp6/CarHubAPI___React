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
    public class FeatureXFeaturetypeController : Controller
	{
		
		private readonly IFeatureService _featureService;
        private readonly IFeatureTypeService _featureTypeService;
        private readonly IFeatureXFeaturetypeService _featureXFeaturetypeService;
		private readonly IMapper _mapper;
		public FeatureXFeaturetypeController(IFeatureXFeaturetypeService featureXFeaturetypeService, IMapper mapper, IFeatureService featureService, IFeatureTypeService featureTypeService)
		{
			
			_featureService= featureService;
			_featureTypeService= featureTypeService;
			_featureXFeaturetypeService = featureXFeaturetypeService;

			_mapper = mapper;
			


		}


		//public async Task<IActionResult> IndexFeatureXFeaturetype()
		//{
		//	List<FeatureXFeaturetypeDTO> list = new();

		//	var response = await _featureXFeaturetypeService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
		//	if (response != null && response.IsSuccess)
		//	{
		//		list = JsonConvert.DeserializeObject<List<FeatureXFeaturetypeDTO>>(Convert.ToString(response.Result));
		//	}
		//	return View(list);
		//}

		public async Task<IActionResult> IndexFeatureXFeaturetype(string term, string orderBy, int currentPage = 1)
		{
			List<FeatureXFeaturetypeDTO> list = new();
			FeatureXFeaturetypeIndexVM featureXFeaturetypeIndexVM = new();

			var response = await _featureXFeaturetypeService.AllDataAsync<APIResponse>(term, orderBy, currentPage, HttpContext.Session.GetString(SD.SessionToken));
			if (response != null && response.IsSuccess)
			{
				featureXFeaturetypeIndexVM = JsonConvert.DeserializeObject<FeatureXFeaturetypeIndexVM>(Convert.ToString(response.Result));
			}
			return View(featureXFeaturetypeIndexVM);
		}
		//  [Authorize(Roles = "admin")]
		public async Task<IActionResult> CreateFeatureXFeaturetype()
		{
			FeatureXFeaturetypeCreateVM featureXFeaturetypeVM = new();
			{
				
				var featureXFeaturetypelist = await _featureXFeaturetypeService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
				if (featureXFeaturetypelist != null && featureXFeaturetypelist.IsSuccess)
				{

					List<FeatureXFeaturetypeDTO> model1 = JsonConvert.DeserializeObject<List<FeatureXFeaturetypeDTO>>(Convert.ToString(featureXFeaturetypelist.Result));
					featureXFeaturetypeVM.FeatureXFeaturetypelist = _mapper.Map<List<FeatureXFeaturetypeCreateDTO>>(model1);
				}

				var featurexList = await _featureService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
				if (featurexList != null && featurexList.IsSuccess)
				{
					featureXFeaturetypeVM.Featurelist = JsonConvert.DeserializeObject<List<FeatureVM>>
					  (Convert.ToString(featurexList.Result)).Select(i => new FeatureVM
					  {
						  Name = i.Name,
						  Id = i.Id,
						  IsChecked = featureXFeaturetypeVM.FeatureXFeaturetypelist.Where(x => x.FeatureId == i.Id ).Any()

					  }).ToList();
				}
				var response1 = await _featureTypeService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
				if (response1 != null && response1.IsSuccess)
				{
					featureXFeaturetypeVM.FeatureTypeList = JsonConvert.DeserializeObject<List<FeatureTypeDTO>>
						(Convert.ToString(response1.Result)).Select(i => new SelectListItem
						{
							Text = i.FeatureTypeName,
							Value = i.Id.ToString()
						});
				}
			};
			return View(featureXFeaturetypeVM);
		}
		[HttpPost]
		[ValidateAntiForgeryToken]

		public async Task<IActionResult> CreateFeatureXFeaturetype(FeatureXFeaturetypeCreateVM featureXFeaturetypeVM)
		{
			if (ModelState.IsValid)
			{
				TempData["success"] = "FeatureXFeaturetype Added successfully";
				var response = await _featureXFeaturetypeService.CreateAsync<APIResponse>(featureXFeaturetypeVM,HttpContext.Session.GetString(SD.SessionToken));
                if (response != null && response.IsSuccess)
				{
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

			var featureXFeaturetypelist = await _featureXFeaturetypeService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
			if (featureXFeaturetypelist != null && featureXFeaturetypelist.IsSuccess)
			{

				List<FeatureXFeaturetypeDTO> model1 = JsonConvert.DeserializeObject<List<FeatureXFeaturetypeDTO>>(Convert.ToString(featureXFeaturetypelist.Result));
				featureXFeaturetypeVM.FeatureXFeaturetypelist = _mapper.Map<List<FeatureXFeaturetypeCreateDTO>>(model1);
			}

			var featurexList = await _featureService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
			if (featurexList != null && featurexList.IsSuccess)
			{
				featureXFeaturetypeVM.Featurelist = JsonConvert.DeserializeObject<List<FeatureVM>>
				  (Convert.ToString(featurexList.Result)).Select(i => new FeatureVM
				  {
					  Name = i.Name,
					  Id = i.Id,
					  IsChecked = featureXFeaturetypeVM.FeatureXFeaturetypelist.Where(x => x.FeatureId == i.Id).Any()

				  }).ToList();
			}
			var response1 = await _featureTypeService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
			if (response1 != null && response1.IsSuccess)
			{
				featureXFeaturetypeVM.FeatureTypeList = JsonConvert.DeserializeObject<List<FeatureTypeDTO>>
					(Convert.ToString(response1.Result)).Select(i => new SelectListItem
					{
						Text = i.FeatureTypeName,
						Value = i.Id.ToString()
					});
			}

			return View(featureXFeaturetypeVM);
		}


		public async Task<IActionResult> UpdateFeatureXFeaturetype(int FeatureXFeaturetypeId)
		{
			FeatureXFeaturetypeUpdateVM featureXFeaturetypeVM = new();
			var response = await _featureXFeaturetypeService.GetAsync<APIResponse>(FeatureXFeaturetypeId, HttpContext.Session.GetString(SD.SessionToken));
			if (response != null && response.IsSuccess)
			{
				FeatureXFeaturetypeDTO model = JsonConvert.DeserializeObject<FeatureXFeaturetypeDTO>(Convert.ToString(response.Result));
				featureXFeaturetypeVM.FeatureXFeaturetype = _mapper.Map<FeatureXFeaturetypeUpdateDTO>(model);
			}


			//response = await _carService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
			//if (response != null && response.IsSuccess)
			//{

			//	featureXFeaturetypeVM.CarList = JsonConvert.DeserializeObject<List<CarDTO>>
			//		(Convert.ToString(response.Result)).Select(i => new SelectListItem
			//		{
			//			Text = i.Name,
			//			Value = i.Id.ToString()
			//		});
			//}

            var response1 = await _featureTypeService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (response1 != null && response1.IsSuccess)
            {
                featureXFeaturetypeVM.FeatureTypeList = JsonConvert.DeserializeObject<List<FeatureTypeDTO>>
                    (Convert.ToString(response1.Result)).Select(i => new SelectListItem
                    {
                        Text = i.FeatureTypeName,
                        Value = i.Id.ToString()
                    });
            }

            var featureList = await _featureService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
			if (featureList != null && featureList.IsSuccess)
			{
				featureXFeaturetypeVM.Featurelist = JsonConvert.DeserializeObject<List<FeatureVM>>
					(Convert.ToString(featureList.Result)).Select(i => new FeatureVM
					{
						Name = i.Name,
						Id = i.Id,
						IsChecked = true

					}).ToList();

			}
			return View(featureXFeaturetypeVM);



		}

		// [Authorize(Roles = "admin")]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> UpdateFeatureXFeaturetype(FeatureXFeaturetypeUpdateVM model)
		{
			if (ModelState.IsValid)
			{
				TempData["success"] = "FeatureXFeaturetype updated successfully";
				var response = await _featureXFeaturetypeService.UpdateAsync<APIResponse>(model, HttpContext.Session.GetString(SD.SessionToken));
				if (response != null && response.IsSuccess)
				{
					return RedirectToAction(nameof(IndexFeatureXFeaturetype));
				}
				else
				{
					if (response.ErrorMessages.Count > 0)
					{
						ModelState.AddModelError("ErrorMessages", response.ErrorMessages.FirstOrDefault());
					}
				}
			}

			//var resp = await _carService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));

			//if (resp != null && resp.IsSuccess)
			//{
			//	model.CarList = JsonConvert.DeserializeObject<List<CarDTO>>
			//		(Convert.ToString(resp.Result)).Select(i => new SelectListItem
			//		{
			//			Text = i.Name,
			//			Value = i.Id.ToString()
			//		});

			//}
            var response1 = await _featureTypeService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (response1 != null && response1.IsSuccess)
            {
                model.FeatureTypeList = JsonConvert.DeserializeObject<List<FeatureTypeDTO>>
                    (Convert.ToString(response1.Result)).Select(i => new SelectListItem
                    {
                        Text = i.FeatureTypeName,
                        Value = i.Id.ToString()
                    });
            }
            var featureList = await _featureService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));

			if (featureList != null && featureList.IsSuccess)
			{
				model.Featurelist = JsonConvert.DeserializeObject<List<FeatureVM>>
						  (Convert.ToString(featureList.Result)).Select(i => new FeatureVM
						  {
							  Name = i.Name,
							  Id = i.Id,
							  IsChecked = true

						  }).ToList();

			}

			return View(model);
		}



		// [Authorize(Roles = "admin")]

		public async Task<IActionResult> DeleteFeatureXFeaturetype(int id)
		{

			var response = await _featureXFeaturetypeService.DeleteAsync<APIResponse>(id, HttpContext.Session.GetString(SD.SessionToken));
			if (response != null && response.IsSuccess)
			{
				TempData["success"] = "FeatureXFeaturetype deleted successfully";
				return RedirectToAction(nameof(IndexFeatureXFeaturetype));
			}
			TempData["error"] = "Error encountered.";
			return View();
		}

	}

}