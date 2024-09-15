namespace StoreManagementSystem.Models.ViewModels
{
    public class ProductDisplayModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public int Price { get; set; }

        public class NullProductDisplayModel : ProductDisplayModel { }
        public class NullProductListDisplayModel : List<ProductDisplayModel> { }
    }
}
