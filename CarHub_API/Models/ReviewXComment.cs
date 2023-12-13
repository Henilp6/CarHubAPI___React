using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace CarHub_API.Models
{
 
    public class ReviewXComment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("ApplicationUser")]
        public string ApplicationUserId { get; set; }
        [ValidateNever]
        public ApplicationUser ApplicationUser { get; set; }
        [Required]
        [ForeignKey("Review")]
        public int ReviewId { get; set; }
        public Review Review { get; set; }
       
        public string Comment { get; set; }
        public DateTime Createddate { get; set; } = DateTime.Now;
        
    }
}
