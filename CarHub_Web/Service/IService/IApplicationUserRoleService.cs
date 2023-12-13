
using CarHub_Web.Models;

namespace CarHub_Web.Service.IService
{
    public interface IApplicationUserRoleService
    {

        Task<T> GetAllAsync<T>(string token);
        Task<T> GetAsync<T>(string Id, string token);


    }
}
