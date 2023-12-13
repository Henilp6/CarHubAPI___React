using CarHub_API.Models;
using CarHub_API.Repository.IRepostiory;

namespace CarHub_API.Repository.IRepository
{
    public interface ICarImageRepository : IRepository<CarImage>
    {
        Task<CarImage> UpdateAsync(CarImage entity);
    }
}
