using CarHub_API.Models;
using CarHub_API.Models.Dto;
using CarHub_API.Repository.IRepostiory;

namespace CarHub_API.Repository.IRepository
{
	public interface IUserRepository
    {
        bool IsUniqueUser(string username);
        Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO);
        Task<ApplicationUserDTO> Register(RegisterationRequestDTO registerationRequestDTO);
    }
}
