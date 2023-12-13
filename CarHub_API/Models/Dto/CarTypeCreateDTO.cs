using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace CarHub_API.Models.Dto
{
    public class CarTypeCreateDTO
    {
        public int Id { get; set; }

        [Required]
        [DisplayName("Type Name")]
        public string TypeName { get; set; }
    }
}
