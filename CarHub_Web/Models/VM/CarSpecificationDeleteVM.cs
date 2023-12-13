
using CarHub_Web.Models.Dto;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CarHub_Web.Models.VM
{
    public class CarSpecificationDeleteVM
    {
        public CarSpecificationDeleteVM()
       {
         CarSpecification = new CarSpecificationDTO();
       }

       public CarSpecificationDTO CarSpecification { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> CarList { get; set; }
        

    }
}
