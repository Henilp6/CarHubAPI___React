using CarHub_Web.Models;
using CarHub_Web.Models.Dto;

namespace CarHub_Web.Service.IService
{
	public interface IAuthService
    {
        Task<T> LoginAsync<T>(LoginRequestDTO objToCreate);
        Task<T> RegisterAsync<T>(RegisterationRequestDTO objToCreate);
    }
}
