using CarHub_API.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace CarHub_API.Models
{
    public class Booking
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [DisplayName("Customer Name")]
        public string CustomerName { get; set; }
        [Required]
        public string MobileNumber { get; set; }
        public string? Email { get; set; }
        public string Address { get; set; }
        [ForeignKey("Car")]
	
		public int CarId { get; set; }
        [ValidateNever]
        public Car Car { get; set; }
        [ForeignKey("Variant")]
        public int VariantId { get; set; }
        [ValidateNever]
        public Variant Variant { get; set; }
        [ForeignKey("Color")]
        public int ColorId { get; set; }
        [ValidateNever]
        public Color Color { get; set; }
        [DisplayName("Booking Date")]
        public DateTime BookingDate { get; set; }= DateTime.Now;
        [ForeignKey("Dealer")]
        public int DealerID { get; set; }
        [ValidateNever]
        public Dealer Dealer { get; set; }


    }
}
//Booking: -
//-Id
//- CustomerName
//- MobileNumber
//- Email
//- Address
//- CarId
//- VariantId
//- Color
//- BookingDate
//- dealerID
