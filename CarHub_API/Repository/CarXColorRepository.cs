using CarHub_API.Data;
using CarHub_API.Models;
using CarHub_API.Repository.IRepository;

namespace CarHub_API.Repository
{
    public class CarXColorRepository : Repository<CarXColor>, ICarXColorRepository
    {
        private readonly ApplicationDbContext _db;
        public CarXColorRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }


        public async Task<CarXColor> UpdateAsync(CarXColor entity)
        {
            
            _db.CarXColors.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}
