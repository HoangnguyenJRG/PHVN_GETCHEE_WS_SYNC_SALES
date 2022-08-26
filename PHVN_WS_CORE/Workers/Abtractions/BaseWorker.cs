using PHVN_WS_CORE.SHARED.Apis;


namespace PHVN_WS_CORE.Workers.Abtractions
{
    public class BaseWorker
    {
        protected readonly IAPIClientService apiClient;
        public BaseWorker(IConfiguration configuration, IAPIClientService apiClient)
        {
            this.apiClient = apiClient;
        }
    }
}
