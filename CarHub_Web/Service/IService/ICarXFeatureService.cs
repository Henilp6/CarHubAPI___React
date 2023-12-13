
using CarHub_Web.Models.Dto;using CarHub_Web.Models.VM;namespace CarHub_Web.Service.IService{
	public interface ICarXFeatureService
	{

		Task<T> GetAllAsync<T>(string token);
		Task<T> GetAsync<T>(int id, string token);
		Task<T> CreateAsync<T>(CarXFeatureCreateVM dto, string token);
		Task<T> UpdateAsync<T>(CarXFeatureUpdateVM dto, string token);
		Task<T> DeleteAsync<T>(int id, string token);

	}}