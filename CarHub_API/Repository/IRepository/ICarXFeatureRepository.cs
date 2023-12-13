using CarHub_API.Models;
using CarHub_API.Repository.IRepostiory;

namespace CarHub_API.Repository.IRepository
{
    public interface ICarXFeatureRepository : IRepository<CarXFeature>
    {
        Task<CarXFeature> UpdateAsync(CarXFeature entity);
    }
}
