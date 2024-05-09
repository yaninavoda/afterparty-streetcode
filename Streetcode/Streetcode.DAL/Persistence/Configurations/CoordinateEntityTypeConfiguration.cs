using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Streetcode.BLL.Entities.AdditionalContent.Coordinates;
using Streetcode.BLL.Entities.AdditionalContent.Coordinates.Types;

namespace Streetcode.DAL.Persistence.Configurations
{
    public class CoordinateEntityTypeConfiguration : IEntityTypeConfiguration<Coordinate>
    {
        public void Configure(EntityTypeBuilder<Coordinate> builder)
        {
            builder
                .HasDiscriminator<string>("CoordinateType")
                .HasValue<Coordinate>("coordinate_base")
                .HasValue<StreetcodeCoordinate>("coordinate_streetcode")
                .HasValue<ToponymCoordinate>("coordinate_toponym");
        }
    }
}
