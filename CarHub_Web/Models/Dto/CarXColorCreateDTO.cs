using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace CarHub_Web.Models.Dto
{
    public class CarXColorCreateDTO
    {
        public int Id { get; set; }

        [Display(Name = "Car Name")]
        public int CarId { get; set; }

        [Display(Name = "Color Name")]
        public int ColorId { get; set; }
     
    }
}
