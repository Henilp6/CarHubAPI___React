using AutoMapper;
using CarHub_Utility;
using CarHub_Web.Models;
using CarHub_Web.Models.Dto;
using CarHub_Web.Models.VM;
using CarHub_Web.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace CarHub_Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class CountryController : Controller
    {
        private readonly ICountryService _countryService;
        private readonly IMapper _mapper;

        public List<CountryDTO> list;
        public CountryController(ICountryService countryService, IMapper mapper)
        {
            _countryService = countryService;
            _mapper = mapper;
        }
        public IActionResult Index()
        {

            return RedirectToAction("GetProjects");
        }
        public async Task<IActionResult> CountryByPagination(string term = "", string orderBy = "", int currentPage = 1)
        {
            ViewData["CurrentFilter"] = term;
            term = string.IsNullOrEmpty(term) ? "" : term.ToLower();

            CountryIndexVM countryIndexVM = new CountryIndexVM();
            var response = await _countryService.CountryByPagination<APIResponse>(term, orderBy, currentPage, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                countryIndexVM = JsonConvert.DeserializeObject<CountryIndexVM>(Convert.ToString(response.Result));
            }
            return View(countryIndexVM);
        }


        public async Task<IActionResult> GetProjects(int pageNum)
        {

            //pageNum = pageNum ?? 0;
            //ViewBag.IsEndOfRecords = false;
            //if (Request.IsAjaxRequest())
            if (pageNum == null)
            {
                pageNum = 0;
            }
            ViewBag.IsEndOfRecords = false;
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                var response = await _countryService.GetCountryData<APIResponse>(pageNum, HttpContext.Session.GetString(SD.SessionToken));
                if (response != null && response.IsSuccess)
                {
                    list = JsonConvert.DeserializeObject<List<CountryDTO>>(Convert.ToString(response.Result));
                }

                ViewBag.IsEndOfRecords = (list.Any());
                ViewBag.list = list;
                return PartialView("_Country", list);
                // return View(list);
            }
            else
            {

                var response = await _countryService.GetCountryData<APIResponse>(pageNum, HttpContext.Session.GetString(SD.SessionToken));
                if (response != null && response.IsSuccess)
                {
                    list = JsonConvert.DeserializeObject<List<CountryDTO>>(Convert.ToString(response.Result));
                }

                ViewBag.TotalNumberProjects = list.Count;
                ViewBag.list = list;

                return View("Index", list);
            }
        }

        //LazyLoading method finish
        public async Task<IActionResult> IndexCountry()
        {
            List<CountryDTO> list = new();

            var response = await _countryService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<CountryDTO>>(Convert.ToString(response.Result));
            }
            return View(list);
        }

     //   [Authorize(Roles ="admin")]
        public async Task<IActionResult> CreateCountry()
        {
            return View();
        }
       // [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCountry(CountryCreateDTO model)
        {
            if (ModelState.IsValid)
            {

                var response = await _countryService.CreateAsync<APIResponse>(model, HttpContext.Session.GetString(SD.SessionToken));
                if (response != null && response.IsSuccess)
{
                    TempData["success"] = "Country created successfully";
                    return RedirectToAction(nameof(CountryByPagination));
                }
            }
            TempData["error"] = "Error encountered.";
            return View(model);
        }


     //   [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateCountry(int CountryId)
{
            var response = await _countryService.GetAsync<APIResponse>(CountryId, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                CountryDTO model = JsonConvert.DeserializeObject<CountryDTO>(Convert.ToString(response.Result));
                return View(_mapper.Map<CountryUpdateDTO>(model));
            }
            return NotFound();
        }
     //   [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateCountry(CountryUpdateDTO model)
        {
            if (ModelState.IsValid)
            {
             
                var response = await _countryService.UpdateAsync<APIResponse>(model, HttpContext.Session.GetString(SD.SessionToken));
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Country updated successfully";
                    return RedirectToAction(nameof(CountryByPagination));
                }
            }
            TempData["error"] = "Error encountered.";
            return View(model);
        }

      //  [Authorize(Roles = "admin")]
        //public async Task<IActionResult> DeleteCountry(int CountryId)
        //{
        //    var response = await _countryService.GetAsync<APIResponse>(CountryId, HttpContext.Session.GetString(SD.SessionToken));
        //    if (response != null && response.IsSuccess)
        //    {
        //        CountryDTO model = JsonConvert.DeserializeObject<CountryDTO>(Convert.ToString(response.Result));
        //        return View(model);
        //    }
        //    return NotFound();
        //}
     //   [Authorize(Roles = "admin")]
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCountry(int CountryId)
        {
            
                var response = await _countryService.DeleteAsync<APIResponse>(CountryId, HttpContext.Session.GetString(SD.SessionToken));
                if (response != null && response.IsSuccess)
                {
                TempData["success"] = "Country deleted successfully";
                return RedirectToAction(nameof(IndexCountry));
                }
            TempData["error"] = "Error encountered.";
			return RedirectToAction(nameof(IndexCountry));
		}
    }
}
