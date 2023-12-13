using CarHub_API.Models;
using CarHub_API.Repository.IRepostiory;

namespace CarHub_API.Repository.IRepository
{
    public interface IVariantRepository : IRepository<Variant>
    {
        Task<Variant> UpdateAsync(Variant entity);
    }
}
