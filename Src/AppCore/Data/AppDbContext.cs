using AppCore.Domains.Accounting;
using AppCore.Domains.Common;
using AppCore.Domains.Customization;
using AppCore.Domains.Discount;
using AppCore.Domains.Identity;
using AppCore.Domains.Inventory;
using AppCore.Domains.Order;
using AppCore.Domains.Payment;
using AppCore.Domains.Pricing;
using AppCore.Domains.Product;
using AppCore.Domains.Settings;
using AppCore.Domains.Shipping;
using AppCore.Domains.Shop;
using AppCore.Domains.Vendor;
using Microsoft.EntityFrameworkCore;

namespace AppCore.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // Identity
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<Permission> Permissions { get; set; }
    public DbSet<PermissionType> PermissionTypes { get; set; }
    public DbSet<UserRolePermission> UserRolePermissions { get; set; }

    // Product
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductVariant> ProductVariants { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Brand> Brands { get; set; }
    public DbSet<ProductCategory> ProductCategories { get; set; }
    public DbSet<ProductAttribute> ProductAttributes { get; set; }
    public DbSet<ProductImage> ProductImages { get; set; }

    // Pricing
    public DbSet<Price> Prices { get; set; }
    public DbSet<Currency> Currencies { get; set; }

    // Inventory
    public DbSet<Warehouse> Warehouses { get; set; }
    public DbSet<StockLedger> StockLedgers { get; set; }
    public DbSet<StockReservation> StockReservations { get; set; }

    // Order
    public DbSet<Cart> Carts { get; set; }
    public DbSet<CartItem> CartItems { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<OrderStatusHistory> OrderStatusHistories { get; set; }

    // Discount
    public DbSet<Discount> Discounts { get; set; }
    public DbSet<ProductDiscount> ProductDiscounts { get; set; }
    public DbSet<CategoryDiscount> CategoryDiscounts { get; set; }
    public DbSet<OrderDiscount> OrderDiscounts { get; set; }

    // Payment
    public DbSet<PaymentGateway> PaymentGateways { get; set; }
    public DbSet<Payment> Payments { get; set; }

    // Shipping
    public DbSet<Carrier> Carriers { get; set; }
    public DbSet<DeliveryMethod> DeliveryMethods { get; set; }
    public DbSet<OrderDelivery> OrderDeliveries { get; set; }

    // Accounting
    public DbSet<Account> Accounts { get; set; }
    public DbSet<Transaction> Transactions { get; set; }

    // Vendor
    public DbSet<Domains.Vendor.Vendor> Vendors { get; set; }

    // Settings
    public DbSet<Setting> Settings { get; set; }
    public DbSet<EnumValue> EnumValues { get; set; }

    // Customization
    public DbSet<SiteTheme> SiteThemes { get; set; }
    public DbSet<Slider> Sliders { get; set; }
    public DbSet<Widget> Widgets { get; set; }

    // Shop (market-specific)
    public DbSet<Color> Colors { get; set; }
    public DbSet<Guarantee> Guarantees { get; set; }
    public DbSet<ProductTag> ProductTags { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }

    public override int SaveChanges()
    {
        SetAuditFields();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        SetAuditFields();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void SetAuditFields()
    {
        var now = DateTime.UtcNow;
        foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = now;
                    break;
                case EntityState.Modified:
                    entry.Entity.UpdatedAt = now;
                    entry.Property(e => e.CreatedAt).IsModified = false;
                    entry.Property(e => e.CreatedBy).IsModified = false;
                    break;
            }
        }
        foreach (var entry in ChangeTracker.Entries<SoftDeletableEntity>())
        {
            if (entry.State == EntityState.Deleted)
            {
                entry.State = EntityState.Modified;
                entry.Entity.IsDeleted = true;
                entry.Entity.DeletedAt = now;
            }
        }
    }
}
