using StoreManagementSystem.Models.ViewModels;

namespace StoreManagementSystem.Services.IServices
{
    public interface IPurchaseProductService
    {
        Task<(List<PurchaseProductDisplayModel>, StoreManagmentError)> GetAllPurchaseProduct(CancellationToken cancellationToken = default);
        Task<(PurchaseProductDisplayModel, StoreManagmentError)> CreatePurchaseProduct(PurchaseProductAddModel purchaseProduct, CancellationToken cancellationToken = default);
        Task<(PurchaseProductDisplayModel, StoreManagmentError)> UpdatePurchaseProduct(int purchaseProductId, PurchaseProductAddModel updatePurchaseProduct, CancellationToken cancellationToken = default);
        Task<(string, StoreManagmentError)> RemovePurchaseProduct(int purchaseProductId, CancellationToken cancellationToken = default);
    }
}
