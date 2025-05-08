using GenericStockManagement.Models;
using GenericStockManagement.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using DbContext = Microsoft.EntityFrameworkCore.DbContext;

namespace GenericStockManagement
{
    public class StockDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<User> Users { get; set; }
        public StockDbContext(DbContextOptions<StockDbContext> options) : base(options) { }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .HasOne<Category>()
                .WithMany()
                .HasForeignKey(f => f.CategoryId);
            base.OnModelCreating(modelBuilder);
        }



    }
}