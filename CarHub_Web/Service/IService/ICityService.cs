

using CarHub_Web.Models.Dto;

namespace CarHub_Web.Service.IService
{
	public interface ICityService
	{
		Task<T> GetAllAsync<T>(string token);
		Task<T> GetAsync<T>(int id, string token);
		Task<T> CreateAsync<T>(CityCreateDTO dto, string token);
		Task<T> UpdateAsync<T>(CityUpdateDTO dto, string token);
		Task<T> DeleteAsync<T>(int id, string token);
		Task<T> CityByPagination<T>(string term, string orderBy, int currentPage, string token);

	}
}
