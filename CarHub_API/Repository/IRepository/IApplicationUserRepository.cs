
using CarHub_API.Models;
using CarHub_API.Repository.IRepostiory;

namespace CarHub_API.Repository.IRepository
{
	public interface IApplicationUserRepository : IRepository<ApplicationUser>
    {
        Task<ApplicationUser> UpdateAsync(ApplicationUser entity);
    }
}