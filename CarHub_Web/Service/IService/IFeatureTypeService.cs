

using CarHub_Web.Models.Dto;

namespace CarHub_Web.Service.IService
{
    public interface IFeatureTypeService
    {

        Task<T> GetAllAsync<T>(string token);
        Task<T> GetAsync<T>(int id, string token);
        Task<T> CreateAsync<T>(FeatureTypeCreateDTO dto, string token);
        Task<T> UpdateAsync<T>(FeatureTypeUpdateDTO dto, string token);
        Task<T> DeleteAsync<T>(int id, string token);
        Task<T> FeatureTypeByPagination<T>(string term, string orderBy, int currentPage, string token);

    }
}
