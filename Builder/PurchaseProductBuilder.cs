using StoreManagementSystem.Models.Domains;
using StoreManagementSystem.Models.ViewModels;

namespace StoreManagementSystem.Builder
{
    public class PurchaseProductBuilder
    {
        public static PurchaseProduct Convert(PurchaseProductAddModel purchaseProductAdd, int createdBy)
        {
            var purchaseProduct = new PurchaseProduct(purchaseProductAdd.ProductId, purchaseProductAdd.Quantity, false, createdBy);
            return purchaseProduct;
        }
    }
}
