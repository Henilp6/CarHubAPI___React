using CarHub_API.Models;
using CarHub_API.Repository.IRepostiory;

namespace CarHub_API.Repository.IRepository
{
    public interface IBrandRepository: IRepository<Brand>
    {
        Task<Brand> UpdateAsync(Brand entity);
    }
}
