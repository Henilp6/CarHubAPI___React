using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace CarHub_API.Models
{
 
    public class CarXFeature
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [ForeignKey("Car")]
        public int CarId { get; set; }
        [ValidateNever]
        public Car Car { get; set; }

        [Required]
        [ForeignKey("FeatureType")]
        public int FeatureTypeId { get; set; }
        [ValidateNever]
        public FeatureType FeatureType { get; set; }

        [Required]
        [ForeignKey("Feature")]
        public int FeatureId { get; set; }
        [ValidateNever]
        public Feature Feature { get; set; }

    }
}
