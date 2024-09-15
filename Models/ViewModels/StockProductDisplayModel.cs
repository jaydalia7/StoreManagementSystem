namespace StoreManagementSystem.Models.ViewModels
{
    public class StockProductDisplayModel
    {
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public int QuantityInStock { get; set; }
    }
    public class NullStockProductListDisplayModel : List<StockProductDisplayModel> { }
}
