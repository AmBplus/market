using ECommerce.AppCore.Domain.Accounting;
using ECommerce.AppCore.Domain.Customization;
using ECommerce.AppCore.Domain.Discount;
using ECommerce.AppCore.Domain.Identity;
using ECommerce.AppCore.Domain.Inventory;
using ECommerce.AppCore.Domain.Order;
using ECommerce.AppCore.Domain.Payment;
using ECommerce.AppCore.Domain.Pricing;
using ECommerce.AppCore.Domain.Product;
using ECommerce.AppCore.Domain.Settings;
using ECommerce.AppCore.Domain.Shipping;
using ECommerce.AppCore.Domain.Vendor;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.AppCore.Data;

public class ECommerceDbContext : DbContext
{
    public ECommerceDbContext(DbContextOptions<ECommerceDbContext> options) : base(options) { }

    // Identity
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
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
    public DbSet<Vendor> Vendors { get; set; }
    // Settings
    public DbSet<Setting> Settings { get; set; }
    public DbSet<EnumValue> EnumValues { get; set; }
    // Customization
    public DbSet<SiteTheme> SiteThemes { get; set; }
    public DbSet<Slider> Sliders { get; set; }
    public DbSet<Widget> Widgets { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ECommerceDbContext).Assembly);
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
        foreach (var entry in ChangeTracker.Entries<Domain.Common.AuditableEntity>())
        {
            var now = DateTime.UtcNow;
            var userId = (long?)null;
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = now;
                    entry.Entity.CreatedBy = userId;
                    break;
                case EntityState.Modified:
                    entry.Entity.UpdatedAt = now;
                    entry.Entity.UpdatedBy = userId;
                    entry.Property(e => e.CreatedAt).IsModified = false;
                    entry.Property(e => e.CreatedBy).IsModified = false;
                    break;
            }
        }
        foreach (var entry in ChangeTracker.Entries<Domain.Common.SoftDeletableEntity>())
        {
            if (entry.State == EntityState.Deleted)
            {
                entry.State = EntityState.Modified;
                entry.Entity.IsDeleted = true;
                entry.Entity.DeletedAt = DateTime.UtcNow;
            }
        }
    }
}
