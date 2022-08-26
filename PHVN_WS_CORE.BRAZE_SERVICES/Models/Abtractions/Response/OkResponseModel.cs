
using Newtonsoft.Json;
using System.Net;

namespace PHVN_WS_CORE.SERVICES.Models.Abtractions.Response
{
    public class OkResponseModel : BaseResponseModel
    {
        public OkResponseModel()
        {
        }

        public OkResponseModel(string message)
        {
            Message = message;
            StatusCode = HttpStatusCode.OK;
        }
    }

    public class OkResponseModel<T> : BaseResponseModel
    {
        [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
        public T Data { get; set; }

        public OkResponseModel(T data)
        {
            Data = data;
        }
    }
}
