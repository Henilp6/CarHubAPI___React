using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace CarHub_Web.Models.Dto
{
    public class CountryCreateDTO
    {

        public int Id { get; set; }

        [Required]
        [DisplayName("Country Name")]
        public string CountryName { get; set; }
		[DisplayName("Is Active")]
		public bool IsActive { get; set; }


    }
}
