using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace CarHub_Web.Models.Dto
{
    public class ColorDTO
    {

        public int Id { get; set; }
        [Required]
        [DisplayName("Color Name")]
        public string ColorName { get; set; }
    }
}
