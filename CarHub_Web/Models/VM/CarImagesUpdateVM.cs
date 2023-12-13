

using CarHub_Web.Models.Dto;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CarHub_Web.Models.VM
{
	public class CarImagesUpdateVM
	{
		public CarImagesUpdateVM()
		{
            CarImage = new CarImageUpdateDTO();
            Car = new CarCreateDTO();
        }
        public CarImageUpdateDTO CarImage { get; set; }

        public CarCreateDTO Car { get; set; }
     
        [ValidateNever]
        public List<CarImageCreateDTO> CarImagelist { get; set; }
    }
}
