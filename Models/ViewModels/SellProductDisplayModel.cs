namespace StoreManagementSystem.Models.ViewModels
{
    public class SellProductDisplayModel
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public DateTime Date { get; set; }
        //public int CreatedBy { get; set; }
        //public DateTime CreatedOn { get; set; }
        //public int? UpdatedBy { get; set; }
        //public DateTime? UpdatedOn { get; set; }
    }
    public class NullSellProductDisplayModel : SellProductDisplayModel { }
    public class NullSellProductListDisplayModel : List<SellProductDisplayModel> { }
}
