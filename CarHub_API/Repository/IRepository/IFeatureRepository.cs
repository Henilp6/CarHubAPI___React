using CarHub_API.Models;
using CarHub_API.Repository.IRepostiory;

namespace CarHub_API.Repository.IRepository
{
    public interface IFeatureRepository : IRepository<Feature>
    {
        Task<Feature> UpdateAsync(Feature entity);
    }
}
