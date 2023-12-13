using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace CarHub_API.Models
{
 
    public class FeatureXFeaturetype
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
       

        [Required]
        [ForeignKey("FeatureType")]
        public int FeatureTypeId { get; set; }
        public FeatureType FeatureType { get; set; }
        [Required]
        [ForeignKey("Feature")]
        public int FeatureId { get; set; }
        public Feature Feature { get; set; }
    }
}
