using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace CarHub_API.Models.Dto
{
    public class CarXColorCreateDTO
    {
        public int Id { get; set; }
        
        public int CarId { get; set; }
        
     
        public int ColorId { get; set; }
     
    }
}
