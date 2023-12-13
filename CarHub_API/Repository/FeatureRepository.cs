using CarHub_API.Data;
using CarHub_API.Models;
using CarHub_API.Repository.IRepository;

namespace CarHub_API.Repository
{
    public class FeatureRepository : Repository<Feature>, IFeatureRepository
    {
        private readonly ApplicationDbContext _db;
        public FeatureRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }


        public async Task<Feature> UpdateAsync(Feature entity)
        {
            
            _db.Features.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}
