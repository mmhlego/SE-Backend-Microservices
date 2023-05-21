using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Products.API.Models;
using SharedModels;

namespace Products.API.Data
{
    public class ProductsContext : DbContext
    {
        public ProductsContext(DbContextOptions<ProductsContext> options)
            : base(options)
        {
        }

        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<ProductRequest> ProductRequests { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Field> Fields { get; set; }
        public DbSet<Subcategory> Subcategories { get; set; }
        public DbSet<Product> Product { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductImage>()
                .HasKey(pi => pi.Id);

            modelBuilder.Entity<ProductRequest>()
                .HasKey(pr => pr.Id);

            modelBuilder.Entity<ProductRequest>()
                .HasKey(pr => pr.Id);

            modelBuilder.Entity<Category>()
                .HasKey(pr => pr.Id);

            modelBuilder.Entity<Field>()
                .HasKey(pr => pr.Id);

            modelBuilder.Entity<Subcategory>()
                .HasKey(pr => pr.Id);



        }
    }
}