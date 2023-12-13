
using CarHub_Web.Models.Dto;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CarHub_Web.Models.VM
{
    public class DealerDeleteVM
    {
        public DealerDeleteVM()
       {
            Dealer = new DealerDTO();
       }
       public DealerDTO Dealer { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> BrandList { get; set; }
        

    }
}
