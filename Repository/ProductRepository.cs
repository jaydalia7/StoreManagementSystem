using Microsoft.EntityFrameworkCore;
using StoreManagementSystem.Data;
using StoreManagementSystem.Models.Domains;
using StoreManagementSystem.Repository.IRepository;

namespace StoreManagementSystem.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly DataContext _dataContext;
        public ProductRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<int> AddProductData(Product product, CancellationToken cancellationToken = default)
        {
            await _dataContext.Products.AddAsync(product);
            await _dataContext.SaveChangesAsync(cancellationToken);
            return product.Id;
        }

        public async Task<bool> CheckProductByName(string productName, CancellationToken cancellationToken = default)
        {
            var product = await _dataContext.Products.FirstOrDefaultAsync(p => p.Name.Equals(productName));
            return product != null ? true : false;
        }

        public async Task<List<Product>> GetAllProducts(CancellationToken cancellationToken = default)
        {
            var products = await _dataContext.Products.Where(p => p.IsDeleted == false).ToListAsync(cancellationToken);
            return products;
        }

        public async Task<Product> GetProductById(int productId, CancellationToken cancellationToken = default)
        {
            var product = await _dataContext.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == productId && p.IsDeleted == false);
            return product;
        }

        public async Task<bool> RemoveProduct(Product product, CancellationToken cancellationToken = default)
        {
            product.IsDeleted = true;
            _dataContext.Products.Update(product);
            var count = await _dataContext.SaveChangesAsync(cancellationToken);
            return count > 0;
        }

        public async Task<Product> UpdateProductData(int productId, Product updateProduct, CancellationToken cancellationToken = default)
        {
            updateProduct.Id = productId;
            _dataContext.Products.Update(updateProduct);
            var count = await _dataContext.SaveChangesAsync(cancellationToken);
            return count > 0 ? updateProduct : null;
        }
    }
}
