using CarHub_API.Models;
using CarHub_API.Repository.IRepostiory;

namespace CarHub_API.Repository.IRepository
{
    public interface ICarXColorRepository : IRepository<CarXColor>
    {
        Task<CarXColor> UpdateAsync(CarXColor entity);
    }
}
