using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Farmers.DataModel
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Farmer> Farmers { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Courier> Couriers { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Shipping> Shippings { get; set; }
        public DbSet<Invoice> Invoices { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed Roles
            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = "1", Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole { Id = "2", Name = "Farmer", NormalizedName = "FARMER" },
                new IdentityRole { Id = "3", Name = "Customer", NormalizedName = "CUSTOMER" },
                new IdentityRole { Id = "4", Name = "Courier", NormalizedName = "COURIER" }
            );

            // Farmer-ApplicationUser Relationship
            modelBuilder.Entity<Farmer>()
                .HasOne(f => f.User)
                .WithOne()
                .HasForeignKey<Farmer>(f => f.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Customer-ApplicationUser Relationship
            modelBuilder.Entity<Customer>()
                .HasOne(c => c.User)
                .WithOne()
                .HasForeignKey<Customer>(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Product-ProductCategory Relationship
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // Product-Farmer Relationship
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Farmer)
                .WithMany(f => f.Products)
                .HasForeignKey(p => p.FarmerId)
                .OnDelete(DeleteBehavior.Cascade);

            // Order-Customer Relationship
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Customer)
                .WithMany(c => c.Orders)
                .HasForeignKey(o => o.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            // Order-OrderDetails Relationship
            modelBuilder.Entity<Order>()
                .HasMany(o => o.OrderDetails)
                .WithOne(od => od.Order)
                .HasForeignKey(od => od.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            // Order-Shipping Relationship
            modelBuilder.Entity<Shipping>()
                .HasOne(s => s.Order)
                .WithOne(o => o.Shipping)
                .HasForeignKey<Shipping>(s => s.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            // Shipping-Courier Relationship
            modelBuilder.Entity<Shipping>()
                .HasOne(s => s.Courier)
                .WithMany(c => c.Shipments)
                .HasForeignKey(s => s.CourierId)
                .OnDelete(DeleteBehavior.Cascade);

            // OrderDetail-Product Relationship
            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Product)
                .WithMany(p => p.OrderDetails)
                .HasForeignKey(od => od.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            // Invoice-Order Relationship
            modelBuilder.Entity<Invoice>()
                .HasOne(i => i.Order)
                .WithOne(o => o.Invoice)
                .HasForeignKey<Invoice>(i => i.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            // Invoice-Customer Relationship
            modelBuilder.Entity<Invoice>()
                .HasOne(i => i.Customer)
                .WithMany(c => c.Invoices)
                .HasForeignKey(i => i.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Invoice-Farmer Relationship
            modelBuilder.Entity<Invoice>()
                .HasOne(i => i.Farmer)
                .WithMany(f => f.Invoices)
                .HasForeignKey(i => i.FarmerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
