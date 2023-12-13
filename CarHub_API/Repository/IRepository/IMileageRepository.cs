using CarHub_API.Models;
using CarHub_API.Repository.IRepostiory;

namespace CarHub_API.Repository.IRepository
{
    public interface IMileageRepository : IRepository<Mileage>
    {
        Task<Mileage> UpdateAsync(Mileage entity);
    }
}
