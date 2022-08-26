namespace PHVN_WS_CORE.SERVICES.Models.Braze
{
    public class PurchaseModel
    {
        public int Id { get; set; }
        public bool loyalty_member { get; set; }
        public string external_id { get; set; }
        public string alias_name { get; set; }
        public string alias_label { get; set; }
        public string app_id { get; set; }
        public string product_id { get; set; }
        public string currency { get; set; }
        public int price { get; set; }
        public int quantity { get; set; }
        public DateTime time { get; set; }
        public string store_id { get; set; }
        public string order_id { get; set; }
        public string order_type { get; set; }
        public string channel { get; set; }
    }
}
