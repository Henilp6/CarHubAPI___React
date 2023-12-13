using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace CarHub_API.Models
{
 
    public class Review
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }


        [ForeignKey("ApplicationUser")]
        public string ApplicationUserId { get; set; }
        [ValidateNever]
        public ApplicationUser ApplicationUser { get; set; }
        [Required]
        [ForeignKey("Car")]
        public int CarId { get; set; }
        public Car Car { get; set; }
        [Display(Name = "overall Rating")]
        public string overallRating { get; set; }
        public string Descriptaion { get; set; }
        [Display(Name = "Your Review title ")]
        public string Title { get; set; }
        [Display(Name = "Like Count")]
        public int LikeCount { get; set; }
        [Display(Name = "DisLike Count")]
        public int DisLikeCount { get; set; }
        [Display(Name = "View Count")]
        public int ViewCount { get; set; }
        public DateTime Createddate { get; set; } = DateTime.Now;


    }
}
