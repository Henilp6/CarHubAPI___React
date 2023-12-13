using CarHub_API.Data;
using CarHub_API.Models;
using CarHub_API.Repository.IRepository;

namespace CarHub_API.Repository
{
    public class ReviewXCommentRepository : Repository<ReviewXComment>, IReviewXCommentRepository
    {
        private readonly ApplicationDbContext _db;
        public ReviewXCommentRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }


        public async Task<ReviewXComment> UpdateAsync(ReviewXComment entity)
        {
            
            _db.ReviewXComments.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}
