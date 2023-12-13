using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace CarHub_API.Models.Dto
{
    public class CarXColorReact
    {
        public int CarId { get; set; }
        public List<String> SelectedColorIds { get; set; }
    }
}
