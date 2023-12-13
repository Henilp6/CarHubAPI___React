using CarHub_API.Data;
using CarHub_API.Models;
using CarHub_API.Repository.IRepository;

namespace CarHub_API.Repository
{
    public class FeatureXFeaturetypeRepository : Repository<FeatureXFeaturetype>, IFeatureXFeaturetypeRepository
    {
        private readonly ApplicationDbContext _db;
        public FeatureXFeaturetypeRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }


        public async Task<FeatureXFeaturetype> UpdateAsync(FeatureXFeaturetype entity)
        {
            
            _db.FeatureXFeaturetypes.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}
