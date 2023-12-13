using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarHub_API.Models.Dto
{
    public class ReviewXCommentCreateDTO
    {
        public int Id { get; set; }


        [Required]
        public string ApplicationUserId { get; set; }

        [Required]
        public int ReviewId { get; set; }
        public string Comment { get; set; }
    }
}
