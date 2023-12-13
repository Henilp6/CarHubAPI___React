using CarHub_Utility;
using CarHub_Web.Models;
using CarHub_Web.Models.Dto;
using CarHub_Web.Models.VM;
using CarHub_Web.Service;
using CarHub_Web.Service.IService;

namespace CarHub_Web.Service
{
	public class FeatureXFeaturetypeService : BaseService, IFeatureXFeaturetypeService
	{
		private readonly IHttpClientFactory _clientFactory;
		private string carUrl;

		public FeatureXFeaturetypeService(IHttpClientFactory clientFactory, IConfiguration configuration) : base(clientFactory)
		{
			_clientFactory = clientFactory;
			carUrl = configuration.GetValue<string>("ServiceUrls:CarAPI");

		}

		public Task<T> CreateAsync<T>(FeatureXFeaturetypeCreateVM dto, string token)
		{
			return SendAsync<T>(new APIRequest()
			{
				ApiType = SD.ApiType.POST,
				Data = dto,
				Url = carUrl + "/api/v1/FeatureXFeaturetypeAPI/CreateFeatureXFeaturetype",
				Token = token
			});
		}

		public Task<T> DeleteAsync<T>(int id, string token)
		{
			return SendAsync<T>(new APIRequest()
			{
				ApiType = SD.ApiType.DELETE,
				Url = carUrl + "/api/v1/FeatureXFeaturetypeAPI/DeleteFeatureXFeaturetype/" + id,
				Token = token
			});
		}

		public Task<T> GetAllAsync<T>(string token)
		{
			return SendAsync<T>(new APIRequest()
			{
				ApiType = SD.ApiType.GET,
				Url = carUrl + "/api/v1/FeatureXFeaturetypeAPI/GetFeatureXFeaturetypes",
				Token = token
			});
		}

		public Task<T> GetAsync<T>(int id, string token)
		{
			return SendAsync<T>(new APIRequest()
			{
				ApiType = SD.ApiType.GET,
				Url = carUrl + "/api/v1/FeatureXFeaturetypeAPI/" + id,
				Token = token
			});
		}

		public Task<T> UpdateAsync<T>(FeatureXFeaturetypeUpdateVM dto, string token)
		{
			return SendAsync<T>(new APIRequest()
			{
				ApiType = SD.ApiType.PUT,
				Data = dto,
				Url = carUrl + "/api/v1/FeatureXFeaturetypeAPI/UpdateFeatureXFeaturetype",
				Token = token
			});
		}
		public Task<T> AllDataAsync<T>(string term, string orderBy, int currentPage, string token)
		{
			//string apiUrl = $"{carUrl}/api/v1/StateAPI/GetStatesData/{Id}/{search}/{pageSize}/{pageNumber}";
			string apiUrl = $"{carUrl}/api/v1/FeatureXFeaturetypeAPI/GetFeatureXFeaturetypeData?term={term}&orderBy={orderBy}&currentPage={currentPage}";

			return SendAsync<T>(new APIRequest()
			{
				ApiType = SD.ApiType.GET,
				Url = apiUrl,
				Token = token
			});

		}
	}
}
