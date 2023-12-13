using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarHub_Web.Models.Dto
{
    public class CarXFeatureCreateDTO
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Car Name")]
        public int CarId { get; set; }
     
        [Required]
        [Display(Name = "FeatureType Name")]
        public int FeatureTypeId { get; set; }
        
        [Required]
        [Display(Name = "Feature Name")]
        public int FeatureId { get; set; }
        
    }
}
