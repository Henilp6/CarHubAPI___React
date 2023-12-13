using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using CarHub_Web.Models.Dto;

namespace CarHub_Web.Models.VM
{
    public class FeatureVM
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Feature Name")]
        public string Name { get; set; }
        public int FeatureTypeId { get; set; }
        [ValidateNever]
        public FeatureTypeDTO FeatureType { get; set; }
        public string FeatureTypeName { get; set; }

        public bool IsChecked { get; set; }

    }
}
