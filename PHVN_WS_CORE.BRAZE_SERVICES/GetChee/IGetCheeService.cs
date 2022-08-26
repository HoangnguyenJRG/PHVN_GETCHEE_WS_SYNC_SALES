using Microsoft.Extensions.Logging;
using PHVN_WS_CORE.SERVICES.DbAccess;
using PHVN_WS_CORE.SERVICES.Models.GetChee;
using PHVN_WS_CORE.SHARED.Configurations;

namespace PHVN_WS_CORE.SERVICES.GetChee
{
    public interface IGetCheeService
    {
        Task<List<PHVNSalesData>> GetPHVNUpLoadSalesByMonth(DateTime pMonth);
        Task<List<PHVNSalesPODData>> GetPHVNUpLoadPODSalesByMonth(DateTime pMonth);
    }
}
