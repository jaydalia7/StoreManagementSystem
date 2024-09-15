using Microsoft.EntityFrameworkCore;
using StoreManagementSystem.Models;
using StoreManagementSystem.Models.Domains;

namespace StoreManagementSystem.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasIndex(e => e.EmailAddress).IsUnique(true);
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<PurchaseProduct> PurchaseProducts { get; set; }
        public DbSet<SellProduct> SellProducts { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
