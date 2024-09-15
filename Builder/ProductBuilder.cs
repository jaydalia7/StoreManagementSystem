using StoreManagementSystem.Models.Domains;
using StoreManagementSystem.Models.ViewModels;

namespace StoreManagementSystem.Builder
{
    public class ProductBuilder
    {
        public static Product Convert(ProductAddModel productAdd)
        {
            var product = new Product(productAdd.Name, productAdd.Description, productAdd.Price, false);
            return product;
        }
    }
}
