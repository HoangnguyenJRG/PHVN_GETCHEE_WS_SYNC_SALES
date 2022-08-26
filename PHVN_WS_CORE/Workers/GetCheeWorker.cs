using Newtonsoft.Json;
using PHVN_WS_CORE.SERVICES.GetChee;
using PHVN_WS_CORE.SERVICES.Models.Abtractions.Response;
using PHVN_WS_CORE.SERVICES.Models.GetChee;
using PHVN_WS_CORE.SHARED.Apis;
using PHVN_WS_CORE.SHARED.Configurations;
using PHVN_WS_CORE.SHARED.Extensions;
using PHVN_WS_CORE.Workers.Abtractions;
using RestSharp;
using System.Reflection;

namespace PHVN_WS_CORE.Workers
{
    public class GetCheeWorker : BaseWorker
    {
        protected readonly ILogger<GetCheeWorker> logger;
        protected readonly IGetCheeService GetCheeService;
        private GetCheeSettings GetCheeSettings;
        private EmailConfigs oMail = new();
        private string mServer;                
        private int Port;
        private string From;
        private string Password;
        private List<EmailObj> EmailsObj;
        private string ApiKey;
        string pathLogfiles;

        public GetCheeWorker(IConfiguration configuration, ILogger<GetCheeWorker> logger, IGetCheeService GetCheeService, IAPIClientService apiClient) : base(configuration, apiClient)
        {
            this.logger = logger;
            GetCheeSettings = configuration.GetOptions<GetCheeSettings>("GetCheeWSSettings");
            ApiKey = GetCheeSettings.ApiKey;
            this.GetCheeService = GetCheeService;
            pathLogfiles = "";
        }

        public async Task DoWork()
        {
            try
            {
                DateTime pThisMonth = DateTime.Now;

                #region SALES
                List<PHVNSalesData> PHVNSales = await GetCheeService.GetPHVNUpLoadSalesByMonth(pThisMonth);

                if (!PHVNSales.NotNullOrEmpty())           
                {
                    logger.LogInformation("{name} - DoWork - No Data PHVNUploadSales", nameof(PHVNUploadSales));
                    return;
                }
                PHVNUploadSales salesH = new();
                salesH.ApiKey = ApiKey ?? "";
                salesH.Brand = "PH";
                salesH.Company = "PHVN";
                salesH.Month = pThisMonth.Month;
                salesH.Year = pThisMonth.Year;

                salesH.SalesData = PHVNSales;
                await PHVNUploadSales(salesH);


                #endregion


                #region PODSALES
                List<PHVNSalesPODData> PHVNPODSales = await GetCheeService.GetPHVNUpLoadPODSalesByMonth(pThisMonth);
                if (!PHVNPODSales.NotNullOrEmpty())
                {
                    logger.LogInformation("{name} - DoWork - No Data PHVNUploadPODSales", nameof(PHVNUploadPODSales));
                    return;
                }
                PHVNUploadPODSales PodSalesH = new PHVNUploadPODSales();
                PodSalesH.ApiKey = ApiKey ?? "";
                PodSalesH.Brand = "PH";
                PodSalesH.Company = "PHVN";
                PodSalesH.Month = pThisMonth.Month;
                PodSalesH.Year = pThisMonth.Year;

                PodSalesH.PodData = PHVNPODSales;
                await PHVNUploadPODSales(PodSalesH);
                #endregion

            }
            catch (Exception ex)
            {
                string msg = ex.Message.ToString();
                if (ex.InnerException != null)
                    msg += Environment.NewLine + "Inner Msg: " + ex.InnerException.ToString();

                logger.LogInformation("{name} - DoWork - {error}", nameof(GetCheeWorker), msg);
            }
        }

        private async Task PHVNUploadSales(PHVNUploadSales data)
        {
            try
            {
                
                WriteFiles(nameof(PHVNUploadSales), "", JsonConvert.SerializeObject(data,Formatting.Indented));
                var response = await apiClient.PostAsync<ResponseModel<PHVNUploadSalesResponses>>(GetCheeSettings.BaseUrl, "api/UploadDataVN/UploadSales", data);

                if (response.IsSuccessful == false)
                {
                    throw new Exception($"StatusCode: {response.StatusCode.ToString()} - Msg: {response.ErrorException?.Message.ToString()}");


                }
                logger.LogInformation("{name} - Successfully.", nameof(PHVNUploadSales));

            }
            catch (Exception ex)
            {
                string msg = $"{nameof(PHVNUploadSales)} Error: {ex.Message.ToString()}";

                if (ex.InnerException != null)
                    msg += Environment.NewLine + "Inner Msg: " + ex.InnerException.ToString();

                throw new Exception(msg);
            }
        }

        private async Task PHVNUploadPODSales(PHVNUploadPODSales data)
        {
            try
            {
                WriteFiles(nameof(PHVNUploadPODSales), "", JsonConvert.SerializeObject(data, Formatting.Indented));
                var response = await apiClient.PostAsync<ResponseModel<PHVNUploadPODSalesResponses>>(GetCheeSettings.BaseUrl, "api/UploadDataVN/UploadPodSales", data);

                if (response.IsSuccessful == false)
                {
                    throw new Exception($"StatusCode: {response.StatusCode.ToString()} - Msg: {response.ErrorException?.Message.ToString()}");
                }
                logger.LogInformation("{name} - Successfully.", nameof(PHVNUploadPODSales));
            }
            catch (Exception ex)
            {
                string msg = $"{nameof(PHVNUploadPODSales)} Error: {ex.Message.ToString()}";

                if (ex.InnerException != null)
                    msg += Environment.NewLine + "Inner Msg: " + ex.InnerException.ToString();

                throw new Exception(msg);
            }
        }

        public void WriteFiles(string pProjectName, string pFilePath, string pLogMess)
        {
            try
            {
                string localPath = new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath;
                localPath = Path.GetDirectoryName(localPath) + $"\\LogDataFiles\\{pProjectName}";
                if (pFilePath.Trim() != "" && Directory.Exists(pFilePath))
                {
                    localPath = pFilePath;
                }

                if (!Directory.Exists(localPath))
                {
                    Directory.CreateDirectory(localPath);
                }

                string path = localPath + "\\" + pProjectName + "_" + DateTime.Now.ToString("yyyyMMdd_hhmmss") + ".json";
                using StreamWriter streamWriter = File.AppendText(path);
                streamWriter.Write(pLogMess);
                streamWriter.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
