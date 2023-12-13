
using CarHub_API.Models.Dto;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CarHub_API.Models.VM
{
    public class MileageCreateVM
    {
        public MileageCreateVM()
        {
            Mileage = new MileageCreateDTO();
			Car = new CarCreateDTO();
		}
        public MileageCreateDTO Mileage { get; set; }
		[ValidateNever]
		public CarCreateDTO Car { get; set; }
		[ValidateNever]
        public IEnumerable<SelectListItem> CarList { get; set; }
       
    }
}
