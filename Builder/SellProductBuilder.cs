using StoreManagementSystem.Models.Domains;
using StoreManagementSystem.Models.ViewModels;

namespace StoreManagementSystem.Builder
{
    public class SellProductBuilder
    {
        public static SellProduct Convert(SellProductAddModel sellProductAddModel, int createdBy)
        {
            var sellProduct = new SellProduct(sellProductAddModel.ProductId, sellProductAddModel.Quantity, false, createdBy);
            return sellProduct;
        }
    }
}
