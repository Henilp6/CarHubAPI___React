using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace CarHub_API.Models.Dto
{
    public class ReviewXCommentDTO
    {
        public int Id { get; set; }

        [Required]
        public string ApplicationUserId { get; set; }
        [ValidateNever]
        public ApplicationUserDTO ApplicationUser { get; set; }
        [Required]
        
        public int ReviewId { get; set; }
        [ValidateNever]
        public ReviewDTO Review { get; set; }

        public string Comment { get; set; }
    }
}
