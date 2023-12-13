
using CarHub_Web.Models.Dto;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CarHub_Web.Models.VM
{
    public class VariantDeleteVM
    {
        public VariantDeleteVM()
       {
            Variant = new VariantDTO();
       }
       public VariantDTO Variant { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> CarList { get; set; }
        

    }
}
