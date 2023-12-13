namespace CarHub_Web.Models.Dto
{
	public class LoginResponseDTO
    {
        public ApplicationUserDTO User { get; set; }
        public string Token { get; set; }
    }
}
