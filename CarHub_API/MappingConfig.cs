using AutoMapper;
using CarHub_API.Models;
using CarHub_API.Models.Dto;
using Microsoft.AspNetCore.Identity;

namespace CarHub_API
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            //    CreateMap<VillaDTO, VillaCreateDTO>().ReverseMap();
            //    CreateMap<VillaDTO, VillaUpdateDTO>().ReverseMap();

            //    CreateMap<VillaNumberDTO, VillaNumberCreateDTO>().ReverseMap();
            //    CreateMap<VillaNumberDTO, VillaNumberUpdateDTO>().ReverseMap();

            CreateMap<ApplicationUser, ApplicationUserDTO>().ReverseMap();
            CreateMap<ApplicationUser, IdentityUser>().ReverseMap();
            CreateMap<ApplicationRole, ApplicationRoleDTO>().ReverseMap();
            CreateMap<ApplicationRole, IdentityRole>().ReverseMap();
            CreateMap<ApplicationUserRole, ApplicationUserRoleDTO>().ReverseMap();
            CreateMap<ApplicationUserRole, IdentityUserRole<string>>().ReverseMap();

            CreateMap<Brand, BrandDTO>().ReverseMap();
            CreateMap<Brand, BrandCreateDTO>().ReverseMap();
            CreateMap<Brand, BrandUpdateDTO>().ReverseMap();

            CreateMap<Car, CarDTO>().ReverseMap();
            CreateMap<Car, CarCreateDTO>().ReverseMap();
            CreateMap<Car, CarUpdateDTO>().ReverseMap();

            CreateMap<CarType, CarTypeDTO>().ReverseMap();
            CreateMap<CarType, CarTypeCreateDTO>().ReverseMap();
            CreateMap<CarType, CarTypeUpdateDTO>().ReverseMap();

            CreateMap<Color, ColorDTO>().ReverseMap();
            CreateMap<Color, ColorCreateDTO>().ReverseMap();
            CreateMap<Color, ColorUpdateDTO>().ReverseMap();

            CreateMap<CarXColor, CarXColorDTO>().ReverseMap();
            CreateMap<CarXColor, CarXColorCreateDTO>().ReverseMap();
            CreateMap<CarXColor, CarXColorUpdateDTO>().ReverseMap();

            CreateMap<CarImage, CarImageDTO>().ReverseMap();
            CreateMap<CarImage, CarImageCreateDTO>().ReverseMap();
            CreateMap<CarImage, CarImageUpdateDTO>().ReverseMap();

            CreateMap<Booking, BookingDTO>().ReverseMap();
            CreateMap<Booking, BookingCreateDTO>().ReverseMap();
            CreateMap<Booking, BookingUpdateDTO>().ReverseMap();

            CreateMap<CarSpecification, CarSpecificationDTO>().ReverseMap();
            CreateMap<CarSpecification, CarSpecificationCreateDTO>().ReverseMap();
            CreateMap<CarSpecification, CarSpecificationUpdateDTO>().ReverseMap();

            CreateMap<CarXFeature, CarXFeatureDTO>().ReverseMap();
            CreateMap<CarXFeature, CarXFeatureCreateDTO>().ReverseMap();
            CreateMap<CarXFeature, CarXFeatureUpdateDTO>().ReverseMap();

            CreateMap<Feature, FeatureDTO>().ReverseMap();
            CreateMap<Feature,FeatureCreateDTO>().ReverseMap();
            CreateMap<Feature,FeatureUpdateDTO>().ReverseMap();

            CreateMap<FeatureType, FeatureTypeDTO>().ReverseMap();
            CreateMap<FeatureType, FeatureTypeCreateDTO>().ReverseMap();
            CreateMap<FeatureType, FeatureTypeUpdateDTO>().ReverseMap();

            CreateMap<FeatureXFeaturetype, FeatureXFeaturetypeDTO>().ReverseMap();
            CreateMap<FeatureXFeaturetype, FeatureXFeaturetypeCreateDTO>().ReverseMap();
            CreateMap<FeatureXFeaturetype, FeatureXFeaturetypeUpdateDTO>().ReverseMap();

            CreateMap<Mileage, MileageDTO>().ReverseMap();
            CreateMap<Mileage, MileageCreateDTO>().ReverseMap();
            CreateMap<Mileage, MileageUpdateDTO>().ReverseMap();

            CreateMap<Review, ReviewDTO>().ReverseMap();
            CreateMap<Review, ReviewCreateDTO>().ReverseMap();
            CreateMap<Review, ReviewUpdateDTO>().ReverseMap();

            CreateMap<ReviewXComment, ReviewXCommentDTO>().ReverseMap();
            CreateMap<ReviewXComment, ReviewXCommentCreateDTO>().ReverseMap();
            CreateMap<ReviewXComment, ReviewXCommentUpdateDTO>().ReverseMap();

            CreateMap<Variant, VariantDTO>().ReverseMap();
            CreateMap<Variant, VariantCreateDTO>().ReverseMap();
            CreateMap<Variant, VariantUpdateDTO>().ReverseMap();

            CreateMap<Dealer, DealerDTO>().ReverseMap();
            CreateMap<Dealer, DealerCreateDTO>().ReverseMap();
            CreateMap<Dealer, DealerUpdateDTO>().ReverseMap();

			CreateMap<City, CityDTO>().ReverseMap();
			CreateMap<City, CityCreateDTO>().ReverseMap();
			CreateMap<City, CityUpdateDTO>().ReverseMap();

			CreateMap<Country, CountryDTO>().ReverseMap();
			CreateMap<Country, CountryCreateDTO>().ReverseMap();
			CreateMap<Country, CountryUpdateDTO>().ReverseMap();

			CreateMap<State, StateDTO>().ReverseMap();
			CreateMap<State, StateCreateDTO>().ReverseMap();
			CreateMap<State, StateUpdateDTO>().ReverseMap();
		}
	}
}
