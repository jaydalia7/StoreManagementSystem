using StoreManagementSystem.Models.Domains;
using StoreManagementSystem.Models.ViewModels;
using System.Threading;

namespace StoreManagementSystem.Services.IServices
{
    public interface IProductService
    {
        Task<(ProductDisplayModel, StoreManagmentError)> CreateProduct(ProductAddModel product, CancellationToken cancellationToken = default);
        Task<(List<ProductDisplayModel>, StoreManagmentError)> GetAllProducts(CancellationToken cancellationToken = default);
        Task<(ProductDisplayModel, StoreManagmentError)> UpdateProduct(int productId, ProductAddModel productUpdateModel, CancellationToken cancellationToken = default);
        Task<(string, StoreManagmentError)> RemoveProduct(int productId, CancellationToken cancellationToken = default);
    }
}
