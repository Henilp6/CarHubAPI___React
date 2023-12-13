using System.ComponentModel.DataAnnotations;

//namespace CarHub_Web.Models.Dto
//{
//	public class LoginRequestDTO
//    {
//        public string UserName { get; set; }
//        public string Password { get; set; }
//    }
//}

namespace CarHub_Web.Models.Dto
{
    public class LoginRequestDTO
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}

