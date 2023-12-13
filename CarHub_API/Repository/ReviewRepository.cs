using CarHub_API.Data;
using CarHub_API.Models;
using CarHub_API.Repository.IRepository;

namespace CarHub_API.Repository
{
    public class ReviewRepository : Repository<Review>, IReviewRepository
    {
        private readonly ApplicationDbContext _db;
        public ReviewRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }


        public async Task<Review> UpdateAsync(Review entity)
        {
            
            _db.Reviews.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}
