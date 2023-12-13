namespace CarHub_API.Repository.IRepository
{
    public interface IUnitOfWork
    {
        //ICategoryRepository Category { get; }
        IBookingRepository Booking { get; }
        IBrandRepository Brand { get; }
        ICarImageRepository CarImage { get; }
        ICarRepository Car { get; }
        ICarTypeRepository CarType { get; }
        ICarXColorRepository CarXColor { get; }
        IColorRepository Color { get; }
        ICarSpecificationRepository CarSpecification { get; }
        ICarXFeatureRepository CarXFeature { get; }
        IFeatureRepository Feature { get; }
		IFeatureTypeRepository FeatureType { get; }
		IFeatureXFeaturetypeRepository FeatureXFeaturetype { get; }
        IMileageRepository Mileage { get; }
        IReviewRepository Review { get; }
        IReviewXCommentRepository ReviewXComment { get; }
        IVariantRepository Variant { get; }
        IDealerRepository Dealer { get; }
		ICityRepository City { get; }
		ICountryRepository Country { get; }

        IApplicationUserRepository ApplicationUser { get; }
        IApplicationRoleRepository ApplicationRole { get; }
        IApplicationUserRoleRepository ApplicationUserRole { get; }
        IUserRepository User { get; }

        IStateRepository State { get; }
		void Save();
    }
}
