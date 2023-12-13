

using CarHub_Web.Models.Dto;

namespace CarHub_Web.Service.IService
{
    public interface IColorService
    {
      
            Task<T> GetAllAsync<T>(string token);
            Task<T> GetAsync<T>(int id, string token);
            Task<T> CreateAsync<T>(ColorCreateDTO dto, string token);
            Task<T> UpdateAsync<T>(ColorUpdateDTO dto, string token);
            Task<T> DeleteAsync<T>(int id, string token);
		Task<T> AllDataAsync<T>(string term, string orderBy, int currentPage, string token);

	}
}
