using Microsoft.Extensions.Logging;
using PHVN_WS_CORE.SERVICES.DbAccess;
using PHVN_WS_CORE.SERVICES.Models.Braze;
using PHVN_WS_CORE.SHARED.Configurations;

namespace PHVN_WS_CORE.SERVICES.Braze
{
    public class BrazeService : IBrazeService
    {
        private readonly ILogger<BrazeService> logger;
        private readonly ISqlDataAccess db;

        public BrazeService(ILogger<BrazeService> logger, ISqlDataAccess db)
        {
            this.logger = logger; 
            this.db = db;
        }
        public async Task<List<PurchaseModel>> GetAllPurchaseAsync()
        {
            try
            {
                var result =  await db.QueryAsync<PurchaseModel>(command: "sp_BrazeAPI_purchase_get",
                                                                 connectionId: "DB38",
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
