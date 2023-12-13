using CarHub_API.Data;
using CarHub_API.Models;
using CarHub_API.Repository.IRepository;
using CarHub_API.Repository;

public class MileageRepository : Repository<Mileage>, IMileageRepository
{
    private readonly ApplicationDbContext _db;
    public MileageRepository(ApplicationDbContext db) : base(db)
    {
        _db = db;
    }


    public async Task<Mileage> UpdateAsync(Mileage entity)
    {

        _db.Mileages.Update(entity);
        await _db.SaveChangesAsync();
        return entity;
    }
}
