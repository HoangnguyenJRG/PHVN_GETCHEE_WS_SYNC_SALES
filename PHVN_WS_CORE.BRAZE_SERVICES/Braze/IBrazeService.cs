using PHVN_WS_CORE.SERVICES.Models.Braze;

namespace PHVN_WS_CORE.SERVICES.Braze
{
    public interface IBrazeService
    {
        Task<List<PurchaseModel>> GetAllPurchaseAsync();
    }
}
