
using CarHub_API.Models;
using CarHub_API.Repository.IRepostiory;

namespace CarHub_API.Repository.IRepository
{
	public interface IApplicationUserRoleRepository : IRepository<ApplicationUserRole>
    {
        Task<ApplicationUserRole> UpdateAsync(ApplicationUserRole entity);
    }
}