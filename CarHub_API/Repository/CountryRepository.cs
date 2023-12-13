


using CarHub_API.Data;
using CarHub_API.Models;
using CarHub_API.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CarHub_API.Repository
{
    public class CountryRepository : Repository<Country>, ICountryRepository
    {
        private readonly ApplicationDbContext _db;
        public CountryRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

  
        public async Task<Country> UpdateAsync(Country entity)
        {
         
            _db.Countries.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}
