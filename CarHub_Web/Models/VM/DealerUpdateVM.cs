

using CarHub_Web.Models.Dto;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CarHub_Web.Models.VM
{
    public class DealerUpdateVM
    {
        public DealerUpdateVM()
        {
            Dealer = new DealerUpdateDTO();
        }
        public DealerUpdateDTO Dealer { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> BrandList { get; set; }

    }
}

