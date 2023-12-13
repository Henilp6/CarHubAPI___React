using AutoMapper;
using CarHub_Utility;
using CarHub_Web.Models;
using CarHub_Web.Models.Dto;
using CarHub_Web.Models.VM;
using CarHub_Web.Service;
using CarHub_Web.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace CarHub_Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class BrandController : Controller
    {
        private readonly IBrandService _brandService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IMapper _mapper;
        public BrandController(IBrandService brandService, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            _brandService = brandService;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;

        }
		public async Task<IActionResult> Index(string term, string orderBy, int currentPage = 1)
		{
			List<BrandDTO> list = new();
			BrandIndexVM brandIndexVM = new();

			var response = await _brandService.AllDataAsync<APIResponse>(term, orderBy, currentPage, HttpContext.Session.GetString(SD.SessionToken));
			if (response != null && response.IsSuccess)
			{
				brandIndexVM = JsonConvert.DeserializeObject<BrandIndexVM>(Convert.ToString(response.Result));
			}
			return View(brandIndexVM);
		}



		//public async Task<IActionResult> IndexBrand()
  //      {
  //          List<BrandDTO> list = new();

  //          var response = await _brandService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
  //          if (response != null && response.IsSuccess)
  //          {
  //              list = JsonConvert.DeserializeObject<List<BrandDTO>>(Convert.ToString(response.Result));
  //          }
  //          return View(list);
  //      }
        //[Authorize(Roles = "admin")]
        public async Task<IActionResult> CreateBrand()
        {
            return View();
        }
        //[Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateBrand(BrandCreateDTO model, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string brandPath = Path.Combine(wwwRootPath, @"images\brand");

                    if (!string.IsNullOrEmpty(model.BrandImage))
                    {
                        //delete the old image
                        var oldImagePath =
                        Path.Combine(wwwRootPath, model.BrandImage.TrimStart('\\'));

                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using (var fileStream = new FileStream(Path.Combine(brandPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    model.BrandImage = @"\images\brand\" + fileName;
                }

                var response = await _brandService.CreateAsync<APIResponse>(model, HttpContext.Session.GetString(SD.SessionToken));
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Brand created successfully";
                    return RedirectToAction(nameof(Index));
                }
            }
            TempData["error"] = "Error encountered.";
            return View(model);
        }


        //[Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateBrand(int brandId)
        {
            var response = await _brandService.GetAsync<APIResponse>(brandId, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                BrandDTO model = JsonConvert.DeserializeObject<BrandDTO>(Convert.ToString(response.Result));
                return View(_mapper.Map<BrandUpdateDTO>(model));
            }
            return NotFound();
        }
        //[Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateBrand(BrandUpdateDTO model)
        {
            if (ModelState.IsValid)
            {
                TempData["success"] = "Brand updated successfully";
                var response = await _brandService.UpdateAsync<APIResponse>(model, HttpContext.Session.GetString(SD.SessionToken));
                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            TempData["error"] = "Error encountered.";
            return View(model);
        }

        //[Authorize(Roles = "admin")]
        //public async Task<IActionResult> DeleteBrand(int brandId)
        //{
        //    var response = await _brandService.GetAsync<APIResponse>(brandId, HttpContext.Session.GetString(SD.SessionToken));
        //    if (response != null && response.IsSuccess)
        //    {
        //        BrandDTO model = JsonConvert.DeserializeObject<BrandDTO>(Convert.ToString(response.Result));
        //        return View(model);
        //    }
        //    return NotFound();
        //}
        ////[Authorize(Roles = "admin")]
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteBrand(int brandId)
        {

            var response = await _brandService.DeleteAsync<APIResponse>(brandId, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Brand deleted successfully";
                return RedirectToAction(nameof(Index));
            }
            TempData["error"] = "Error encountered.";
			return RedirectToAction(nameof(Index));
		}
    }
}
