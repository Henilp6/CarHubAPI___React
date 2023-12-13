using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace CarHub_API.Models.Dto
{
    public class CarCreateDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Details { get; set; }
        public int BrandId { get; set; }
        
        public int CarTypeId { get; set; }
        
        [Required]
        [Display(Name = "Starting Price")]
        public double StartingPrice { get; set; }
        [Required]
        [Display(Name = "End Price")]
        public double EndPrice { get; set; }
        public int ManufacturingYear { get; set; }

        [DisplayName("Is Active")]
        public bool IsActive { get; set; }
        public string ImageURL { get; set; }
        //[ValidateNever]

        //public List<CarImage> CarImages { get; set; }

    }
}
