using Dapper;
using Microsoft.Extensions.Logging;
using PHVN_WS_CORE.SERVICES.DbAccess;
using PHVN_WS_CORE.SERVICES.Models.GetChee;
using PHVN_WS_CORE.SHARED.Configurations;

namespace PHVN_WS_CORE.SERVICES.GetChee
{
    public class GetCheeService : IGetCheeService
    {
        private readonly ILogger<GetCheeService> logger;
        private readonly ISqlDataAccess db;

        public GetCheeService(ILogger<GetCheeService> logger, ISqlDataAccess db)
        {
            this.logger = logger;
            this.db = db;
        }
        public async Task<List<PHVNSalesData>> GetPHVNUpLoadSalesByMonth(DateTime pMonth)
        {
            try
            {
                
                DateTime firstDayOfMonth = new DateTime(pMonth.Year, pMonth.Month, 1);
                DateTime lastDayOfMonth = new DateTime(pMonth.Year, pMonth.Month, pMonth.Day-1);
                //DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);


                //var parameter = new { fromDate = firstDayOfMonth.Date.ToString("yyyy-MM-dd") , toDate = lastDayOfMonth.Date.ToString("yyyy-MM-dd") };
                ParameterPHVNUploadSalesModel parameter = new ParameterPHVNUploadSalesModel();
                parameter.fromDate = firstDayOfMonth.Date.ToString("yyyy-MM-dd");
                parameter.toDate = lastDayOfMonth.Date.ToString("yyyy-MM-dd");

                var result = await db.QueryAsync<PHVNSalesData, ParameterPHVNUploadSalesModel>(command: "usp_PHVNUploadSalesByMonth",
                                                                 parameter,
                                                                 connectionId: "DB27",
                                                                 commandType: System.Data.CommandType.StoredProcedure);
                return result.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<PHVNSalesPODData>> GetPHVNUpLoadPODSalesByMonth(DateTime pMonth)
        {
            try
            {

                DateTime firstDayOfMonth = new DateTime(pMonth.Year, pMonth.Month, 1);
                DateTime lastDayOfMonth = new DateTime(pMonth.Year, pMonth.Month, pMonth.Day - 1);
                //DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);


                //var parameter = new { fromDate = firstDayOfMonth.Date.ToString("yyyy-MM-dd") , toDate = lastDayOfMonth.Date.ToString("yyyy-MM-dd") };
                ParameterPHVNUploadPODSalesModel parameter = new ParameterPHVNUploadPODSalesModel();
                parameter.fromDate = firstDayOfMonth.Date.ToString("yyyy-MM-dd");
                parameter.toDate = lastDayOfMonth.Date.ToString("yyyy-MM-dd");

                var result = await db.QueryAsync<PHVNSalesPODData, ParameterPHVNUploadPODSalesModel>(command: "usp_PHVNUploadPODSalesByMonth",
                                                                 parameter,
                                                                 connectionId: "DB27",
                                                                 commandType: System.Data.CommandType.StoredProcedure);
                return result.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
