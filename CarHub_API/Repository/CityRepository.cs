


using CarHub_API.Data;
using CarHub_API.Models;
using CarHub_API.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CarHub_API.Repository
{
    public class CityRepository : Repository<City>, ICityRepository
    {
        private readonly ApplicationDbContext _db;
        public CityRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

  
        public async Task<City> UpdateAsync(City entity)
        {
         
            _db.Cities.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}
