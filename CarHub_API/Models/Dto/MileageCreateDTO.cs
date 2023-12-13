using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace CarHub_API.Models.Dto
{
    public class MileageCreateDTO
    {
        public int Id { get; set; }
        [Required]
        public int CarId { get; set; }
        
        public string FuelType { get; set; }

        public string Transmission { get; set; }

        [Display(Name = "Mileage")]
        public double Average { get; set; }
    }
}
