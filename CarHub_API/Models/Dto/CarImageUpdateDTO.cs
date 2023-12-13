using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace CarHub_API.Models.Dto
{
    public class CarImageUpdateDTO
    {
        [Required]
        public int Id { get; set; }
       
       public string ImageUrl { get; set; }
        public int CarId { get; set; }
        
    }
}
