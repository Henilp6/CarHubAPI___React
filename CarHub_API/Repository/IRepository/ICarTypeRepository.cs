using CarHub_API.Models;
using CarHub_API.Repository.IRepostiory;

namespace CarHub_API.Repository.IRepository
{
    public interface ICarTypeRepository : IRepository<CarType>
    {
        Task<CarType> UpdateAsync(CarType entity);
    }
}