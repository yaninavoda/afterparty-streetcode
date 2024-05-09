﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Streetcode.BLL.Entities.Partners;

namespace Streetcode.DAL.Persistence.Configurations
{
    public class PartnerEntityTypeConfiguration : IEntityTypeConfiguration<Partner>
    {
        public void Configure(EntityTypeBuilder<Partner> builder)
        {
            builder
                .HasMany(d => d.PartnerSourceLinks)
                .WithOne(p => p.Partner)
                .HasForeignKey(d => d.PartnerId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(p => p.IsKeyPartner)
                .HasDefaultValue("false");
        }
    }
}
