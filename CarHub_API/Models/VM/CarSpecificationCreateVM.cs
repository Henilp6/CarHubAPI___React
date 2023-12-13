
using CarHub_API.Models.Dto;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CarHub_API.Models.VM
{
    public class CarSpecificationCreateVM
    {
        public CarSpecificationCreateVM()
        {
            CarSpecification = new CarSpecificationCreateDTO();
            Car = new CarCreateDTO();
        }
        [ValidateNever]
        public CarCreateDTO Car { get; set; }
        public CarSpecificationCreateDTO CarSpecification { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> CarList { get; set; }
        
    }
}
