using CarHub_API.Data;
using CarHub_API.Models;
using CarHub_API.Repository.IRepository;

namespace CarHub_API.Repository
{
    public class CarImageRepository : Repository<CarImage>, ICarImageRepository
    {
        private readonly ApplicationDbContext _db;
        public CarImageRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }


        public async Task<CarImage> UpdateAsync(CarImage entity)
        {
            
            _db.CarImages.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}
