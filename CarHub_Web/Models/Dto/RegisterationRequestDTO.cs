////namespace CarHub_Web.Models.Dto
////{
////	public class RegisterationRequestDTO
////    {
////        public string Password { get; set; }
////        public string? City { get; set; }
////        public string? State { get; set; }
////        public string? PostalCode { get; set; }
////        public string PhoneNumber { get; set; }
////        public string FirstName { get; set; }
////        public string? MiddleName { get; set; }
////        public string? LastName { get; set; }
////        public string? Address { get; set; }
////        public string? Country { get; set; }
////        public string Email { get; set; }
////    }
////}
using System.ComponentModel.DataAnnotations;

namespace CarHub_Web.Models.Dto
{
    public class RegisterationRequestDTO
    {
        [Required]
        [RegularExpression(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$", ErrorMessage = "Invalid Password Enter 8 character password like 'Example@123' ")]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage ="Password Does Not Match.")]
        public string ConfirmPassword { get; set; }
        public string? City { get; set; }

        public string? State { get; set; }
        public string? PostalCode { get; set; }
        [Required]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Invalid Phone number")]
        public string PhoneNumber { get; set; }
        [Required]
        [StringLength(60, MinimumLength = 3)]
        public string FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public string? Address { get; set; }
        public string? Country { get; set; }
        [Required]
        [StringLength(50)]
        [RegularExpression(@"[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}", ErrorMessage = "Invalid Email Address Enter like 'mailto:example123@gmail.com' ")]
        public string Email { get; set; }
    }
}

