using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreManagementSystem.Models.Domains
{
    public class PurchaseProduct
    {
        [Key]
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public DateTime Date { get; set; }
        public bool IsDeleted { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; }
        [ForeignKey("CreatedBy")]
        public User User { get; set; }

        [ForeignKey("UpdatedBy")]
        public User UpdatedUser { get; set; }

        public PurchaseProduct(int productId, int quantity, bool isDeleted, int createdBy)
        {
            ProductId = productId;
            Quantity = quantity;
            Date = DateTime.Now;
            CreatedBy = createdBy;
            CreatedOn = DateTime.Now;
            IsDeleted = isDeleted;
        }
    }
}
