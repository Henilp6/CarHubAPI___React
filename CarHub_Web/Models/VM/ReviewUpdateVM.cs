using CarHub_Web.Models.Dto;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CarHub_Web.Models.VM
{
    public class ReviewUpdateVM
    {
        public ReviewUpdateVM()
        {
            Review = new ReviewUpdateDTO();
        }
        public ReviewUpdateDTO Review { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> CarList { get; set; }
        
    }
}
