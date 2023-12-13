

using CarHub_Web.Models.Dto;

namespace CarHub_Web.Service.IService
{
    public interface ICarTypeService
    {
      
            Task<T> GetAllAsync<T>(string token);
            Task<T> GetAsync<T>(int id, string token);
            Task<T> CreateAsync<T>(CarTypeCreateDTO dto, string token);
            Task<T> UpdateAsync<T>(CarTypeUpdateDTO dto, string token);
            Task<T> DeleteAsync<T>(int id, string token);
        Task<T> AllDataAsync<T>(string term, string orderBy, int currentPage, string token);


    }
}
