using StoreManagementSystem.Models.Domains;

namespace StoreManagementSystem.Repository.IRepository
{
    public interface IPurchaseProductRepository
    {
        Task<List<PurchaseProduct>> GetAllPurchaseProduct(CancellationToken cancellationToken = default);
        Task<int> AddPurchaseProductData(PurchaseProduct purchaseProduct, CancellationToken cancellationToken = default);
        Task<PurchaseProduct> GetPurchaseProductById(int Id, CancellationToken cancellationToken = default);
        Task<PurchaseProduct> UpdatePurchaseProduct(PurchaseProduct updatePurchaseProduct, int userId, CancellationToken cancellationToken = default);
        Task<bool> RemovePurchaseProduct(PurchaseProduct removeProduct, int userId, CancellationToken cancellationToken = default);
        Task<List<PurchaseProduct>> GetPurchaseProductByProductId(int productId, CancellationToken cancellationToken = default);
    }
}
