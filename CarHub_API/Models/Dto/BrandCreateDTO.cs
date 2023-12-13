using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace CarHub_API.Models.Dto
{
    public class BrandCreateDTO
    {


        public int Id { get; set; }
        [Required]
        [DisplayName("Brand Name")]
        public string BrandName { get; set; }

        [DisplayName("Brand Image")]
        public string BrandImage { get; set; }

        [DisplayName("Is Delete")]
        public bool IsDelete { get; set; }

        [DisplayName("Is Active")]
        public bool IsActive { get; set; }
    }
}
