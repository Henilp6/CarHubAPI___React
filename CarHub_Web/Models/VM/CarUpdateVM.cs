using CarHub_Web.Models.Dto;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CarHub_Web.Models.VM
{
    public class CarUpdateVM
    {
        public CarUpdateVM()
        {
            Car = new CarUpdateDTO();
        }
        public CarUpdateDTO Car { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> BrandList { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> CarTypeList { get; set; }
    }
}
