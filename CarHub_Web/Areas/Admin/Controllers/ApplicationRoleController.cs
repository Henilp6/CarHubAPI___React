using AutoMapper;
using CarHub_Utility;
using CarHub_Web.Models;
using CarHub_Web.Models.Dto;
using CarHub_Web.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CarHub_Web.Areas.Admin.Controllers{
	[Area("Admin")]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class ApplicationRoleController : Controller
    {
        private readonly IApplicationRoleService _roleService;
        private readonly IMapper _mapper;
        public ApplicationRoleController(IApplicationRoleService roleService, IMapper mapper)
        {
            _roleService = roleService;
            _mapper = mapper;
        }

        public async Task<IActionResult> IndexApplicationRole()
        {
            List<ApplicationRoleDTO> list = new();

            var response = await _roleService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<ApplicationRoleDTO>>(Convert.ToString(response.Result));
            }
            return View(list);
        }

        public async Task<IActionResult> CreateApplicationRole()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateApplicationRole(ApplicationRoleDTO model)
        {
            if (ModelState.IsValid)
            {

                var response = await _roleService.CreateAsync<APIResponse>(model, HttpContext.Session.GetString(SD.SessionToken));
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "ApplicationRole created successfully";
                    return RedirectToAction(nameof(IndexApplicationRole));
                }
            }
            TempData["error"] = "Error encountered.";
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> UpdateApplicationRole(string ApplicationRoleId)
        {
            var response = await _roleService.GetAsync<APIResponse>(ApplicationRoleId, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                ApplicationRoleDTO model = JsonConvert.DeserializeObject<ApplicationRoleDTO>(Convert.ToString(response.Result));
                return View(model);
            }
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateApplicationRole(ApplicationRoleDTO model)
        {
            if (ModelState.IsValid)
            {

                var response = await _roleService.UpdateAsync<APIResponse>(model, HttpContext.Session.GetString(SD.SessionToken));
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "ApplicationRole updated successfully";
                    return RedirectToAction(nameof(IndexApplicationRole));
                }
            }
            TempData["error"] = "Error encountered.";
            return View(model);
        }

        public async Task<IActionResult> DeleteApplicationRole(string ApplicationRoleId)
        {

            var response = await _roleService.DeleteAsync<APIResponse>(ApplicationRoleId, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "ApplicationRole deleted successfully";
                return RedirectToAction(nameof(IndexApplicationRole));
            }
            TempData["error"] = "Error encountered.";
            return RedirectToAction("Index");
        }
    }}