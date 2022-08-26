using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PHVN_WS_CORE.SERVICES.Models.GetChee
{
   
        public class PHVNUploadSales
        {
            public string ApiKey { get; set; }
            public string Brand { get; set; }
            public string Company { get; set; }
            public int Month { get; set; }
            public int Year { get; set; }

            public List<PHVNSalesData> SalesData { get; set; }
        }

        public class PHVNSalesData
        {
            public decimal AvgDeliveryTime { get; set; }
            public decimal AvgMakeTime { get; set; }
            public string Currency { get; set; }
            public string DayPart { get; set; }
            public decimal Latitude { get; set; }
            public decimal Longitude { get; set; }
            public string OrderChannel { get; set; }
            public string Store { get; set; }
            public decimal TotalAmt { get; set; }
            public int TotalTC { get; set; }

        }
        public class PHVNUploadSalesResponses
        {
            public int Status { get; set; }
            public string rMessage { get; set; }

        }


    
}

