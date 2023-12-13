using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarHub_Web.Models.Dto
{
    public class FeatureTypeDTO
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "FeatureType Name")]
        public string FeatureTypeName { get; set; }
    }
}
