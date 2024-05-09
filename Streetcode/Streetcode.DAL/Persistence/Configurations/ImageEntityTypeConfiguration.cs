﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Streetcode.BLL.Entities.Media.Images;
using Streetcode.BLL.Entities.Partners;

namespace Streetcode.DAL.Persistence.Configurations
{
    public class EntityTypeConfiguration : IEntityTypeConfiguration<Image>
    {
        public void Configure(EntityTypeBuilder<Image> builder)
        {
            builder
                .HasOne(d => d.Art)
                .WithOne(a => a.Image)
                .HasForeignKey<Art>(a => a.ImageId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(im => im.ImageDetails)
                .WithOne(info => info.Image)
                .HasForeignKey<ImageDetails>(a => a.ImageId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(d => d.Partner)
                .WithOne(p => p.Logo)
                .HasForeignKey<Partner>(d => d.LogoId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasMany(d => d.Facts)
                .WithOne(p => p.Image)
                .HasForeignKey(d => d.ImageId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasMany(i => i.SourceLinkCategories)
                .WithOne(s => s.Image)
                .HasForeignKey(d => d.ImageId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
