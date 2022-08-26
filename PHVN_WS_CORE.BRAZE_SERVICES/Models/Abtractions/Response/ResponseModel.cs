
using Newtonsoft.Json;
using RestSharp;
using System.Net;

namespace PHVN_WS_CORE.SERVICES.Models.Abtractions.Response
{
    public class ResponseModel<TModel>
    {
        public TModel Data { get; set; }

        public HttpStatusCode StatusCode { get; set; }

        public string Message { get; set; }

        public bool IsSuccessful { get; set; }

        public ResponseStatus ResponseStatus { get; set; }
        public string ErrorMessage { get; set; }

        public ErrorExceptionModel ErrorException { get; set; }

    }

    public class ErrorExceptionModel
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Message { get; set; }
    }
}
