using CarHub_API.Models;
using CarHub_API.Repository.IRepostiory;

namespace CarHub_API.Repository.IRepository
{
    public interface IDealerRepository : IRepository<Dealer>
    {
        Task<Dealer> UpdateAsync(Dealer entity);
    }
}
