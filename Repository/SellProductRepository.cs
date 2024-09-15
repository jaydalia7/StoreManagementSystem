using Microsoft.EntityFrameworkCore;
using StoreManagementSystem.Data;
using StoreManagementSystem.Models.Domains;
using StoreManagementSystem.Repository.IRepository;

namespace StoreManagementSystem.Repository
{
    public class SellProductRepository : ISellProductRepository
    {
        private readonly DataContext _dataContext;
        public SellProductRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<int> AddSellProductData(SellProduct sellProduct, CancellationToken cancellationToken = default)
        {
            await _dataContext.SellProducts.AddAsync(sellProduct);
            await _dataContext.SaveChangesAsync(cancellationToken);
            return sellProduct.Id;
        }

        public async Task<List<SellProduct>> GetAllSellProduct(CancellationToken cancellationToken = default)
        {
            var sellProducts = await _dataContext.SellProducts.Where(sp => !sp.IsDeleted).ToListAsync(cancellationToken);
            return sellProducts;
        }

        public async Task<SellProduct> GetSellProductById(int sellProductId, CancellationToken cancellationToken = default)
        {
            var sellProduct = await _dataContext.SellProducts.Where(sp => sp.Id == sellProductId && !sp.IsDeleted).FirstOrDefaultAsync(cancellationToken);
            return sellProduct;
        }

        public async Task<List<SellProduct>> GetSellProductByProductId(int productId, CancellationToken cancellationToken = default)
        {
            var sellProducts = await _dataContext.SellProducts.AsNoTracking().Where(sp => sp.ProductId == productId && !sp.IsDeleted).ToListAsync(cancellationToken);
            return sellProducts;
        }

        public async Task<bool> RemoveSellProduct(SellProduct removeSellProduct, int userId, CancellationToken cancellationToken = default)
        {
            removeSellProduct.UpdatedBy = userId;
            removeSellProduct.UpdatedOn = DateTime.Now;
            removeSellProduct.IsDeleted = true;
            _dataContext.SellProducts.Update(removeSellProduct);
            var count = await _dataContext.SaveChangesAsync(cancellationToken);
            return count > 0;
        }

        public async Task<SellProduct> UpdateSellProduct(SellProduct updateSellProduct, int userId, CancellationToken cancellationToken = default)
        {
            updateSellProduct.UpdatedBy = userId;
            updateSellProduct.UpdatedOn = DateTime.Now;
            _dataContext.SellProducts.Update(updateSellProduct);
            var count = await _dataContext.SaveChangesAsync(cancellationToken);
            return count > 0 ? updateSellProduct : null;
        }


    }
}
