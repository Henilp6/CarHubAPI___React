using AutoMapper;
using CarHub_Web.Models.Dto;

namespace CarHub_Web
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<BookingDTO, BookingCreateDTO>().ReverseMap();
            CreateMap<BookingDTO, BookingUpdateDTO>().ReverseMap();

            CreateMap<BrandDTO, BrandCreateDTO>().ReverseMap();
            CreateMap<BrandDTO, BrandUpdateDTO>().ReverseMap();

            CreateMap<CarDTO, CarCreateDTO>().ReverseMap();
            CreateMap<CarDTO, CarUpdateDTO>().ReverseMap();

            CreateMap<CarImageDTO, CarImageCreateDTO>().ReverseMap();
            CreateMap<CarImageDTO, CarImageUpdateDTO>().ReverseMap();

            CreateMap<CarSpecificationDTO, CarSpecificationCreateDTO>().ReverseMap();
            CreateMap<CarSpecificationDTO, CarSpecificationUpdateDTO>().ReverseMap();

            CreateMap<CarTypeDTO, CarTypeCreateDTO>().ReverseMap();
            CreateMap<CarTypeDTO, CarTypeUpdateDTO>().ReverseMap();

            CreateMap<CarXColorDTO, CarXColorCreateDTO>().ReverseMap();
            CreateMap<CarXColorDTO, CarXColorUpdateDTO>().ReverseMap();

            CreateMap<CarXFeatureDTO, CarXFeatureCreateDTO>().ReverseMap();
            CreateMap<CarXFeatureDTO, CarXFeatureUpdateDTO>().ReverseMap();

            CreateMap<ColorDTO, ColorCreateDTO>().ReverseMap();
            CreateMap<ColorDTO, ColorUpdateDTO>().ReverseMap();

            CreateMap<DealerDTO, DealerCreateDTO>().ReverseMap();
            CreateMap<DealerDTO, DealerUpdateDTO>().ReverseMap();

            CreateMap<FeatureDTO, FeatureCreateDTO>().ReverseMap();
            CreateMap<FeatureDTO, FeatureUpdateDTO>().ReverseMap();

            CreateMap<FeatureTypeDTO, FeatureTypeCreateDTO>().ReverseMap();
            CreateMap<FeatureTypeDTO, FeatureTypeUpdateDTO>().ReverseMap();

            CreateMap<FeatureXFeaturetypeDTO, FeatureXFeaturetypeCreateDTO>().ReverseMap();
            CreateMap<FeatureXFeaturetypeDTO, FeatureXFeaturetypeUpdateDTO>().ReverseMap();

            CreateMap<MileageDTO, MileageCreateDTO>().ReverseMap();
            CreateMap<MileageDTO, MileageUpdateDTO>().ReverseMap();

            CreateMap<ReviewDTO, ReviewCreateDTO>().ReverseMap();
            CreateMap<ReviewDTO, ReviewUpdateDTO>().ReverseMap();

            CreateMap<ReviewXCommentDTO, ReviewXCommentCreateDTO>().ReverseMap();
            CreateMap<ReviewXCommentDTO, ReviewXCommentUpdateDTO>().ReverseMap();

            CreateMap<VariantDTO, VariantCreateDTO>().ReverseMap();
            CreateMap<VariantDTO, VariantUpdateDTO>().ReverseMap();

			CreateMap<StateDTO, StateCreateDTO>().ReverseMap();
			CreateMap<StateDTO, StateUpdateDTO>().ReverseMap();

			CreateMap<CountryDTO, CountryCreateDTO>().ReverseMap();
			CreateMap<CountryDTO, CountryUpdateDTO>().ReverseMap();

			CreateMap<CityDTO, CityCreateDTO>().ReverseMap();
			CreateMap<CityDTO, CityUpdateDTO>().ReverseMap();

			CreateMap<MileageDTO, MileageCreateDTO>().ReverseMap();
			CreateMap<MileageDTO, MileageUpdateDTO>().ReverseMap();

			
		}
    }
}
