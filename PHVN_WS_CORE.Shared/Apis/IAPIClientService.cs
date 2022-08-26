using RestSharp;

namespace PHVN_WS_CORE.SHARED.Apis
{
    public interface IAPIClientService
    {
        Task<TResult> GetAsync<TResult>(string endpointServer, string url, Dictionary<string, object> queries = null, Dictionary<string, string> headers = null);
        Task<TResult> PostAsync<TResult>(string endpointServer, string url, object data, Dictionary<string, string> headers = null);
        Task<TResult> PutAsync<TResult>(string endpointServer, string url, object data, Dictionary<string, string> headers = null);
        Task<TResult> DeleteAsync<TResult>(string endpointServer, string url, Dictionary<string, string> headers = null);
    }
}