using CarHub_Web.Models.Dto;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CarHub_Web.Models.VM
{
	public class CarCreateVM
	{
		public CarCreateVM()
		{
			Car = new CarCreateDTO();
		}
		public CarCreateDTO Car { get; set; }
		[ValidateNever]
		public IEnumerable<SelectListItem> BrandList { get; set; }
		[ValidateNever]
		public IEnumerable<SelectListItem> CarTypeList { get; set; }
	}
}
