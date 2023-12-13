using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CarHub_API.Models.Dto
{
    public class VariantUpdateDTO
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int CarId { get; set; }
        [Display(Name = "Variant Name")]
        public string VariantName { get; set; }
        public string Transmission { get; set; }
        public double Price { get; set; }
    }
}
