using CarHub_Web.Models.Dto;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CarHub_Web.Models.VM
{
	public class CarXColorUpdateVM
	{
		public CarXColorUpdateVM()
		{
			CarXColor = new CarXColorUpdateDTO();
		}
		public CarXColorUpdateDTO CarXColor { get; set; }
		[ValidateNever]
		public IEnumerable<SelectListItem> CarList { get; set; }
		[ValidateNever]
		public List<ColorVM> Colorlist { get; set; }
	}
}
