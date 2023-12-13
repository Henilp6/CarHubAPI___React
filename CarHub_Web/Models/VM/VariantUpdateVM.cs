

using CarHub_Web.Models.Dto;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CarHub_Web.Models.VM
{
    public class VariantUpdateVM
    {
        public VariantUpdateVM()
        {
            Variant = new VariantUpdateDTO();
        }
        public VariantUpdateDTO Variant { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> CarList { get; set; }
       
    }
}

