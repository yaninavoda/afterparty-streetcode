﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Streetcode.BLL.Entities.Sources;

namespace Streetcode.DAL.Persistence.Configurations
{
    public class SourceLinkCategoryEntityTypeConfiguration : IEntityTypeConfiguration<SourceLinkCategory>
    {
        public void Configure(EntityTypeBuilder<SourceLinkCategory> builder)
        {
            builder
                .HasMany(d => d.StreetcodeCategoryContents)
                .WithOne(p => p.SourceLinkCategory)
                .HasForeignKey(d => d.SourceLinkCategoryId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
