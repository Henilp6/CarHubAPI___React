using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace CarHub_API.Models.Dto
{
    public class BookingDTO
    {
      
        public int Id { get; set; }
        [Required]
        [DisplayName("Customer Name")]
        public string CustomerName { get; set; }
        [Required]
        public string MobileNumber { get; set; }
        public string? Email { get; set; }
        public string Address { get; set; }
       
        public int CarId { get; set; }
        [ValidateNever]
        public CarDTO Car { get; set; }
        
        public int VariantId { get; set; }
        [ValidateNever]
        public VariantDTO Variant { get; set; }
        //[ForeignKey("Color")]
        public int ColorId { get; set; }
        [ValidateNever]
        public ColorDTO Color { get; set; }
        //[DisplayName("Booking Date")]
        //public DateTime BookingDate { get; set; } = DateTime.Now;
        
        public int DealerID { get; set; }
        [ValidateNever]
        public DealerDTO Dealer { get; set; }
    }
}
