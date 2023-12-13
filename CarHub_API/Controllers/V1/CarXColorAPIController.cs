using AutoMapper;using Azure;using CarHub_API.Data;using CarHub_API.Models;using CarHub_API.Models.Dto;using CarHub_API.Models.VM;using CarHub_API.Repository.IRepository;using Microsoft.AspNetCore.Authorization;using Microsoft.AspNetCore.Http.HttpResults;using Microsoft.AspNetCore.Identity;using Microsoft.AspNetCore.JsonPatch;using Microsoft.AspNetCore.Mvc;using Microsoft.EntityFrameworkCore;using System.Data;using System.Drawing;using System.Net;using System.Security.Claims;using System.Text.Json;using static System.Runtime.InteropServices.JavaScript.JSType;namespace CarHub_API.Controllers.v1{
    [Route("api/v{version:apiVersion}/[controller]/[Action]")]
    [ApiController]    [ApiVersion("1.0")]    public class CarXColorAPIController : ControllerBase    {        protected APIResponse _response;        private readonly IUnitOfWork _unitOfWork;        private readonly IMapper _mapper;        private readonly ApplicationDbContext  _db;        public CarXColorAPIController(IUnitOfWork unitOfWork, IMapper mapper, ApplicationDbContext db)        {            _unitOfWork = unitOfWork;            _mapper = mapper;            _db = db;            _response = new();        }        [HttpGet(Name = "GetCarXColors")]        [ResponseCache(CacheProfileName = "Default30")]        [ProducesResponseType(StatusCodes.Status403Forbidden)]        [ProducesResponseType(StatusCodes.Status401Unauthorized)]        [ProducesResponseType(StatusCodes.Status200OK)]        public async Task<ActionResult<APIResponse>> GetCarXColors([FromQuery(Name = "filterDisplayOrder")] int? Id,            [FromQuery] string? search, int pageSize = 0, int pageNumber = 1)        {            try            {                IEnumerable<CarXColor> carXColorList;                if (Id > 0)                {                    carXColorList = await _unitOfWork.CarXColor.GetAllAsync(u => u.Id == Id, includeProperties: "Car,Color", pageSize: pageSize,                        pageNumber: pageNumber);                }                else                {                    carXColorList = await _unitOfWork.CarXColor.GetAllAsync(includeProperties: "Car,Color", pageSize: pageSize,                        pageNumber: pageNumber);                }                if (!string.IsNullOrEmpty(search))                {                    carXColorList = carXColorList.Where(u => u.Car.Name.ToLower().Contains(search));                }                Pagination pagination = new() { PageNumber = pageNumber, PageSize = pageSize };                Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagination));                _response.Result = _mapper.Map<List<CarXColorDTO>>(carXColorList);                _response.StatusCode = HttpStatusCode.OK;                return Ok(_response);            }            catch (Exception ex)            {                _response.IsSuccess = false;                _response.ErrorMessages                     = new List<string>() { ex.ToString() };            }            return _response;        }        [HttpGet("{id:int}", Name = "GetCarXColor")]        [ProducesResponseType(StatusCodes.Status403Forbidden)]        [ProducesResponseType(StatusCodes.Status401Unauthorized)]        [ProducesResponseType(StatusCodes.Status200OK)]        [ProducesResponseType(StatusCodes.Status400BadRequest)]        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(200, Type =typeof(VillaDTO))]
        //        [ResponseCache(Location =ResponseCacheLocation.None,NoStore =true)]
        public async Task<ActionResult<APIResponse>> GetCarXColor(int id)        {            try            {                if (id == 0)                {                    _response.StatusCode = HttpStatusCode.BadRequest;                    return BadRequest(_response);                }                var carXColor = await _unitOfWork.CarXColor.GetAsync(u => u.Id == id);                if (carXColor == null)                {                    _response.StatusCode = HttpStatusCode.NotFound;                    return NotFound(_response);                }                _response.Result = _mapper.Map<CarXColorDTO>(carXColor);                _response.StatusCode = HttpStatusCode.OK;                return Ok(_response);            }            catch (Exception ex)            {                _response.IsSuccess = false;                _response.ErrorMessages                     = new List<string>() { ex.ToString() };            }            return _response;        }        [HttpPost(Name = "CreateCarXColor")]
        //[Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]        [ProducesResponseType(StatusCodes.Status400BadRequest)]        [ProducesResponseType(StatusCodes.Status500InternalServerError)]        public async Task<ActionResult<APIResponse>> CreateCarXColor([FromForm] CarXColorReact createDTO)        {
            try
            {

                await _unitOfWork.CarXColor.RemoveRangeAsync(u => u.CarId == createDTO.CarId, false);

                foreach (var colorid in createDTO.SelectedColorIds)
                {
                    // bool combinationExists = carXColorList.Any(c => c.CarId == createDTO.CarId && c.ColorId == Convert.ToInt32(colorid));
                    //if (combinationExists)
                    //{
                    CarXColor carXColor = new();
                    carXColor.CarId = createDTO.CarId;
                    carXColor.ColorId = Convert.ToInt32(colorid);
                    await _unitOfWork.CarXColor.CreateAsync(carXColor);

                    //}
                    //else
                    //{
                    //    CarXColor carXColor = new();
                    //    carXColor.CarId = createDTO.CarId;
                    //    carXColor.ColorId = Convert.ToInt32(colorid);
                    //    await _unitOfWork.CarXColor.CreateAsync(carXColor);

                    //}
                }
                _response.StatusCode = HttpStatusCode.Created;
                return _response;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;        }        [ProducesResponseType(StatusCodes.Status204NoContent)]        [ProducesResponseType(StatusCodes.Status403Forbidden)]        [ProducesResponseType(StatusCodes.Status401Unauthorized)]        [ProducesResponseType(StatusCodes.Status404NotFound)]        [ProducesResponseType(StatusCodes.Status400BadRequest)]        [HttpDelete("{id:int}", Name = "DeleteCarXColor")]
        //[Authorize(Roles = "admin")]
        public async Task<ActionResult<APIResponse>> DeleteCarXColor(int id)        {            try            {                if (id == 0)                {                    return BadRequest();                }                var carXColor = await _unitOfWork.CarXColor.GetAsync(u => u.Id == id);                if (carXColor == null)                {                    return NotFound();                }                await _unitOfWork.CarXColor.RemoveAsync(carXColor);                _response.StatusCode = HttpStatusCode.NoContent;                _response.IsSuccess = true;                return Ok(_response);            }            catch (Exception ex)            {                _response.IsSuccess = false;                _response.ErrorMessages                     = new List<string>() { ex.ToString() };            }            return _response;        }

        // [Authorize(Roles = "admin")]
        [HttpPut(Name = "UpdateCarXColor")]        [ProducesResponseType(StatusCodes.Status204NoContent)]        [ProducesResponseType(StatusCodes.Status400BadRequest)]        public async Task<ActionResult<APIResponse>> UpdateCarXColor([FromForm] CarXColorUpdateVM updateDTO)        {            try            {                List<ColorVM> colorVMList = _mapper.Map<List<ColorVM>>(updateDTO.Colorlist);                CarXColor carxcolor = _mapper.Map<CarXColor>(updateDTO.CarXColor);                List<CarXColor> carXColorlist = await _unitOfWork.CarXColor.GetAllAsync(u => u.CarId == carxcolor.CarId, includeProperties: "Car,Color");                foreach (var carlist in carXColorlist)                {                    CarXColor carxColor = new();                    carxColor.Id = carlist.Id;                    carxColor.CarId = carlist.CarId;                    carxColor.ColorId = carlist.ColorId;                    await _unitOfWork.CarXColor.RemoveAsync(carxColor);                }                foreach (var colorlist in colorVMList)                {                    if (colorlist.IsChecked == true)                    {                        CarXColor carxColor = new();                        carxColor.CarId = carxcolor.CarId;                        carxColor.ColorId = colorlist.Id;                        await _unitOfWork.CarXColor.CreateAsync(carxColor);                    }                    else                    {                        continue;                    }                }                _response.StatusCode = HttpStatusCode.NoContent;                _response.IsSuccess = true;                return Ok(_response);            }            catch (Exception ex)            {                _response.IsSuccess = false;                _response.ErrorMessages                     = new List<string>() { ex.ToString() };            }            return _response;        }        [HttpPatch("{id:int}", Name = "UpdatePartialCarXColor")]        [ProducesResponseType(StatusCodes.Status204NoContent)]        [ProducesResponseType(StatusCodes.Status400BadRequest)]        public async Task<IActionResult> UpdatePartialCarXColor(int id, JsonPatchDocument<CarXColorUpdateDTO> patchDTO)        {            if (patchDTO == null || id == 0)            {                return BadRequest();            }            var brand = await _unitOfWork.CarXColor.GetAsync(u => u.Id == id, tracked: false);            CarXColorUpdateDTO carXColorDTO = _mapper.Map<CarXColorUpdateDTO>(brand);            if (brand == null)            {                return BadRequest();            }            patchDTO.ApplyTo(carXColorDTO, ModelState);            CarXColor model = _mapper.Map<CarXColor>(carXColorDTO);            await _unitOfWork.CarXColor.UpdateAsync(model);            if (!ModelState.IsValid)            {                return BadRequest(ModelState);            }            return NoContent();        }

        [HttpGet("{carId:int}", Name = "GetCarXColorByCarId")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetCarXColorByCarId(int carId)
        {
            try
            {
                if (carId == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var states = _db.CarXColors.Include(u => u.Car).Include(u => u.Color).Where(u => u.CarId == carId).ToList();

                if (states == null || states.Count() == 0)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                _response.Result = _mapper.Map<List<CarXColorDTO>>(states);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }

            return _response;
        }    }}