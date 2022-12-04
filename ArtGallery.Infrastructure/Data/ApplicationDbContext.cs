using ArtGallery.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ArtGallery.Infrastructure.Data
{
    //public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<CartPainting>()
                .HasKey(cp => new { cp.CartId,cp.PaintingId });

            builder.Entity<OrderPainting>()
                .HasKey(op => new { op.OrderId,op.PaintingId });

            builder.Entity<ApplicationUser>()
                .HasOne(e => e.Cart)
                .WithOne(e => e.User)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(builder);
        }


        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartPainting> CartPaintings { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderPainting> OrderPainting { get; set; }
        public DbSet<Painting> Painting { get; set; }
    }
}