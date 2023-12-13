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
using System.Collections.Generic;
using System.Data;


namespace CarHub_Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class CarXFeatureController : Controller
    {
        private readonly ICarService _carService;
        private readonly IFeatureService _featureService;
        private readonly IFeatureTypeService _featureTypeService;
        private readonly ICarXFeatureService _carXFeatureService;
        private readonly IFeatureXFeaturetypeService _featureXFeaturetypeService;
        private readonly IMapper _mapper;
        public CarXFeatureController(ICarXFeatureService carXFeatureService, IFeatureXFeaturetypeService featureXFeaturetypeService, IMapper mapper, IFeatureService featureService, ICarService carService, IFeatureTypeService featureTypeService)
        {
            _carService = carService;
            _featureService = featureService;
            _featureTypeService = featureTypeService;
            _carXFeatureService = carXFeatureService;
            _featureXFeaturetypeService = featureXFeaturetypeService;

            _mapper = mapper;



        }


        public async Task<IActionResult> IndexCarXFeature()
        {
            List<CarXFeatureDTO> list = new();

            var response = await _carXFeatureService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<CarXFeatureDTO>>(Convert.ToString(response.Result));
            }
            return View(list);
        }


        //  [Authorize(Roles = "admin")]
        public async Task<IActionResult> CreateCarXFeature(int carId, int FeatureTypeId)
        {

            CarXFeatureCreateVM carXFeatureVM = new();
            {
                var response = await _carService.GetAsync<APIResponse>(carId, HttpContext.Session.GetString(SD.SessionToken));
                if (response != null &&
                    response.IsSuccess)
                {

                    CarDTO model = JsonConvert.DeserializeObject<CarDTO>(Convert.ToString(response.Result));
                    carXFeatureVM.Car = _mapper.Map<CarCreateDTO>(model);
                }
                var carxFeaturelist = await _carXFeatureService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
                if (carxFeaturelist != null && carxFeaturelist.IsSuccess)
                {

                    List<CarXFeatureDTO> model1 = JsonConvert.DeserializeObject<List<CarXFeatureDTO>>(Convert.ToString(carxFeaturelist.Result));
                    carXFeatureVM.CarXFeaturelist = _mapper.Map<List<CarXFeatureCreateDTO>>(model1);
                }

                //var featurexList = await _featureService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
                //if (featurexList != null && featurexList.IsSuccess)
                //{
                //	carXFeatureVM.Featurelist = JsonConvert.DeserializeObject<List<FeatureVM>>
                //	  (Convert.ToString(featurexList.Result)).Select(i => new FeatureVM
                //	  {
                //		  Name = i.Name,
                //		  Id = i.Id,
                //		  FeatureTypeId = i.FeatureTypeId,
                //		  IsChecked = carXFeatureVM.CarXFeaturelist.Where(x => x.FeatureId == i.Id && x.CarId == carId && x.FeatureTypeId == FeatureTypeId).Any()
                //	  }).ToList();

                //}

                var featurel = await _featureService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
                if (featurel != null && featurel.IsSuccess)
                {
                    var featurlist = JsonConvert.DeserializeObject<List<FeatureDTO>>(Convert.ToString(featurel.Result));
                    //    carXFeatureVM.FeatureList1 = featurlist.Where(x => x.FeatureType.Id == FeatureTypeId).ToList();
                    var model = featurlist.Where(x => x.FeatureType.Id == FeatureTypeId).ToList();
                    carXFeatureVM.Featurelist = model.Select(i => new FeatureVM
                    {
                        Name = i.Name,
                        Id = i.Id,
                        FeatureTypeId = i.FeatureTypeId,
                        IsChecked = carXFeatureVM.CarXFeaturelist.Where(x => x.FeatureId == i.Id && x.CarId == carId).Any()
                    }).ToList();

                }
                //if (FeatureTypeId != 0)
                //{
                //carXFeatureVM.CarXFeature.FeatureTypeId = FeatureTypeId;

                var response1 = await _featureTypeService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
                if (response1 != null && response1.IsSuccess)
                {
                    carXFeatureVM.FeatureTypeList = JsonConvert.DeserializeObject<List<FeatureTypeDTO>>
                            (Convert.ToString(response1.Result)).Select(i => new SelectListItem
                            {
                                Text = i.FeatureTypeName,
                                Value = i.Id.ToString()
                            }).ToList();
                }

                
                
                foreach (var item in carXFeatureVM.FeatureTypeList)
                {
                    if (item.Value == FeatureTypeId.ToString())
                    {
                        item.Selected = true;
                        break;
                    }
                   
                }





                //var response1 = await _featureTypeService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
                //if (response1 != null && response1.IsSuccess)
                //{
                //    carXFeatureVM.FeatureTypeList = JsonConvert.DeserializeObject<List<FeatureTypeDTO>>
                //            (Convert.ToString(response1.Result)).Select(i => new SelectListItem
                //            {
                //                Text = i.FeatureTypeName,
                //                Value = i.Id.ToString()
                //            });

                //if (FeatureTypeId > 0)
                //{
                //	var list = JsonConvert.DeserializeObject<List<FeatureTypeDTO>>
                //		(Convert.ToString(response1.Result));
                //	var model = list.Where(x=> x.Id == FeatureTypeId).ToList();
                //                   carXFeatureVM.FeatureTypeList = model.Select(i => new SelectListItem
                //                   {
                //                       Text = i.FeatureTypeName,
                //                       Value = i.Id.ToString()
                //                   });

                //               }
                //else
                //{

                //                   carXFeatureVM.FeatureTypeList = JsonConvert.DeserializeObject<List<FeatureTypeDTO>>
                //                       (Convert.ToString(response1.Result)).Select(i => new SelectListItem
                //                       {
                //                           Text = i.FeatureTypeName,
                //                           Value = i.Id.ToString()
                //                       });
                //               }

                //}
            };
           
                return View(carXFeatureVM);
            

        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> CreateCarXFeature(CarXFeatureCreateVM carXFeatureVM)
        {
            if (ModelState.IsValid)
            {

                var response = await _carXFeatureService.CreateAsync<APIResponse>(carXFeatureVM, HttpContext.Session.GetString(SD.SessionToken));
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "CarXFeature Added successfully";
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

            var carXFeaturelist = await _carXFeatureService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (carXFeaturelist != null && carXFeaturelist.IsSuccess)
            {

                List<CarXFeatureDTO> model1 = JsonConvert.DeserializeObject<List<CarXFeatureDTO>>(Convert.ToString(carXFeaturelist.Result));
                carXFeatureVM.CarXFeaturelist = _mapper.Map<List<CarXFeatureCreateDTO>>(model1);
            }

            var featurexList = await _featureService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (featurexList != null && featurexList.IsSuccess)
            {
                carXFeatureVM.Featurelist = JsonConvert.DeserializeObject<List<FeatureVM>>
                  (Convert.ToString(featurexList.Result)).Select(i => new FeatureVM
                  {
                      Name = i.Name,
                      Id = i.Id,
                      IsChecked = carXFeatureVM.CarXFeaturelist.Where(x => x.FeatureId == i.Id).Any()

                  }).ToList();
            }
            var response1 = await _featureTypeService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (response1 != null && response1.IsSuccess)
            {
                carXFeatureVM.FeatureTypeList = JsonConvert.DeserializeObject<List<FeatureTypeDTO>>
                    (Convert.ToString(response1.Result)).Select(i => new SelectListItem
                    {
                        Text = i.FeatureTypeName,
                        Value = i.Id.ToString()
                    }).ToList();
            }


            return View(carXFeatureVM);
        }


        public async Task<IActionResult> UpdateCarXFeature(int CarXFeatureId)
        {
            CarXFeatureUpdateVM carXFeatureVM = new();
            var response = await _carXFeatureService.GetAsync<APIResponse>(CarXFeatureId, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                CarXFeatureDTO model = JsonConvert.DeserializeObject<CarXFeatureDTO>(Convert.ToString(response.Result));
                carXFeatureVM.CarXFeature = _mapper.Map<CarXFeatureUpdateDTO>(model);
            }


            response = await _carService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {

                carXFeatureVM.CarList = JsonConvert.DeserializeObject<List<CarDTO>>
                    (Convert.ToString(response.Result)).Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    });
            }

            var response1 = await _featureTypeService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (response1 != null && response1.IsSuccess)
            {
                carXFeatureVM.FeatureTypeList = JsonConvert.DeserializeObject<List<FeatureTypeDTO>>
                    (Convert.ToString(response1.Result)).Select(i => new SelectListItem
                    {
                        Text = i.FeatureTypeName,
                        Value = i.Id.ToString()
                    });
            }

            var featureList = await _featureService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (featureList != null && featureList.IsSuccess)
            {
                carXFeatureVM.Featurelist = JsonConvert.DeserializeObject<List<FeatureVM>>
                    (Convert.ToString(featureList.Result)).Select(i => new FeatureVM
                    {
                        Name = i.Name,
                        Id = i.Id,
                        IsChecked = true

                    }).ToList();

            }
            return View(carXFeatureVM);



        }

        // [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateCarXFeature(CarXFeatureUpdateVM model)
        {
            if (ModelState.IsValid)
            {
                TempData["success"] = "CarXFeature updated successfully";
                var response = await _carXFeatureService.UpdateAsync<APIResponse>(model, HttpContext.Session.GetString(SD.SessionToken));
                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(IndexCarXFeature));
                }
                else
                {
                    if (response.ErrorMessages.Count > 0)
                    {
                        ModelState.AddModelError("ErrorMessages", response.ErrorMessages.FirstOrDefault());
                    }
                }
            }

            var resp = await _carService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));

            if (resp != null && resp.IsSuccess)
            {
                model.CarList = JsonConvert.DeserializeObject<List<CarDTO>>
                    (Convert.ToString(resp.Result)).Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    });

            }
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

        public async Task<IActionResult> DeleteCarXFeature(int id)
        {

            var response = await _carXFeatureService.DeleteAsync<APIResponse>(id, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "CarXFeature deleted successfully";
                return RedirectToAction(nameof(IndexCarXFeature));
            }
            TempData["error"] = "Error encountered.";
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetFeaturesByType(int FeatureTypeId, int carId)
        {
            List<CarXFeatureDTO> list = new();
            var carxFeaturelist = await _carXFeatureService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (carxFeaturelist != null && carxFeaturelist.IsSuccess)
            {

                List<CarXFeatureDTO> model1 = JsonConvert.DeserializeObject<List<CarXFeatureDTO>>(Convert.ToString(carxFeaturelist.Result));
                list = _mapper.Map<List<CarXFeatureDTO>>(model1);
            }


            var response1 = await _featureTypeService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (response1 != null && response1.IsSuccess)
            {
                var FeatureTypeList = JsonConvert.DeserializeObject<List<FeatureTypeDTO>>
                        (Convert.ToString(response1.Result)).Select(i => new SelectListItem
                        {
                            Text = i.FeatureTypeName,
                            Value = i.Id.ToString()
                        }).ToList();
            }

            var featurel = await _featureService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            List<FeatureVM> featureVMList = new List<FeatureVM>();
            if (featurel != null && featurel.IsSuccess)
            {
                var featurlist = JsonConvert.DeserializeObject<List<FeatureDTO>>(Convert.ToString(featurel.Result));
                var model = featurlist.Where(x => x.FeatureType.Id == FeatureTypeId).ToList();

                featureVMList = model.Select(i => new FeatureVM
                {
                    Name = i.Name,
                    Id = i.Id,
                    FeatureTypeId = i.FeatureTypeId,
                    IsChecked = list.Where(x => x.FeatureId == i.Id && x.CarId == carId).Any() 
                }).ToList();
            }

            return Json(featureVMList);
        }


    }

}