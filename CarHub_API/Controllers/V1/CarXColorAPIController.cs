﻿using AutoMapper;
    [Route("api/v{version:apiVersion}/[controller]/[Action]")]
    [ApiController]
        //[ProducesResponseType(200, Type =typeof(VillaDTO))]
        //        [ResponseCache(Location =ResponseCacheLocation.None,NoStore =true)]
        public async Task<ActionResult<APIResponse>> GetCarXColor(int id)
        //[Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
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
            return _response;
        //[Authorize(Roles = "admin")]
        public async Task<ActionResult<APIResponse>> DeleteCarXColor(int id)

        // [Authorize(Roles = "admin")]
        [HttpPut(Name = "UpdateCarXColor")]

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
        }