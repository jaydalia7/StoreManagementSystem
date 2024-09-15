using StoreManagementSystem.Models.Domains;
using System.Threading;

namespace StoreManagementSystem.Repository.IRepository
{
    public interface IProductRepository
    {
        Task<bool> CheckProductByName(string productName, CancellationToken cancellationToken = default);
        Task<int> AddProductData(Product product, CancellationToken cancellationToken = default);
        Task<Product> GetProductById(int productId, CancellationToken cancellationToken = default);
        Task<List<Product>> GetAllProducts(CancellationToken cancellationToken = default);
        Task<Product> UpdateProductData(int productId, Product updateProduct, CancellationToken cancellationToken = default);
        Task<bool> RemoveProduct(Product product, CancellationToken cancellationToken = default);
    }
}
