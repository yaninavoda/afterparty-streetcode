﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Streetcode.BLL.Entities.News;

namespace Streetcode.DAL.Persistence.Configurations
{
    public class NewsEntityTypeConfiguration : IEntityTypeConfiguration<News>
    {
        public void Configure(EntityTypeBuilder<News> builder)
        {
            builder
                .HasOne(x => x.Image)
                .WithOne(x => x.News)
                .HasForeignKey<News>(x => x.ImageId);
        }
    }
}
