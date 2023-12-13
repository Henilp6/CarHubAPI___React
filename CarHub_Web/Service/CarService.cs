

using CarHub_Utility;
using CarHub_Web.Models;
using CarHub_Web.Models.Dto;
using CarHub_Web.Service;
using CarHub_Web.Service.IService;

namespace CarHub_Web.Service{    public class CarService : BaseService, ICarService    {        private readonly IHttpClientFactory _clientFactory;        private string carUrl;        public CarService(IHttpClientFactory clientFactory, IConfiguration configuration) : base(clientFactory)        {            _clientFactory = clientFactory;            carUrl = configuration.GetValue<string>("ServiceUrls:CarAPI");        }        public Task<T> CreateAsync<T>(CarCreateDTO dto, string token)        {            return SendAsync<T>(new APIRequest()            {                ApiType = SD.ApiType.POST,                Data = dto,                Url = carUrl + "/api/v1/CarAPI/CreateCar",                Token = token            });        }        public Task<T> DeleteAsync<T>(int id, string token)        {            return SendAsync<T>(new APIRequest()            {                ApiType = SD.ApiType.DELETE,                Url = carUrl + "/api/v1/CarAPI/DeleteCar/" + id,                Token = token            });        }        public Task<T> GetAllAsync<T>(string token)        {            return SendAsync<T>(new APIRequest()            {                ApiType = SD.ApiType.GET,                Url = carUrl + "/api/v1/CarAPI/GetCars",                Token = token            });        }        //public Task<T> GetCarData<T>(int pageNum, string token)        //{        //    string apiUrl = $"{carUrl}/api/v1/CarAPI/GetCarData?pageNum={pageNum}";        //    return SendAsync<T>(new APIRequest()        //    {        //        ApiType = SD.ApiType.GET,        //        Url = apiUrl,        //        Token = token        //    });        //}        public Task<T> GetAsync<T>(int id, string token)        {            return SendAsync<T>(new APIRequest()            {                ApiType = SD.ApiType.GET,                Url = carUrl + "/api/v1/CarAPI/GetCar/" + id,                Token = token            });        }        public Task<T> UpdateAsync<T>(CarUpdateDTO dto, string token)        {            return SendAsync<T>(new APIRequest()            {                ApiType = SD.ApiType.PUT,                Data = dto,                Url = carUrl + "/api/v1/CarAPI/UpdateCar/" + dto.Id,                Token = token            });        }
        public Task<T> CarSearchByLazyLoading<T>(int pageNum, string searchText, string token)
        {
            string apiUrl = $"{carUrl}/api/v1/CarAPI/CarSearchByLazyLoading?pageNum={pageNum}&searchText={searchText}";
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = apiUrl,
                Token = token
            });
        }
		public Task<T> AllDataAsync<T>(string term, string orderBy, int currentPage, string token)
		{
			//string apiUrl = $"{carUrl}/api/v1/StateAPI/GetStatesData/{Id}/{search}/{pageSize}/{pageNumber}";
			string apiUrl = $"{carUrl}/api/v1/CarAPI/GetDataIndex?term={term}&orderBy={orderBy}&currentPage={currentPage}";

			return SendAsync<T>(new APIRequest()
			{
				ApiType = SD.ApiType.GET,
				Url = apiUrl,
				Token = token
			});

		}
	}}
