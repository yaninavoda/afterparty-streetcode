﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Streetcode.BLL.Entities.AdditionalContent;

namespace Streetcode.DAL.Persistence.Configurations
{
    public class StreetcodeTagIndexEntityTypeConfiguration : IEntityTypeConfiguration<StreetcodeTagIndex>
    {
        public void Configure(EntityTypeBuilder<StreetcodeTagIndex> builder)
        {
            builder
                .HasKey(nameof(StreetcodeTagIndex.StreetcodeId), nameof(StreetcodeTagIndex.TagId));
        }
    }
}
