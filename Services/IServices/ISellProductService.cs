using StoreManagementSystem.Models.ViewModels;

namespace StoreManagementSystem.Services.IServices
{
    public interface ISellProductService
    {
        Task<(List<SellProductDisplayModel>, StoreManagmentError)> GetAllSellProduct(CancellationToken cancellationToken = default);
        Task<(SellProductDisplayModel, StoreManagmentError)> CreateSellProduct(SellProductAddModel sellProduct, CancellationToken cancellationToken = default);
        Task<(SellProductDisplayModel, StoreManagmentError)> UpdateSellProduct(int sellProductId, SellProductAddModel updateSellProduct, CancellationToken cancellationToken = default);
        Task<(string, StoreManagmentError)> RemoveSellProduct(int sellProductId, CancellationToken cancellationToken = default);
        Task<(List<StockProductDisplayModel>, StoreManagmentError)> ProductStock(CancellationToken cancellationToken = default);
    }
}
