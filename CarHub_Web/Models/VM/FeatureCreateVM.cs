using CarHub_Web.Models.Dto;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CarHub_Web.Models.VM
{
    public class FeatureCreateVM
    {
        public FeatureCreateVM()
        {
            Feature = new FeatureCreateDTO();
        }
        public FeatureCreateDTO Feature { get; set; }
        public FeatureTypeDTO FeatureType { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> FeatureTypeList { get; set; }

    }
}
