


using CarHub_API.Data;
using CarHub_API.Models;
using CarHub_API.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CarHub_API.Repository
{
    public class StateRepository : Repository<State>, IStateRepository
    {
        private readonly ApplicationDbContext _db;
        public StateRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

  
        public async Task<State> UpdateAsync(State entity)
        {
         
            _db.States.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}
