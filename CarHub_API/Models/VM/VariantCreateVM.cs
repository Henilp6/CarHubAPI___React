
using CarHub_API.Models.Dto;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CarHub_API.Models.VM
{
    public class VariantCreateVM
    {
        public VariantCreateVM()
        {
            Variant = new VariantCreateDTO();
			Car = new CarCreateDTO();
		}
		[ValidateNever]
		public CarCreateDTO Car { get; set; }
		public VariantCreateDTO Variant { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> CarList { get; set; }
        
    }
}
