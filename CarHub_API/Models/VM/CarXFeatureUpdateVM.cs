using CarHub_API.Models.Dto;
using CarHub_API.Models.VM;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CarHub_API.Models.VM
{
	public class CarXFeatureUpdateVM
	{
		public CarXFeatureUpdateVM()
		{
			CarXFeature = new CarXFeatureUpdateDTO();
		}
		public CarXFeatureUpdateDTO CarXFeature { get; set; }
		[ValidateNever]
		public IEnumerable<SelectListItem> CarList { get; set; }
		[ValidateNever]
		public IEnumerable<SelectListItem> FeatureTypeList { get; set; }
		[ValidateNever]
		public List<FeatureVM> Featurelist { get; set; }
	}
}
