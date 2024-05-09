﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Streetcode.BLL.Entities.Toponyms;

namespace Streetcode.DAL.Persistence.Configurations
{
    public class ToponymEntityTypeConfiguration : IEntityTypeConfiguration<Toponym>
    {
        public void Configure(EntityTypeBuilder<Toponym> builder)
        {
            builder
                .HasOne(d => d.Coordinate)
                .WithOne(p => p.Toponym)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
