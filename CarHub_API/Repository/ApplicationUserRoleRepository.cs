using CarHub_API.Data;
using CarHub_API.Models;
using CarHub_API.Repository.IRepository;

namespace CarHub_API.Repository
{
	public class ApplicationUserRoleRepository : Repository<ApplicationUserRole>, IApplicationUserRoleRepository
    {
        private readonly ApplicationDbContext _db;
        public ApplicationUserRoleRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<ApplicationUserRole> UpdateAsync(ApplicationUserRole entity)
        {
           // _db.ApplicationUserRoles.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}
