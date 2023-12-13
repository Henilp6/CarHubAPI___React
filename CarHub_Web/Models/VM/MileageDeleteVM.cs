using CarHub_Web.Models.Dto;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CarHub_Web.Models.VM
{
    public class MileageDeleteVM
    {
  
            public MileageDeleteVM()
            {
                Mileage = new MileageDTO();
			  Car = new CarCreateDTO();
		}
            public MileageDTO Mileage { get; set; }
		    [ValidateNever]
		   public CarCreateDTO Car { get; set; }
		  [ValidateNever]
            public IEnumerable<SelectListItem> CarList { get; set; }
       


    }
}
