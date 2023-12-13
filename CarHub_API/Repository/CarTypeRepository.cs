using CarHub_API.Data;
using CarHub_API.Models;
using CarHub_API.Repository.IRepository;

namespace CarHub_API.Repository
{
    public class CarTypeRepository : Repository<CarType>, ICarTypeRepository
    {
        private readonly ApplicationDbContext _db;
        public CarTypeRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }


        public async Task<CarType> UpdateAsync(CarType entity)
        {

            _db.CarTypes.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}
