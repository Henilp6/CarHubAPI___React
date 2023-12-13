using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace CarHub_API.Models.Dto
{
    public class BookingCreateDTO
    {
        public int Id { get; set; }

        [Required]
        [DisplayName("Customer Name")]
        public string CustomerName { get; set; }
        [Required]
        public string MobileNumber { get; set; }
        public string? Email { get; set; }
        public string Address { get; set; }
        //[ForeignKey("Car")]
        [Required]
        public int CarId { get; set; }
        //[ValidateNever]
        //public Car Car { get; set; }
        [Required]

        public int VariantId { get; set; }
        [Required]
        public int ColorId { get; set; }
        
        //[DisplayName("Booking Date")]
        //public DateTime BookingDate { get; set; } = DateTime.Now;
        
        public int DealerID { get; set; }
        
    }
}
