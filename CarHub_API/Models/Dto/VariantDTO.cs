using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace CarHub_API.Models.Dto
{
    public class VariantDTO
    {
        public int Id { get; set; }
        [Required]
        public int CarId { get; set; }
        [ValidateNever]
        public CarDTO  Car { get; set; }
        [Display(Name = "Variant Name")]
        public string VariantName { get; set; }
        public string Transmission { get; set; }
        public double Price { get; set; }
    }
}
