using AppCore.Domains.Entities.ID;
using AppCore.Domains.Entities.Shop.ProductAgg;
using AppCore.Domains.Entities.Shop.ProductAggregate;
using Microsoft.EntityFrameworkCore;

namespace AppCore.Data;

public class AppDbContext : DbContext
{
    public DbSet<AppUser> Users { get; set; }
    public DbSet<AppRole> Roles { get; set; }
    public DbSet<AppUserDomainRole> UserDomainRoles { get; set; }
    public DbSet<PermissionType> PermissionTypes { get; set; }
    public DbSet<Permission> Permissions { get; set; }
    public DbSet<UserRolePermission> UserRolePermissions { get; set; }

    // Shop
    public DbSet<ProductTag> ProductTags { get; set; }
    public DbSet<ProductBrand> ProductBrands { get; set; }
    public DbSet<Guarantee> Guarantees { get; set; }
    public DbSet<Color> Colors { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
