using Microsoft.EntityFrameworkCore;
using MarketplaceAPI.Models.Base;

namespace MarketplaceAPI.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany()
                .HasForeignKey(p => p.CategoryId);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Seller)
                .WithMany()
                .HasForeignKey(p => p.SellerId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascading delete

            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Product)
                .WithMany()
                .HasForeignKey(t => t.ProductId);

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Buyer)
                .WithMany()
                .HasForeignKey(t => t.BuyerId); // Ensure BuyerId matches the data type of UserId in the User entity

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Seller)
                .WithMany()
                .HasForeignKey(t => t.SellerId) // Update the foreign key property name
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascading delete
        }


    }
}
