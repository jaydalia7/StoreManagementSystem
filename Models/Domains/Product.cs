using System.ComponentModel.DataAnnotations;

namespace StoreManagementSystem.Models.Domains
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public int Price { get; set; }
        public bool IsDeleted { get; set; }
        public ICollection<PurchaseProduct> PurchaseProducts { get; set; }
        public ICollection<SellProduct> SellProducts { get; set; }
        public Product(string name, string description, int price, bool isDeleted)
        {
            Name = name;
            Description = description;
            Price = price;
            IsDeleted = isDeleted;
        }
    }
}
