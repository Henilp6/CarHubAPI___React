using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace CarHub_API.Models.Dto
{
    public class CarXColorDTO
    {
        public int Id { get; set; }
        
        public int CarId { get; set; }
        [ValidateNever]
        public CarDTO Car { get; set; }
        
        public int ColorId { get; set; }
        [ValidateNever]
        public ColorDTO Color { get; set; }
    }
}
