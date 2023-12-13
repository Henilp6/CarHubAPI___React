using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarHub_Web.Models.Dto
{
    public class MileageDTO
    {
        public int Id { get; set; }
        [Required]
        [ForeignKey("Car")]
        [Display(Name = "Car Name")]
        public int CarId { get; set; }
        public CarDTO Car { get; set; }
        public string FuelType { get; set; }

        public string Transmission { get; set; }

        [Display(Name = "Mileage")]
        public double Average { get; set; }
    }
}
