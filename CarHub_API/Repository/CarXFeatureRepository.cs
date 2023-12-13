using CarHub_API.Data;
using CarHub_API.Models;
using CarHub_API.Repository.IRepository;

namespace CarHub_API.Repository
{
    public class CarXFeatureRepository : Repository<CarXFeature>, ICarXFeatureRepository
    {
        private readonly ApplicationDbContext _db;
        public CarXFeatureRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }


        public async Task<CarXFeature> UpdateAsync(CarXFeature entity)
        {
            
            _db.CarXFeatures.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}
