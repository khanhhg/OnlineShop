using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using WebBanHangOnline.Models.EF;

namespace WebBanHangOnline.Models
{
    public class ShopOnlineDBContext : DbContext
    {
        public ShopOnlineDBContext(DbContextOptions<ShopOnlineDBContext> options)
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
        public DbSet<Posts> Posts { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<ProductCategory> ProductCategory { get; set; }
        public DbSet<ProductImage> ProductImage { get; set; }
        public DbSet<Subscribe> Subscribe { get; set; }
        public DbSet<SystemSetting> SystemSetting { get; set; }   

    }
}
