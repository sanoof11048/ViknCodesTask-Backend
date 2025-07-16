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
        public DbSet<SubVariant> VariantOptions { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<ProductStock> ProductStocks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ProductVariant>()
                .HasOne(pv => pv.Product)
                .WithMany(p => p.Variants)
                .HasForeignKey(pv => pv.ProductId);

            modelBuilder.Entity<SubVariant>()
                .HasOne(sv => sv.ProductVariant)
                .WithMany(pv => pv.Options)
                .HasForeignKey(sv => sv.ProductVariantId);

            modelBuilder.Entity<Product>()
               .HasIndex(p => p.ProductCode)
               .IsUnique();

            modelBuilder.Entity<ProductVariant>()
                .HasIndex(v => new { v.ProductId, v.VariantName })
                .IsUnique();

            modelBuilder.Entity<SubVariant>()
                .HasIndex(o => new { o.ProductVariantId, o.Value })
                .IsUnique();

            modelBuilder.Entity<ProductStock>()
                .HasIndex(s => new { s.ProductId, s.VariantKey })
                .IsUnique();

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
