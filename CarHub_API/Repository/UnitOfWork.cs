using CarHub_API.Data;
using CarHub_API.Models;
using CarHub_API.Repository.IRepository;

namespace CarHub_API.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _db;
        public IApplicationUserRepository ApplicationUser { get; private set; }
        public IApplicationRoleRepository ApplicationRole { get; private set; }
        public IApplicationUserRoleRepository ApplicationUserRole { get; private set; }
        public IUserRepository User { get; private set; }
        public IBookingRepository Booking { get; private set; }
        public IBrandRepository Brand { get; private set; }
        public ICarImageRepository CarImage { get; private set; }
        public ICarRepository Car { get; private set; }
        public ICarTypeRepository CarType { get; private set; }
        public ICarXColorRepository CarXColor { get; private set; }
        public IColorRepository Color { get; private set; }
        public IFeatureRepository Feature { get; private set; }
        public IFeatureTypeRepository FeatureType { get; private set; }
        public IFeatureXFeaturetypeRepository FeatureXFeaturetype { get; private set; }
        public IMileageRepository Mileage { get; private set; }
        public IReviewRepository Review { get; private set; }
        public IVariantRepository Variant { get; private set; }
        public IReviewXCommentRepository ReviewXComment { get; private set; }
        public ICarXFeatureRepository CarXFeature { get; private set; }
        public ICarSpecificationRepository  CarSpecification { get; private set; }
        public IDealerRepository Dealer { get; private set; }
		public ICountryRepository Country { get; private set; }
		public ICityRepository City { get; private set; }

		public IStateRepository State { get; private set; }

		public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;

            //Category = new CategoryRepository(_db);
            Booking = new BookingRepository(_db);
            Brand = new BrandRepository(_db);
            CarImage = new CarImageRepository(_db);
            CarType = new CarTypeRepository(_db);
            Color = new ColorRepository(_db);
            Car = new CarRepository(_db);
            CarXColor = new CarXColorRepository(_db);
            Feature = new FeatureRepository(_db);
            FeatureType = new FeatureTypeRepository(_db);
           FeatureXFeaturetype= new FeatureXFeaturetypeRepository(_db);
            Mileage = new MileageRepository(_db);
            Review = new ReviewRepository(_db);
            Variant = new VariantRepository(_db);
            ReviewXComment = new ReviewXCommentRepository(_db);
            CarXFeature = new CarXFeatureRepository(_db);
            CarSpecification = new CarSpecificationRepository(_db);
            Dealer = new DealerRepository(_db);
			City = new CityRepository(_db);
			State = new StateRepository(_db);
			Country = new CountryRepository(_db);
            ApplicationUser = new ApplicationUserRepository(_db);
            ApplicationRole = new ApplicationRoleRepository(_db);
            ApplicationUserRole = new ApplicationUserRoleRepository(_db);

        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
