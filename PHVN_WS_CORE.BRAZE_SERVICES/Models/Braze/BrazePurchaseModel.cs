using Newtonsoft.Json;
using System.Net;

namespace PHVN_WS_CORE.SERVICES.Models.Braze
{
    public class PurchaseDataSync
    {
        public List<BrazePurchaseModel> purchases { get; set; }
    }

    public class BrazePurchaseModel
    {
        public int id { get; set; }
        public string external_id { get; set; }
        public user_alias user_alias { get; set; }
        public string app_id { get; set; }
        public string product_id { get; set; }
        public string currency { get; set; }
        public int price { get; set; }
        public int quantity { get; set; }
        public DateTime time { get; set; }
        public Properties properties { get; set; }
        public bool _update_existing_only { get; set; }
    }

    public class Properties
    {
        public string store_id { get; set; }
        public int order_id { get; set; }
        public string order_type { get; set; }
        public string channel { get; set; }
    }

    public class user_alias
    {
        public string alias_name { get; set; }
        public string alias_label { get; set; }
    }

    public class BrazeResponseModel
    {
        public string message { get; set; }
        public int attributes_processed { get; set; }
        public int events_processed { get; set; }
        public int purchases_processed { get; set; }
    }
}
