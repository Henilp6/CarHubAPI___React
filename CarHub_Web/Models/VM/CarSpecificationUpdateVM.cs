

using CarHub_Web.Models.Dto;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CarHub_Web.Models.VM
{
    public class CarSpecificationUpdateVM
    {
        public CarSpecificationUpdateVM()
        {
            CarSpecification = new CarSpecificationUpdateDTO();
        }
        public CarSpecificationUpdateDTO CarSpecification { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> CarList { get; set; }

    }
    
}

