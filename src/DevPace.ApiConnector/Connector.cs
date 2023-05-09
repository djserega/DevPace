using Newtonsoft.Json;
using RestSharp;

namespace DevPace.ApiConnector
{
    public class Connector
    {
        private readonly RestClient _restClient;
        private readonly Config _config;

        public Connector()
        {
            _config = new();

            _restClient = new(new HttpClient() { BaseAddress = new Uri(_config.Server) });
        }

        public async Task<IEnumerable<Models.Customer>?> GetCustomerList()
        {
            RestRequest request = new(_config.GetCustomerList);

            return await GetData<IEnumerable<Models.Customer>>(request);
        }

        public async Task<Models.Customer?> AddCustomer(Models.Customer customer)
        {
            RestRequest request = new(_config.PostCustomer);

            request.AddBody(customer);

            return (customer.Id == default)
                ? await PostData<Models.Customer>(request)
                : await PutData<Models.Customer>(request);
        }

        public async Task RemoveCustomer(Models.Customer customer)
        {
            RestRequest request = new(_config.PostCustomer + "/" + customer.Id);

            await DeleteData(request);
        }

        public async Task<Models.Customer?> GetCustomer(int id)
        {
            RestRequest request = new(_config.GetCustomer + "/" + id);

            return await GetData<Models.Customer>(request);
        }


        #region Requests

        private async Task<T?> GetData<T>(RestRequest request)
        {
            CheckRequest(request);

            RestResponse response;
            try
            {
                response = await _restClient.GetAsync(request);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            if (string.IsNullOrEmpty(response.Content))
                return default;
            else
                return JsonConvert.DeserializeObject<T>(response.Content);
        }

        private async Task<T?> PostData<T>(RestRequest request)
        {
            CheckRequest(request);

            RestResponse response;
            try
            {
                response = await _restClient.PostAsync(request);

                if (string.IsNullOrEmpty(response.Content))
                    return default;
                else
                    return JsonConvert.DeserializeObject<T>(response.Content);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task<T?> PutData<T>(RestRequest request)
        {
            CheckRequest(request);

            RestResponse response;
            try
            {
                response = await _restClient.PutAsync(request);

                if (string.IsNullOrEmpty(response.Content))
                    return default;
                else
                    return JsonConvert.DeserializeObject<T>(response.Content);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task DeleteData(RestRequest request)
        {
            CheckRequest(request);

            RestResponse response;
            try
            {
                response = await _restClient.DeleteAsync(request);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        #endregion


        private static void CheckRequest(RestRequest request)
        {
            if (request.Timeout == default)
                request.Timeout = 10 * 1000;
        }

    }
}