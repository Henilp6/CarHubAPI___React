

using CarHub_Web.Models.Dto;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CarHub_Web.Models.VM
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

        public List<FeatureXFeaturetypeCreateDTO> FeatureXFeaturetypelist { get; set; }
        public List<CarXFeatureCreateDTO> CarXFeaturelist { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> CarList { get; set; }
        [ValidateNever]
        public List<SelectListItem> FeatureTypeList { get; set; }
        [ValidateNever]
        public List<FeatureVM> Featurelist { get; set; }
        [ValidateNever]
        public List<FeatureXFeaturetypeDTO> FeatureXFeaturetypeList { get; set; }

        [ValidateNever]
        public List<FeatureDTO> FeatureList1 { get; set; }
        public FeatureTypeDTO Featuretype { get; set; }

    }
}
