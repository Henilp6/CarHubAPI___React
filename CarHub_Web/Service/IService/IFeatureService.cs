using CarHub_Web.Models.Dto;
using System.Threading.Tasks;

namespace CarHub_Web.Service.IService
{
    public interface IFeatureService
    {
      
            Task<T> GetAllAsync<T>(string token);
            Task<T> GetAsync<T>(int id, string token);
            Task<T> CreateAsync<T>(FeatureCreateDTO dto, string token);
            Task<T> UpdateAsync<T>(FeatureUpdateDTO dto, string token);
            Task<T> DeleteAsync<T>(int id, string token);
		Task<T> AllDataAsync<T>(string term, string orderBy, int currentPage, string token);

	}
}
