using CarHub_Web.Models.Dto;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CarHub_Web.Models.VM
{
    public class FeatureUpdateVM
    {
        public FeatureUpdateVM()
        {
            Feature = new FeatureUpdateDTO();
        }
        public FeatureUpdateDTO Feature { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> FeatureTypeList { get; set; }

    }
}
