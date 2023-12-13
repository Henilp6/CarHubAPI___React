using CarHub_API.Data;
using CarHub_API.Models;
using CarHub_API.Repository.IRepository;

namespace CarHub_API.Repository
{
    public class DealerRepository : Repository<Dealer>, IDealerRepository
    {
        private readonly ApplicationDbContext _db;
        public DealerRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }


        public async Task<Dealer> UpdateAsync(Dealer entity)
        {
            
            _db.Dealers.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}
