using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace CarHub_Web.Models.Dto
{
    public class CarTypeUpdateDTO
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [DisplayName("Type Name")]
        public string TypeName { get; set; }
    }
}
