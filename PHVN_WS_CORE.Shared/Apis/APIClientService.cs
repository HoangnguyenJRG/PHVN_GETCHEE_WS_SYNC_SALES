using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Serializers;
using System.Web;

namespace PHVN_WS_CORE.SHARED.Apis
{
    public class APIClientService : IAPIClientService, IDisposable
    {
        public APIClientService(IConfiguration configuration)
        {
        }

        public async Task<TResult> GetAsync<TResult>(string endpointServer, string url, Dictionary<string, object> queries = null, Dictionary<string, string> headers = null )
            => await SendRequest<TResult>(endpointServer, url, Method.Get, queries: queries,headers: headers);

        public async Task<TResult> PostAsync<TResult>(string endpointServer, string url, object data, Dictionary<string, string> headers = null)
            => await SendRequest<TResult>(endpointServer, url, Method.Post, data, headers: headers);

        public async Task<TResult> PutAsync<TResult>(string endpointServer, string url, object data, Dictionary<string, string> headers = null)
            => await SendRequest<TResult>(endpointServer, url, Method.Put, data, headers: headers);

        public async Task<TResult> DeleteAsync<TResult>(string endpointServer, string url, Dictionary<string, string> headers = null)
            => await SendRequest<TResult>(endpointServer, url, Method.Delete, headers: headers);


        private async Task<TResult> SendRequest<TResult>(string endpointServer,
                                                         string url, 
                                                         Method method, 
                                                         object data = null,
                                                         Dictionary<string, object> queries = null,
                                                         Dictionary<string, string> headers = null)
        {
            try
            {
                var options = new RestClientOptions(endpointServer)
                {
                    MaxTimeout = 5000
                };

                using var client = new RestClient(options);
                var request = new RestRequest(url, method);
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("Connection", "keep-alive");

                if (headers != null && headers.Count > 0)
                {
                    foreach (var header in headers)
                        request.AddHeader(header.Key, header.Value);
                }

                if (queries != null && queries.Count > 0 && method == Method.Get)
                {
                    request.AddObject(queries);
                }

                if ((method == Method.Post || method == Method.Put) && data != null)
                {
                    request.AddJsonBody(data);
                }
                string json = JsonConvert.SerializeObject(data);
                var response = await client.ExecuteAsync(request);

            



                if (response != null)
                {
                    var resObjs = new
                    {
                        StatusCode = response.StatusCode,
                        IsSuccessful = response.IsSuccessful,
                        Data = JsonConvert.DeserializeObject(response.Content),
                        ResponseStatus = response.ResponseStatus,
                        ErrorException = response.ErrorException
                    };

                    var obj = JsonConvert.SerializeObject(resObjs);

                    return JsonConvert.DeserializeObject<TResult>(obj);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return default(TResult);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
