using StoreManagementSystem.Models.ViewModels;
using StoreManagementSystem.Models.Domains;

namespace StoreManagementSystem.Repository.IRepository
{
    public interface ISellProductRepository
    {
        Task<List<SellProduct>> GetAllSellProduct(CancellationToken cancellationToken = default);
        Task<int> AddSellProductData(SellProduct sellProduct, CancellationToken cancellationToken = default);
        Task<SellProduct> GetSellProductById(int sellProductId, CancellationToken cancellationToken = default);
        Task<SellProduct> UpdateSellProduct(SellProduct updateSellProduct, int userId, CancellationToken cancellationToken = default);
        Task<bool> RemoveSellProduct(SellProduct removeSellProduct, int userId, CancellationToken cancellationToken = default);
        Task<List<SellProduct>> GetSellProductByProductId(int productId, CancellationToken cancellationToken = default);
    }
}
