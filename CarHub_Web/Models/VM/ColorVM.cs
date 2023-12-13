using CarHub_Web.Models.Dto;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace CarHub_Web.Models.VM
{
    public class ColorVM
    {
        public int Id { get; set; }
        [Required]
        [DisplayName("Color Name")]
        public string ColorName { get; set; }
       
        public bool IsChecked { get; set; }

    }
}
