using BusinessInfo.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessInfo.Persistence.Configurations
{
    public class RentalPartnerTypeConfiguration : BaseEntityTypeConfiguration<RentalPartner>
    {
        public override void Configure(EntityTypeBuilder<RentalPartner> builder)
        {
            builder.ToTable(nameof(RentalPartner));
            builder.Property(x => x.Name)
                .HasColumnType("varchar(255)")
                .IsRequired(false);

            builder.Property(x => x.Document)
                            .HasMaxLength(14)
                            .IsRequired();

            builder.Property(x => x.Active)
                            .HasDefaultValue(false)
                            .IsRequired();
           
            builder.HasOne(i => i.Issuer)
              .WithMany(r => r.RentalPartners)
              .HasForeignKey(v => v.IssuerId)
              .OnDelete(DeleteBehavior.Cascade);
        }

    }
}