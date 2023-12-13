

using CarHub_Web.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace CarHub_Web.Service.IService
{
	public interface IBrandService
	{

		Task<T> GetAllAsync<T>(string token);
		Task<T> GetAsync<T>(int id, string token);
		Task<T> CreateAsync<T>(BrandCreateDTO dto, string token);
		Task<T> UpdateAsync<T>(BrandUpdateDTO dto, string token);
		Task<T> DeleteAsync<T>(int id, string token);
		Task<T> AllDataAsync<T>(string term, string orderBy, int currentPage, string token);

	}
}
