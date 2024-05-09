﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Streetcode.BLL.Entities.Streetcode;

namespace Streetcode.DAL.Persistence.Configurations
{
    public class RelatedFigureEntityTypeConfiguration : IEntityTypeConfiguration<RelatedFigure>
    {
        public void Configure(EntityTypeBuilder<RelatedFigure> builder)
        {
            builder
                .HasKey(d => new { d.ObserverId, d.TargetId });

            builder
                .HasOne(d => d.Observer)
                .WithMany(d => d.Observers)
                .HasForeignKey(d => d.ObserverId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(d => d.Target)
                .WithMany(d => d.Targets)
                .HasForeignKey(d => d.TargetId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
