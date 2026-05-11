using AppCore.Domains.Entities.Shop.ProductAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppCore.Data.Mappings.ProductAgg
{
    public class GuaranteeMap : IEntityTypeConfiguration<Guarantee>
    {
        public void Configure(EntityTypeBuilder<Guarantee> builder)
        {
            builder.ToTable("Guarantees", "Shop");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(300);

            builder.Property(x => x.IsDelete)
                .IsRequired();

            builder.Property(x => x.IsActive)
                .IsRequired();

            builder.Property(x => x.IsUpdate)
                .IsRequired();

            builder.Property(x => x.UpdateAt);

            builder.Property(x => x.CreateAt);

            builder.Property(x => x.UpdateBy);

            builder.Property(x => x.CreatedBy);
        }
    }
}
