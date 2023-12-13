using CarHub_API.Models;
using CarHub_API.Repository.IRepostiory;

namespace CarHub_API.Repository.IRepository
{
    public interface ICarRepository : IRepository<Car>
    {
        Task<Car> UpdateAsync(Car entity);
    }
}
