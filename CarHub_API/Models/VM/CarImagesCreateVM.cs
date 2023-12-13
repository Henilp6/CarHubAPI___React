using CarHub_API.Models.Dto;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CarHub_API.Models.VM
{
	public class CarImagesCreateVM
	{
		public CarImagesCreateVM()
		{
			CarImage = new CarImageCreateDTO();
			Car = new CarCreateDTO();
		}
		public CarImageCreateDTO CarImage { get; set; }
	
		public CarCreateDTO Car { get; set; }
		//      [ValidateNever]
		//public IEnumerable<SelectListItem> CarList { get; set; }
		[ValidateNever]
		public List<CarImageCreateDTO> CarImagelist { get; set; }
	}
}
