using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using ViknCodesTask.Common;
using ViknCodesTask.Models;
using ViknCodesTask.Models.ProductModels;

namespace ViknCodesTask.Data
{
    public class AppDbContext : DbContext
    {
        private readonly IHttpContextAccessor? _httpContextAccessor;
        public AppDbContext(IHttpContextAccessor httpContextAccessor, DbContextOptions<AppDbContext> options) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductVariant> ProductVariants { get; set; }
        public DbSet<ProductSubVariant> ProductSubVariants { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<ProductStock> ProductStocks { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>()
                .HasMany(p => p.Variants)
                    .WithOne(v => v.Product)
                    .HasForeignKey(v => v.ProductId)
                    .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProductVariant>()
                .HasMany(v => v.SubVariants)
                    .WithOne(sv => sv.Variant)
                    .HasForeignKey(sv => sv.ProductVariantId)
                    .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Product>()
               .HasIndex(p => p.ProductCode)
               .IsUnique();

            modelBuilder.Entity<ProductVariant>()
                .HasIndex(v => new { v.ProductId, v.VariantName })
                .IsUnique();

            modelBuilder.Entity<ProductStock>(entity =>
            {
                entity.HasKey(s => s.Id);
                entity.Property(s => s.Quantity).IsRequired();

                entity.HasOne(s => s.Variant)
                    .WithMany(v => v.ProductStock)
                    .HasForeignKey(s => s.ProductVariantId)
                    .OnDelete(DeleteBehavior.Cascade);

                modelBuilder.Entity<Product>()
    .HasMany(p => p.ProductCategories)
    .WithMany(pc => pc.Products)
    .UsingEntity<Dictionary<string, object>>(
        "ProductCategoryMappings",
        j => j.HasOne<ProductCategory>().WithMany().HasForeignKey("CategoryId"),
        j => j.HasOne<Product>().WithMany().HasForeignKey("ProductId"),
        j =>
        {
            j.ToTable("ProductCategoryMappings");
        });
            });

        }
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries<BaseEntity>();
            var userId = GetCurrentUserId();

            foreach (var entry in entries)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedAt = DateTimeOffset.UtcNow;
                        entry.Entity.CreatedBy = userId;
                        entry.Entity.UpdatedAt = DateTimeOffset.UtcNow;
                        entry.Entity.UpdatedBy = userId;
                        break;

                    case EntityState.Modified:
                        entry.Entity.UpdatedAt = DateTimeOffset.UtcNow;
                        entry.Entity.UpdatedBy = userId;
                        break;
                }
            }
            return await base.SaveChangesAsync(cancellationToken);
        }

        private Guid GetCurrentUserId()
        {
            var user = _httpContextAccessor?.HttpContext?.User;
            var userIdClaim = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return Guid.TryParse(userIdClaim, out var userId) ? userId : Guid.Empty;
        }
    }
}
