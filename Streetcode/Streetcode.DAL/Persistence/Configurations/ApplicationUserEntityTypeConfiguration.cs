using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Streetcode.DAL.Entities.Users;

namespace Streetcode.DAL.Persistence.Configurations;

public class ApplicationUserEntityTypeConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder
            .HasMany(user => user.RefreshTokens)
            .WithOne(rt => rt.ApplicationUser)
            .HasForeignKey(rt => rt.ApplicationUserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}