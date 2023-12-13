using CarHub_Utility;
using CarHub_Web.Models;
using CarHub_Web.Models.Dto;
using CarHub_Web.Service.IService;

namespace CarHub_Web.Service{
	public class AuthService : BaseService, IAuthService    {
		private readonly IHttpClientFactory _clientFactory;
		private string carUrl;

		public AuthService(IHttpClientFactory clientFactory, IConfiguration configuration) : base(clientFactory)
		{
			_clientFactory = clientFactory;
            carUrl = configuration.GetValue<string>("ServiceUrls:CarAPI");

		}

		public Task<T> LoginAsync<T>(LoginRequestDTO obj)        {            return SendAsync<T>(new APIRequest()            {                ApiType = SD.ApiType.POST,                Data = obj,                Url = carUrl + "/api/v1/UsersAuth/login"            });        }        public Task<T> RegisterAsync<T>(RegisterationRequestDTO obj)        {            return SendAsync<T>(new APIRequest()            {                ApiType = SD.ApiType.POST,                Data = obj,                Url = carUrl + "/api/v1/UsersAuth/register"            });        }    }}