using CarHub_API.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;

namespace CarHub_API.Models
{
    public class Car
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }
        public string Details { get; set; }

        [ForeignKey("Brand")]
        public int BrandId { get; set; }
        [ValidateNever]
        public Brand Brand { get; set; }
        [ForeignKey("CarType")]
        public int CarTypeId { get; set; }
        [ValidateNever]
        public CarType CarType { get; set; }
        [Required]
        [Display(Name = "Starting Price")]
        [Range(1, 100000000, ErrorMessage = "Please enter a value between 1 and 100000000")]
        public double StartingPrice { get; set; }
        [Required]
        [Display(Name = "End Price")]
        [Range(1, 100000000, ErrorMessage = "Please enter a value between 1 and 100000000")]
        public double EndPrice { get; set; }
        [Range(1995, 2025, ErrorMessage = "Please enter a value between 1995 and 2025")]
        public int ManufacturingYear { get; set; }

        [DisplayName("Is Active")]
        public bool IsActive { get; set; }

        public string ImageURL { get; set; }


        //[ValidateNever]

        //public List<CarImage> CarImages { get; set; }




    }
}

