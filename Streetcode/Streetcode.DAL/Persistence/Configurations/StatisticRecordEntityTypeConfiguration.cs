﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Streetcode.BLL.Entities.Analytics;

namespace Streetcode.DAL.Persistence.Configurations
{
    public class StatisticRecordEntityTypeConfiguration : IEntityTypeConfiguration<StatisticRecord>
    {
        public void Configure(EntityTypeBuilder<StatisticRecord> builder)
        {
            builder
              .HasOne(x => x.StreetcodeCoordinate)
              .WithOne(x => x.StatisticRecord)
              .HasForeignKey<StatisticRecord>(x => x.StreetcodeCoordinateId);
        }
    }
}
