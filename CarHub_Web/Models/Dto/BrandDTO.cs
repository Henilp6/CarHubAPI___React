using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace CarHub_Web.Models.Dto
{
    public class BrandDTO
    { 
        public int Id { get; set; }
        [Required]
        [DisplayName("Brand Name")]
        [StringLength(50, MinimumLength = 2)]
        public string BrandName { get; set; }

        [DisplayName("Brand Image")]
        public string BrandImage { get; set; }

        [DisplayName("Is Delete")]
        public bool IsDelete { get; set; }

        [DisplayName("Is Active")]
        public bool IsActive { get; set; }
    }
}
