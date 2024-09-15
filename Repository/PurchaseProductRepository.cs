using Microsoft.EntityFrameworkCore;
using StoreManagementSystem.Data;
using StoreManagementSystem.Models.Domains;
using StoreManagementSystem.Repository.IRepository;

namespace StoreManagementSystem.Repository
{
    public class PurchaseProductRepository : IPurchaseProductRepository
    {
        private readonly DataContext _dataContext;
        public PurchaseProductRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<int> AddPurchaseProductData(PurchaseProduct purchaseProduct, CancellationToken cancellationToken = default)
        {
            await _dataContext.PurchaseProducts.AddAsync(purchaseProduct);
            await _dataContext.SaveChangesAsync(cancellationToken);
            return purchaseProduct.Id;
        }

        public async Task<List<PurchaseProduct>> GetAllPurchaseProduct(CancellationToken cancellationToken = default)
        {
            var purchaseProduct = await _dataContext.PurchaseProducts.Where(pp => pp.IsDeleted == false).ToListAsync(cancellationToken);
            return purchaseProduct;
        }

        public async Task<PurchaseProduct> GetPurchaseProductById(int purchaseProductId, CancellationToken cancellationToken = default)
        {
            var purchaseProduct = await _dataContext.PurchaseProducts.AsNoTracking().FirstOrDefaultAsync(pp => pp.Id == purchaseProductId && !pp.IsDeleted, cancellationToken);
            return purchaseProduct;
        }

        public async Task<List<PurchaseProduct>> GetPurchaseProductByProductId(int productId, CancellationToken cancellationToken = default)
        {
            var purchaseProducts = await _dataContext.PurchaseProducts.AsNoTracking().Where(pp => pp.ProductId == productId && !pp.IsDeleted).ToListAsync(cancellationToken);
            return purchaseProducts;
        }

        public async Task<bool> RemovePurchaseProduct(PurchaseProduct removeProduct, int userId, CancellationToken cancellationToken = default)
        {
            removeProduct.UpdatedBy = userId;
            removeProduct.UpdatedOn = DateTime.Now;
            removeProduct.IsDeleted = true;
            _dataContext.PurchaseProducts.Update(removeProduct);
            var count = await _dataContext.SaveChangesAsync(cancellationToken);
            return count > 0;
        }

        public async Task<PurchaseProduct> UpdatePurchaseProduct(PurchaseProduct updatePurchaseProduct, int userId, CancellationToken cancellationToken = default)
        {
            updatePurchaseProduct.UpdatedBy = userId;
            updatePurchaseProduct.UpdatedOn = DateTime.Now;
            _dataContext.PurchaseProducts.Update(updatePurchaseProduct);
            var count = await _dataContext.SaveChangesAsync(cancellationToken);
            return count > 0 ? updatePurchaseProduct : null;
        }
    }
}
