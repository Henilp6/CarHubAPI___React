using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace CarHub_API.Models.Dto
{
    public class StateCreateDTO
    {
        public int Id { get; set; }

        [Required]
        [DisplayName("State Name")]
        public string StateName { get; set; }
        public int CountryId { get; set; }
        public bool IsActive { get; set; }
    }
}
