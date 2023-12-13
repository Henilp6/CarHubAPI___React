
using CarHub_API.Models.Dto;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CarHub_API.Models.VM
{
	public class CarXFeatureCreateVM
	{
		public CarXFeatureCreateVM()
		{
			CarXFeature = new CarXFeatureCreateDTO();
			Car = new CarCreateDTO();
		}
		public CarXFeatureCreateDTO CarXFeature { get; set; }
		[ValidateNever]
		public CarCreateDTO Car { get; set; }
		public List<CarXFeatureCreateDTO> CarXFeaturelist { get; set; }
		[ValidateNever]
		public IEnumerable<SelectListItem> CarList { get; set; }
		[ValidateNever]
		public IEnumerable<SelectListItem> FeatureTypeList { get; set; }
		[ValidateNever]
		public List<FeatureVM> Featurelist { get; set; }
	}
}
