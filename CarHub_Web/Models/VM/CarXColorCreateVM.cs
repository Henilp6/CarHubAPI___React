
using CarHub_Web.Models.Dto;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CarHub_Web.Models.VM
{
	public class CarXColorCreateVM
	{
		public CarXColorCreateVM()
		{
			CarXColor = new CarXColorCreateDTO();
			Car = new CarCreateDTO();
		}
		public CarXColorCreateDTO CarXColor { get; set; }
		public List<CarXColorCreateDTO> CarXColorlist { get; set; }
		public CarCreateDTO Car { get; set; }
	
		[ValidateNever]
		public List<ColorVM> Colorlist { get; set; }
	}
}
