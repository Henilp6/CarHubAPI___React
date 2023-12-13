using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CarHub_Web.Models.Dto
{
    public class FeatureXFeaturetypeUpdateDTO
    {
        [Required]
        public int Id { get; set; }


        [Required]
        
        public int FeatureTypeId { get; set; }
        
        [Required]
        
        public int FeatureId { get; set; }
        
    }
}
