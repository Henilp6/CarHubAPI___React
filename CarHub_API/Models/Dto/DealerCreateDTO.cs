using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace CarHub_API.Models.Dto
{
    public class DealerCreateDTO
    {
        public int Id { get; set; }
        [Required]
        [DisplayName(" Dealer Name")]
        public string DealerName { get; set; }
        [Required]
        public string MobileNumber { get; set; }
        public string? Email { get; set; }
        public string DealerLocation { get; set; }
        [Required]
        public int BrandId { get; set; }
        
        [DisplayName("Is Available")]
        public bool IsAvailable { get; set; }
    }
}
