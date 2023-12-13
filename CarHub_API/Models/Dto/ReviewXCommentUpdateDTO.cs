using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CarHub_API.Models.Dto
{
    public class ReviewXCommentUpdateDTO
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string ApplicationUserId { get; set; }

        [Required]
        
        public int ReviewId { get; set; }
       

        public string Comment { get; set; }
    }
}
