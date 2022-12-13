using lifeEcommerce.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace lifeEcommerce.Data
{
    public class LifeEcommerceDbContext : DbContext
    {
        public LifeEcommerceDbContext(DbContextOptions<LifeEcommerceDbContext> options) : base(options)
        {

        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }
        public DbSet<ShoppingCard> ShoppingCards { get; set; }
        public DbSet<OrderData> OrderData { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ShoppingCard>(entity =>
            {
                entity.HasIndex(x => new { x.UserId, x.ProductId}).IsUnique();
            });
        }
    }
}
