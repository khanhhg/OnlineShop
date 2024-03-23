using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebBanHangOnline.Data.Models.EF;

namespace WebBanHangOnline.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Adv> Adv { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Contact> Contact { get; set; }
        public DbSet<Counter> Counter { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderDetail> OrderDetail { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<ProductCategory> ProductCategory { get; set; }
        public DbSet<ProductImage> ProductImage { get; set; }
        public DbSet<Subscribe> Subscribe { get; set; }
        public DbSet<SystemSetting> SystemSetting { get; set; }
        public DbSet<UserProfile> UserProfile { get; set; }
        public DbSet<Comments> Comments { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductCategory>()
                        .HasMany(e => e.Products)
                        .WithOne(e => e.ProductCategory)
                        .HasForeignKey(e => e.ProductCategoryId)
                        .HasPrincipalKey(e => e.ProductCategoryId);

            modelBuilder.Entity<Product>()
                       .HasMany(e => e.ProductImages)
                       .WithOne(e => e.Product)
                       .HasForeignKey(e => e.ProductId)
                       .HasPrincipalKey(e => e.ProductId);

            modelBuilder.Entity<Product>()
                        .HasMany(e => e.Orders)
                        .WithMany(e => e.Products)
                        .UsingEntity<OrderDetail>(
                        l => l.HasOne<Order>().WithMany().HasForeignKey(e => e.OrderId),
                        r => r.HasOne<Product>().WithMany().HasForeignKey(e => e.ProductId));        
            base.OnModelCreating(modelBuilder);
        }
    }
}