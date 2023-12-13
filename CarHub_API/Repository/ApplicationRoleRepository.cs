using CarHub_API.Data;
using CarHub_API.Models;
using CarHub_API.Repository.IRepository;

namespace CarHub_API.Repository
{
	public class ApplicationRoleRepository : Repository<ApplicationRole>, IApplicationRoleRepository
	{
        private readonly ApplicationDbContext _db;
        public ApplicationRoleRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<ApplicationRole> UpdateAsync(ApplicationRole entity)
        {
            _db.ApplicationRoles.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}
