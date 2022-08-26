using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PHVN_WS_CORE.SERVICES.Models.GetChee
{
    public class PHVNUploadSalesModel
    {
        public string Company { get; set; }
        public string Month { get; set; }
        public string Store { get; set; }
        public string OrderChannel { get; set; }
        public string DayPart { get; set; }
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }
        public int TotalTC { get; set; }
        public string Currency { get; set; }
        public decimal TotalAmt { get; set; }
        public decimal AvgDeliveryTime { get; set; }
        public decimal AvgMakeTime { get; set; }
    }

    public class ParameterPHVNUploadSalesModel
    {
        public string fromDate { get; set; }
        public string toDate { get; set; }
        
    }
}
