using CarHub_API.Models;
using CarHub_API.Repository.IRepostiory;

namespace CarHub_API.Repository.IRepository
{
    public interface IColorRepository : IRepository<Color>
    {
        Task<Color> UpdateAsync(Color entity);
    }
}
