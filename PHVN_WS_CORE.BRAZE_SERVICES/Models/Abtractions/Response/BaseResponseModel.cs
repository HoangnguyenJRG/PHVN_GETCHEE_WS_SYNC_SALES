using Newtonsoft.Json;
using System.Net;


namespace PHVN_WS_CORE.SERVICES.Models.Abtractions.Response
{
    public class BaseResponseModel
    {
        [JsonProperty("code", NullValueHandling = NullValueHandling.Ignore)]
        public HttpStatusCode StatusCode { get; set; }

        [JsonProperty("error", NullValueHandling = NullValueHandling.Ignore)]
        public string ErrorMessage { get; set; }
        [JsonProperty("warning", NullValueHandling = NullValueHandling.Ignore)]
        public string WarningMessage { get; set; }

        [JsonProperty("message", NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; set; }

        [JsonProperty("status", NullValueHandling = NullValueHandling.Ignore)]
        public bool IsSuccessful
        {
            get
            {
                return string.IsNullOrEmpty(ErrorMessage);
            }
        }



        public BaseResponseModel()
        {
            //StatusCode = HttpStatusCode.OK;
        }
        public BaseResponseModel(string warning)
        {
            StatusCode = HttpStatusCode.OK;
            WarningMessage = warning;
        }

        public BaseResponseModel(HttpStatusCode statusCode, string errorMessage)
        {
            ErrorMessage = errorMessage;
            StatusCode = statusCode;
        }
    }
}
