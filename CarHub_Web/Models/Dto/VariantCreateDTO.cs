using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarHub_Web.Models.Dto
{
    public class VariantCreateDTO
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Car Name")]
        public int CarId { get; set; }
        
        [Display(Name = "Variant Name")]
        [Required]
        public string VariantName { get; set; }
        public string Transmission { get; set; }
        public double Price { get; set; }
    }
}
