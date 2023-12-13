
using CarHub_Web.Models;
using CarHub_Web.Models.VM;

namespace CarHub_Web.Service.IService
{
	public interface IApplicationUserService
    {
            Task<T> GetAllAsync<T>(string token);
            Task<T> GetAsync<T>(string Id, string token);
            Task<T> UpdateAsync<T>(UserVM dto, string token);
    }
}
