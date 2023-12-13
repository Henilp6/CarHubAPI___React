using AutoMapper;
using CarHub_Utility;
using CarHub_Web.Models;
using CarHub_Web.Models.Dto;
using CarHub_Web.Models.VM;
using CarHub_Web.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Data;
using System.Security.Claims;

namespace CarHub_Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class CityController : Controller
    {
        private readonly IStateService _stateService;
        private readonly ICityService _cityService;
        private readonly ICountryService _countryService;
        private readonly IMapper _mapper;
        public CityController(IStateService stateService, IMapper mapper, ICityService cityService, ICountryService countryService)
        {
            _stateService = stateService;
            _cityService = cityService;
            _countryService = countryService;
            _mapper = mapper;
        }

        //public async Task<IActionResult> IndexCity()
        //{
        //    List<CityDTO> list = new();

        //    var response = await _cityService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
        //    if (response != null && response.IsSuccess)
        //    {
        //        list = JsonConvert.DeserializeObject<List<CityDTO>>(Convert.ToString(response.Result));
        //    }
        //    return View(list);
        //}
        public async Task<IActionResult> IndexCity(string term = "", string orderBy = "", int currentPage = 1)
        {
            ViewData["CurrentFilter"] = term;
            //term = string.IsNullOrEmpty(term) ? "" : term.ToLower();

            CityIndexVM cityIndexVM = new CityIndexVM();
            var response = await _cityService.CityByPagination<APIResponse>(term, orderBy, currentPage, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                cityIndexVM = JsonConvert.DeserializeObject<CityIndexVM>(Convert.ToString(response.Result));
            }
            return View(cityIndexVM);
        }

        public async Task<IActionResult> CreateCity()
        {
            CityCreateVM cityCategoryVM = new();
            var response = await _stateService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                cityCategoryVM.StateList = JsonConvert.DeserializeObject<List<StateDTO>>
                    (Convert.ToString(response.Result)).Select(i => new SelectListItem
                    {
                        Text = i.StateName,
                        Value = i.Id.ToString()
                    });
            }
            var response1 = await _countryService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (response1 != null && response1.IsSuccess)
            {
                cityCategoryVM.CountryList = JsonConvert.DeserializeObject<List<CountryDTO>>
                    (Convert.ToString(response1.Result)).Select(i => new SelectListItem
                    {
                        Text = i.CountryName,
                        Value = i.Id.ToString()
                    });
            }
            return View(cityCategoryVM);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCity(CityCreateVM model)
        {
            if (ModelState.IsValid)
            {
                //var claimsIdentity = (ClaimsIdentity)User.Identity;
                //var userId = claimsIdentity.FindSecond(ClaimTypes.NameIdentifier).Value;
                //string ApplicationUserId = userId;

                var response = await _cityService.CreateAsync<APIResponse>(model.City, HttpContext.Session.GetString(SD.SessionToken));
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Data Created sucessfully.";
                    return RedirectToAction(nameof(IndexCity));
                }
                else
                {
                    if (response.ErrorMessages.Count > 0)
                    {
                        ModelState.AddModelError("ErrorMessages", response.ErrorMessages.FirstOrDefault());
                    }
                }
            }

            var resp = await _stateService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (resp != null && resp.IsSuccess)
            {
                model.StateList = JsonConvert.DeserializeObject<List<StateDTO>>
                    (Convert.ToString(resp.Result)).Select(i => new SelectListItem
                    {
                        Text = i.StateName,
                        Value = i.Id.ToString()
                    });
            }
            var res = await _countryService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (res != null && res.IsSuccess)
            {
                model.CountryList = JsonConvert.DeserializeObject<List<CountryDTO>>
                    (Convert.ToString(res.Result)).Select(i => new SelectListItem
                    {
                        Text = i.CountryName,
                        Value = i.Id.ToString()
                    });
            }
            return View(model);
        }

        public async Task<IActionResult> UpdateCity(int id)
        {
            CityUpdateVM cityCategoryVM = new();
            var response = await _cityService.GetAsync<APIResponse>(id, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                CityDTO model = JsonConvert.DeserializeObject<CityDTO>(Convert.ToString(response.Result));
                cityCategoryVM.City = _mapper.Map<CityUpdateDTO>(model);
            }

            response = await _stateService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                cityCategoryVM.StateList = JsonConvert.DeserializeObject<List<StateDTO>>
                    (Convert.ToString(response.Result)).Select(i => new SelectListItem
                    {
                        Text = i.StateName,
                        Value = i.Id.ToString()
                    });
            }
            response = await _countryService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                cityCategoryVM.CountryList = JsonConvert.DeserializeObject<List<CountryDTO>>
                    (Convert.ToString(response.Result)).Select(i => new SelectListItem
                    {
                        Text = i.CountryName,
                        Value = i.Id.ToString()
                    });
            }
            return View(cityCategoryVM);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateCity(CityUpdateVM model)
        {
            if (ModelState.IsValid)
            {
                var response = await _cityService.UpdateAsync<APIResponse>(model.City, HttpContext.Session.GetString(SD.SessionToken));
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Data Updated sucessfully.";
                    return RedirectToAction(nameof(IndexCity));
                }
                else
                {
                    if (response.ErrorMessages.Count > 0)
                    {
                        TempData["error"] = response.ErrorMessages.FirstOrDefault();
                    }
                }
            }

            var resp = await _countryService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (resp != null && resp.IsSuccess)
            {
                model.CountryList = JsonConvert.DeserializeObject<List<CountryDTO>>
                    (Convert.ToString(resp.Result)).Select(i => new SelectListItem
                    {
                        Text = i.CountryName,
                        Value = i.Id.ToString()
                    }); ;
            }

            var res = await _stateService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (res != null && res.IsSuccess)
            {
                model.StateList = JsonConvert.DeserializeObject<List<StateDTO>>
                    (Convert.ToString(res.Result)).Select(i => new SelectListItem
                    {
                        Text = i.StateName,
                        Value = i.Id.ToString()
                    }); ;
            }
            return View(model);
        }

        //public async Task<IActionResult> UpdateCity(int id)
        //{
        //    CityUpdateVM cityCategoryVM = new();
        //    var response = await _cityService.GetAsync<APIResponse>(id, HttpContext.Session.GetString(SD.SessionToken));
        //    if (response != null && response.IsSuccess)
        //    {
        //        CityDTO model = JsonConvert.DeserializeObject<CityDTO>(Convert.ToString(response.Result));
        //        cityCategoryVM.City = _mapper.Map<CityUpdateDTO>(model);
        //    }

        //    response = await _stateService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
        //    if (response != null && response.IsSuccess)
        //    {
        //        cityCategoryVM.StateList = JsonConvert.DeserializeObject<List<StateDTO>>
        //            (Convert.ToString(response.Result)).Select(i => new SelectListItem
        //            {
        //                Text = i.StateName,
        //                Value = i.Id.ToString()
        //            });
        //    }
        //    response = await _countryService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
        //    if (response != null && response.IsSuccess)
        //    {
        //        cityCategoryVM.CountryList = JsonConvert.DeserializeObject<List<CountryDTO>>
        //            (Convert.ToString(response.Result)).Select(i => new SelectListItem
        //            {
        //                Text = i.CountryName,
        //                Value = i.Id.ToString()
        //            });
        //    }
        //    return View(cityCategoryVM);
        //}


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> UpdateCity(CityUpdateVM model)
        //{
        //    if (ModelState.IsValid)
        //    {

        //        var response = await _cityService.UpdateAsync<APIResponse>(model.City, HttpContext.Session.GetString(SD.SessionToken));
        //        if (response != null && response.IsSuccess)
        //        {
        //            TempData["success"] = "Data Updated sucessfully.";
        //            return RedirectToAction(nameof(IndexCity));
        //        }
        //        else
        //        {
        //            if (response.ErrorMessages.Count > 0)
        //            {
        //                ModelState.AddModelError("ErrorMessages", response.ErrorMessages.FirstOrDefault());
        //            }
        //        }
        //    }

        //    var resp = await _countryService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
        //    if (resp != null && resp.IsSuccess)
        //    {
        //        model.CountryList = JsonConvert.DeserializeObject<List<CountryDTO>>
        //            (Convert.ToString(resp.Result)).Select(i => new SelectListItem
        //            {
        //                Text = i.CountryName,
        //                Value = i.Id.ToString()
        //            }); ;
        //    }

        //    var res = await _stateService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
        //    if (res != null && res.IsSuccess)
        //    {
        //        model.StateList = JsonConvert.DeserializeObject<List<StateDTO>>
        //            (Convert.ToString(res.Result)).Select(i => new SelectListItem
        //            {
        //                Text = i.StateName,
        //                Value = i.Id.ToString()
        //            }); ;
        //    }
        //    return View(model);
        //}


        //public async Task<IActionResult> DeleteCity(int id)
        //{
        //    CityDeleteVM cityCategoryVM = new();
        //    var response = await _cityService.GetAsync<APIResponse>(id, HttpContext.Session.GetString(SD.SessionToken));
        //    if (response != null && response.IsSuccess)
        //    {
        //        CityDTO model = JsonConvert.DeserializeObject<CityDTO>(Convert.ToString(response.Result));
        //        cityCategoryVM.City = model;
        //    }

        //    var response2 = await _stateService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
        //    var response1 = await _countryService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
        //    if (response2 != null && response2.IsSuccess && response1 != null && response1.IsSuccess)
        //    {
        //        cityCategoryVM.StateList = JsonConvert.DeserializeObject<List<StateDTO>>
        //            (Convert.ToString(response2.Result)).Select(i => new SelectListItem
        //            {
        //                Text = i.StateName,
        //                Value = i.Id.ToString()
        //            });
        //        cityCategoryVM.CountryList = JsonConvert.DeserializeObject<List<CountryDTO>>
        //            (Convert.ToString(response1.Result)).Select(i => new SelectListItem
        //            {
        //                Text = i.CountryName,
        //                Value = i.Id.ToString()
        //            });
        //        return View(cityCategoryVM);
        //    }
        //    return NotFound();
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCity(int id)

        {

            var response = await _cityService.DeleteAsync<APIResponse>(id, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Data Delated sucessfully.";
                return RedirectToAction(nameof(IndexCity));
            }

			return RedirectToAction(nameof(IndexCity));
		}



    }
}
