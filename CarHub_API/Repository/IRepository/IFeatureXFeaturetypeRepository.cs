using CarHub_API.Models;
using CarHub_API.Repository.IRepostiory;

namespace CarHub_API.Repository.IRepository
{
    public interface IFeatureXFeaturetypeRepository : IRepository<FeatureXFeaturetype>
    {
        Task<FeatureXFeaturetype> UpdateAsync(FeatureXFeaturetype entity);
    }
}
