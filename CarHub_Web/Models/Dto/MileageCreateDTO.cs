using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace CarHub_Web.Models.Dto
{
    public class MileageCreateDTO
    {
        public int Id { get; set; }
        [Required]
		[Display(Name = "Car Name")]
		public int CarId { get; set; }

        public string FuelType { get; set; }

        public string Transmission { get; set; }

        [Display(Name = "Mileage")]
        public double Average { get; set; }
    }
}
