namespace StoreManagementSystem.Models.ViewModels
{
    public class PurchaseProductDisplayModel
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public DateTime Date { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
    public class NullPurchaseProductDisplayModel : PurchaseProductDisplayModel { }
    public class NullPurchaseProductListDisplayModel : List<PurchaseProductDisplayModel> { }
}
