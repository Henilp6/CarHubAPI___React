
using CarHub_Web.Models.Dto;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CarHub_Web.Models.VM
{
    public class  DealerCreateVM
    {
        public DealerCreateVM()
        {
            Dealer = new DealerCreateDTO();
        }
        public DealerCreateDTO Dealer { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> BrandList { get; set; }
        
    }
}
