using CarHub_Utility;
using CarHub_Web.Models;
using CarHub_Web.Models.Dto;
using CarHub_Web.Service.IService;

namespace CarHub_Web.Service{
	public class ApplicationRoleService : BaseService, IApplicationRoleService    {
		private readonly IHttpClientFactory _clientFactory;
		private string carUrl;

		public ApplicationRoleService(IHttpClientFactory clientFactory, IConfiguration configuration) : base(clientFactory)
		{
            _clientFactory = clientFactory;
            carUrl = configuration.GetValue<string>("ServiceUrls:CarAPI");

		}

        public Task<T> CreateAsync<T>(ApplicationRoleDTO dto, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = dto,
                Url = carUrl + "/api/v1/applicationRoleAPI",
                Token = token
            });
        }

        public Task<T> DeleteAsync<T>(string id, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.DELETE,
                Url = carUrl + "/api/v1/applicationRoleAPI/" + id,
                Token = token
            });
        }

        public Task<T> GetAllAsync<T>(string token)        {            return SendAsync<T>(new APIRequest()            {                ApiType = SD.ApiType.GET,                Url = carUrl + "/api/v1/applicationRoleAPI",                Token = token            });        }        public Task<T> GetAsync<T>(string Id, string token)        {            return SendAsync<T>(new APIRequest()            {                ApiType = SD.ApiType.GET,                Url = carUrl + "/api/v1/applicationRoleAPI/" + Id,                Token = token            });        }        public Task<T> UpdateAsync<T>(ApplicationRoleDTO dto, string token)        {            return SendAsync<T>(new APIRequest()            {                ApiType = SD.ApiType.PUT,                Data = dto,                Url = carUrl + "/api/v1/applicationRoleAPI/" + dto.Id,                Token = token            });        }    }}
