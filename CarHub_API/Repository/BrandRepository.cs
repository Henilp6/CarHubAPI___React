using CarHub_API.Data;
using CarHub_API.Models;
using CarHub_API.Repository.IRepository;

namespace CarHub_API.Repository
{
    public class BrandRepository: Repository<Brand>, IBrandRepository
    {
        private readonly ApplicationDbContext _db;
        public BrandRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }


        public async Task<Brand> UpdateAsync(Brand entity)
        {
            
            _db.Brands.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}
