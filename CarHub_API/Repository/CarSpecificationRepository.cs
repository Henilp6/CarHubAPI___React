using CarHub_API.Data;
using CarHub_API.Models;
using CarHub_API.Repository.IRepository;

namespace CarHub_API.Repository
{
    public class CarSpecificationRepository : Repository<CarSpecification>, ICarSpecificationRepository
    {
        private readonly ApplicationDbContext _db;
        public CarSpecificationRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }


        public async Task<CarSpecification> UpdateAsync(CarSpecification entity)
        {
            
            _db.CarSpecifications.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}
