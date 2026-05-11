using AppCore.Domains.Entities.Shop.ProductAgg;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppCore.Data.Mappings.ProductAgg
{
    public class CouponMap : IEntityTypeConfiguration<Coupon>
    {
        public void Configure(EntityTypeBuilder<Coupon> builder)
        {
            builder.ToTable("Coupons", "Shop");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Code)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.Type)
                .HasConversion<int>()
                .IsRequired();

            builder.Property(x => x.Value)
                .IsRequired();

            builder.Property(x => x.MinOrderAmount)
                .IsRequired();

            builder.Property(x => x.MaxDiscountAmount)
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.UsageLimit);

            builder.Property(x => x.PerUserLimit);

            builder.Property(x => x.IsActive)
                .IsRequired();

            builder.Property(x => x.IsDelete)
                .IsRequired();

            builder.Property(x => x.StartsAt)
                .IsRequired();

            builder.Property(x => x.ExpiresAt)
                .IsRequired();

            builder.HasIndex(x => x.Code).IsUnique();
        }
    }
}
