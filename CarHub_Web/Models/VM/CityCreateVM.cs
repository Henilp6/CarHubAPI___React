
using CarHub_Web.Models.Dto;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CarHub_Web.Models.VM
{
    public class CityCreateVM
    {
        public CityCreateVM()
        {
            City = new CityCreateDTO();
        }
        public CityCreateDTO City { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> CountryList { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> StateList { get; set; }
    }
}
