using CarHub_Web.Models.Dto;using CarHub_Web.Models.VM;namespace CarHub_Web.Service.IService{
	public interface ICarXColorService
	{

		Task<T> GetAllAsync<T>(string token);
		Task<T> GetAsync<T>(int id, string token);
		Task<T> CreateAsync<T>(CarXColorCreateVM dto, string token);
		Task<T> UpdateAsync<T>(CarXColorUpdateVM dto, string token);
		Task<T> DeleteAsync<T>(int id, string token);
		

	}}