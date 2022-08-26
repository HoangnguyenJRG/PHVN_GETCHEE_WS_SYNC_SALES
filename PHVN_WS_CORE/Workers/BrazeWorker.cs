using Newtonsoft.Json;
using PHVN_WS_CORE.SERVICES.Braze;
using PHVN_WS_CORE.SERVICES.Models.Abtractions.Response;
using PHVN_WS_CORE.SERVICES.Models.Braze;
using PHVN_WS_CORE.SHARED.Apis;
using PHVN_WS_CORE.SHARED.Configurations;
using PHVN_WS_CORE.SHARED.Extensions;
using PHVN_WS_CORE.Workers.Abtractions;
using RestSharp;

namespace PHVN_WS_CORE.Workers
{
    public class BrazeWorker : BaseWorker
    {
        protected readonly ILogger<BrazeWorker> logger;
        protected readonly IBrazeService brazeService;
        private BrazeSettings brazeSettings;

        public BrazeWorker(IConfiguration configuration, ILogger<BrazeWorker> logger, IBrazeService brazeService, IAPIClientService apiClient) : base(configuration, apiClient)
        {
            this.logger = logger;
            brazeSettings = configuration.GetOptions<BrazeSettings>("BrazeSettings");
            this.brazeService = brazeService;
        }

        public async Task DoWork()
        {
            try
            {
                List<PurchaseModel> purchases = await brazeService.GetAllPurchaseAsync();
                List<string> ids = new List<string>();

                if (!purchases.NotNullOrEmpty())
                    return;

                ids = purchases.Select(x => x.Id.ToString()).ToList();

                List<BrazePurchaseModel> member = purchases.Where(t => t.loyalty_member == true).Select(i => new BrazePurchaseModel()
                {
                    external_id = i.external_id,
                    app_id = i.app_id,
                    //app_id = i.Id == 796936 ?  i.app_id : "80fb7bd6-e9c1-4ae0-97ab-3747ad2edb0d",
                    product_id = i.product_id,
                    currency = i.currency,
                    price = i.price,
                    quantity = i.quantity,
                    time = i.time,
                    properties = new Properties()
                    {
                        order_type = i.order_type,
                        store_id = i.store_id,
                        channel = i.channel
                    }
                }).ToList();

                List<BrazePurchaseModel> nonMember = purchases.Where(t => t.loyalty_member == false).Select(i => new BrazePurchaseModel()
                {
                    user_alias = new user_alias()
                    {
                        alias_name = i.alias_name,
                        alias_label = i.alias_label
                    },
                    app_id = i.app_id,
                    product_id = i.product_id,
                    currency = i.currency,
                    price = i.price,
                    quantity = i.quantity,
                    time = i.time,
                    properties = new Properties()
                    {
                        order_type = i.order_type,
                        store_id = i.store_id,
                        channel = i.channel
                    },
                    _update_existing_only = false
                }).ToList();

                List<BrazePurchaseModel> data = new();
                PurchaseDataSync purchaseDataSync = new();

                if (member.NotNullOrEmpty())
                {
                    data.AddRange(member);
                }

                if (nonMember.NotNullOrEmpty())
                {
                    data.AddRange(nonMember);
                }

                if (!data.NotNullOrEmpty())
                    return;

                purchaseDataSync.purchases = data;

                await SyncPurchaseData(purchaseDataSync);
            }
            catch (Exception ex)
            {
                string msg = ex.Message.ToString();
                if (ex.InnerException != null)
                    msg += Environment.NewLine + "Inner Msg: " + ex.InnerException.ToString();

                logger.LogInformation("{name} - DoWork - {error}", nameof(BrazeWorker), msg);
            }
        }

        private async Task SyncPurchaseData(PurchaseDataSync data)
        {
            try
            {
                var response = await apiClient.PostAsync<ResponseModel<BrazeResponseModel>>(brazeSettings.BaseUrl, "users/track", data, new Dictionary<string, string>{
                                                {"X-Braze-Bulk", "true"},
                                                {"Authorization", "Bearer " + brazeSettings.AccessToken}
                                            });

                if(response.IsSuccessful == false)
                {
                    throw new Exception($"StatusCode: {response.StatusCode.ToString()} - Msg: {response.ErrorException?.Message.ToString()}");
                }
            }
            catch(Exception ex)
            {
                string msg = $"{nameof(SyncPurchaseData)} Error: {ex.Message.ToString()}";

                if (ex.InnerException != null)
                    msg += Environment.NewLine + "Inner Msg: " + ex.InnerException.ToString();

                throw new Exception(msg);
            }
        }
    }
}
