using CarHub_Web.Models.Dto;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CarHub_Web.Models.VM
{
    public class HomeVM
    {
        public CarSpecificationDTO CarSpecification { get; set; }
        public MileageDTO Mileage { get; set; }
        public CarImageDTO CarImage { get; set; }
        public CarDTO Car { get; set; } = new CarDTO();
        public ReviewDTO Review { get; set; }
        [ValidateNever]
        public List<CarDTO> CarList { get; set; }
        [ValidateNever]
        public List<BrandDTO> BrandList { get; set; }
        [ValidateNever]
        public List<CarTypeDTO> CarTypeList { get; set; }

        [ValidateNever]
        public List<CarImageDTO> CarImagelist { get; set; }
        [ValidateNever]
        public List<MileageDTO> MileageList { get; set; }
        [ValidateNever]
        public List<CarSpecificationDTO> CarSpecificationList { get; set; }
        [ValidateNever]
        public List<CarXFeatureDTO> CarXFeatureList { get; set; }
        [ValidateNever]
        public List<FeatureTypeDTO> FeatureTypeList { get; set; }
        [ValidateNever]
        public List<FeatureXFeaturetypeDTO> FeatureXFeaturetypeList { get; set; }
        [ValidateNever]
        public List<ReviewDTO> ReviewList { get; set; }
        [ValidateNever]
        public List<ReviewXCommentDTO> ReviewXCommentList { get; set; }

        public ReviewXCommentCreateDTO ReviewXComment { get; set; } = new ReviewXCommentCreateDTO();

    }
}
