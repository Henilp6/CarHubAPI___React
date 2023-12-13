
using CarHub_API.Models;
using CarHub_API.Repository.IRepostiory;

namespace CarHub_API.Repository.IRepository
{
	public interface IApplicationRoleRepository : IRepository<ApplicationRole>
    {
        Task<ApplicationRole> UpdateAsync(ApplicationRole entity);
    }
}

