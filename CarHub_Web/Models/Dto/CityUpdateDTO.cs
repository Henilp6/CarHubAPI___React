using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace CarHub_Web.Models.Dto
{
    public class CityUpdateDTO
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [DisplayName("City Name")]
        public string CityName { get; set; }
        [DisplayName("Country Name")]
        public int CountryId { get; set; }
        [DisplayName("State Name")]
        public int StateId { get; set; }
        public bool IsActive { get; set; }


    }
}
