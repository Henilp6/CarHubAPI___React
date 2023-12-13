

using CarHub_API.Models;
using CarHub_API.Repository.IRepostiory;
using System.Linq.Expressions;

namespace CarHub_API.Repository.IRepository
{
    public interface ICityRepository : IRepository<City>
    {
      
        Task<City> UpdateAsync(City entity);
  
    }
}
