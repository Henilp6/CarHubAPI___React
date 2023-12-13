using CarHub_API.Data;
using CarHub_API.Models;
using CarHub_API.Repository.IRepository;

namespace CarHub_API.Repository
{
    public class ColorRepository: Repository<Color>, IColorRepository
    {
        private readonly ApplicationDbContext _db;
        public ColorRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }


        public async Task<Color> UpdateAsync(Color entity)
        {
            
            _db.Colors.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}
