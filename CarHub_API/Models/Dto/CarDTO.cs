using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace CarHub_API.Models.Dto
{
    public class CarDTO
    {
        public int Id { get; set; }
       
        public string Name { get; set; }
        public string Details { get; set; }

        public int BrandId { get; set; }
        [ValidateNever]
        public BrandDTO Brand { get; set; }
        public int CarTypeId { get; set; }
        [ValidateNever]
        public CarTypeDTO CarType { get; set; }
     
        [Display(Name = "Starting Price")]
        public double StartingPrice { get; set; }
        
        [Display(Name = "End Price")]
        public double EndPrice { get; set; }
        public int ManufacturingYear { get; set; }

        [DisplayName("Is Active")]
        public bool IsActive { get; set; }
        public string ImageURL { get; set; }
        //[ValidateNever]

        //public List<CarImageDTO> CarImages { get; set; }
    }
}
