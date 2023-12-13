using AutoMapper;
using Azure;
using CarHub_API.Models;
using CarHub_API.Models.Dto;
using CarHub_API.Models.VM;
using CarHub_API.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Net;
using System.Security.Claims;
using System.Text.Json;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace CarHub_API.Controllers.v1
{
    [Route("api/v{version:apiVersion}/CarImageAPI")]
    [ApiController]
    [ApiVersion("1.0")]

    public class CarImageAPIController : ControllerBase
    {
        protected APIResponse _response;
        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;

        public CarImageAPIController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _response = new();




        }




        [HttpGet]
        [ResponseCache(CacheProfileName = "Default30")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetCarImages()
        {
            try
            {

                List<CarImage> carImageList = await _unitOfWork.CarImage.GetAllAsync(includeProperties: "Car");
                _response.Result = _mapper.Map<List<CarImageDTO>>(carImageList);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                     = new List<string>() { ex.ToString() };
            }
            return _response;

        }



        [HttpGet("{id:int}", Name = "GetCarImage")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetCarImage(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var carImage = await _unitOfWork.CarImage.GetAsync(u => u.Id == id, includeProperties: "Car");
                if (carImage == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                _response.Result = _mapper.Map<CarImageDTO>(carImage);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                     = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        //[Authorize(Roles = "admin")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CreateCarImage([FromBody] CarImagesCreateVM createDTO)
        {
            try
            {
                List<CarImage> carImageList = _mapper.Map<List<CarImage>>(createDTO.CarImagelist);
                
                Car car = _mapper.Map<Car>(createDTO.Car);

               
                foreach (var item in carImageList)
                {
                    CarImage carImage = new();
                    carImage.CarId = car.Id;
                    carImage.ImageUrl = item.ImageUrl;

                    _unitOfWork.CarImage.CreateAsync(carImage);
                }
                _response.StatusCode = HttpStatusCode.Created;
                return _response;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                     = new List<string>() { ex.ToString() };
            }
            return _response;
        }

     


        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete("{id:int}", Name = "DeleteCarImage")]

        public async Task<ActionResult<APIResponse>> DeleteCarImage(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }
                var image = await _unitOfWork.CarImage.GetAsync(u => u.Id == id);
                if (image == null)
                {
                    return NotFound();
                }
                await _unitOfWork.CarImage.RemoveAsync(image);
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                     = new List<string>() { ex.ToString() };
            }
            return _response;
        }



        [HttpPut("{imageId:int}/{carId:int}", Name = "SetCarImage")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> SetCompanyImage(int imageId, int carId)
        {
            try
            {
                if (carId == 0 && imageId == 0)
                {
                    return BadRequest();
                }

                var car = await _unitOfWork.Car.GetAsync(u => u.Id == carId, tracked: false);
                var carImage = await _unitOfWork.CarImage.GetAsync(u => u.Id == imageId, tracked: false);

                CarUpdateDTO carUpdateDTO = _mapper.Map<CarUpdateDTO>(car);
                carUpdateDTO.ImageURL = carImage.ImageUrl;

                if (car == null)
                {
                    return BadRequest();
                }

                Car model = _mapper.Map<Car>(carUpdateDTO);
                await _unitOfWork.Car.UpdateAsync(model);

                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                     = new List<string>() { ex.ToString() };
            }
            return _response;
        }

    }
}

