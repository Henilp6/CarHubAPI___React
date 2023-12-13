using CarHub_API.Data;
using CarHub_API.Models;
using CarHub_API.Repository.IRepository;

namespace CarHub_API.Repository
{
    public class VariantRepository : Repository<Variant>, IVariantRepository
    {
        private readonly ApplicationDbContext _db;
        public VariantRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }


        public async Task<Variant> UpdateAsync(Variant entity)
        {
            
            _db.Variants.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}
