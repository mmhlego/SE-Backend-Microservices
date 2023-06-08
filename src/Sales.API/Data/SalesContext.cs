using Microsoft.EntityFrameworkCore;
using Sales.API.Models;

namespace Sales.API.Data {
    public class SalesContext : DbContext {
        public SalesContext(DbContextOptions<SalesContext> options) : base(options) { }
        public DbSet<DiscountToken> DiscountTokens { get; set; } = null!;
        public DbSet<Order> Orders { get; set; } = null!;
        public DbSet<OrderItem> OrderItems { get; set; } = null!;
        public DbSet<Purchase> Purchases { get; set; } = null!;
        public DbSet<Sale> Sales { get; set; } = null!;
        public DbSet<SalePrice> SalePrices { get; set; } = null!;
    }
}