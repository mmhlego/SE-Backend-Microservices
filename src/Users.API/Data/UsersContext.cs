using Microsoft.EntityFrameworkCore;
using SharedModels;
namespace Users.API.Data {
    public class UsersContext : DbContext
    {
        public UsersContext(DbContextOptions<UsersContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerAddress> CustomerAddresses { get; set; }
        public DbSet<Seller> Sellers { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<User>()
        //        .HasMany(u => u.Customers)
        //        .WithOne(c => c.User)
        //        .HasForeignKey(c => c.UserId);

        //    modelBuilder.Entity<User>()
        //        .HasMany(u => u.Sellers)
        //        .WithOne(s => s.User)
        //        .HasForeignKey(s => s.UserId);

        //    modelBuilder.Entity<Customer>()
        //        .HasMany(c => c.CustomerAddresses)
        //        .WithOne(a => a.Customer)
        //        .HasForeignKey(a => a.CustomerId);
        //}
    }
}