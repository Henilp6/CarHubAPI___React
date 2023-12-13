using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarHub_API.Models.Dto
{
    public class VariantCreateDTO
    {
        public int Id { get; set; }
        [Required]
        public int CarId { get; set; }
        
        [Display(Name = "Variant Name")]
        public string VariantName { get; set; }
        public string Transmission { get; set; }
        public double Price { get; set; }
    }
}
