using CarHub_API.Models;
using CarHub_API.Repository.IRepostiory;

namespace CarHub_API.Repository.IRepository
{
    public interface IReviewXCommentRepository : IRepository<ReviewXComment>
    {
        Task<ReviewXComment> UpdateAsync(ReviewXComment entity);
    }
}
