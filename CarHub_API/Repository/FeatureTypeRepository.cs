using CarHub_API.Data;
using CarHub_API.Models;
using CarHub_API.Repository.IRepository;

namespace CarHub_API.Repository
{
    public class FeatureTypeRepository : Repository<FeatureType>, IFeatureTypeRepository
    {
        private readonly ApplicationDbContext _db;
        public FeatureTypeRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }


        public async Task<FeatureType> UpdateAsync(FeatureType entity)
        {
            
            _db.FeatureTypes.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}
