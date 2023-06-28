using Microsoft.EntityFrameworkCore;
using SharedModels;
using Users.API.Models;

namespace Users.API.Data {
    public class UsersContext : DbContext {
        public UsersContext(DbContextOptions<UsersContext> options) : base(options) { }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Customer> Customers { get; set; } = null!;
        public DbSet<CustomerAddress> CustomerAddresses { get; set; } = null!;
        public DbSet<Seller> Sellers { get; set; } = null!;
        public DbSet<Verify> verifies { get; set; } = null!;
    }
}