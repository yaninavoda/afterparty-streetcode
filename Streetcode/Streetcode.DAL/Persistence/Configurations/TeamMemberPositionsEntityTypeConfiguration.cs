﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Streetcode.BLL.Entities.Team;

namespace Streetcode.DAL.Persistence.Configurations
{
    public class TeamMemberPositionsEntityTypeConfiguration : IEntityTypeConfiguration<TeamMemberPositions>
    {
        public void Configure(EntityTypeBuilder<TeamMemberPositions> builder)
        {
            builder
                .HasKey(nameof(TeamMemberPositions.TeamMemberId), nameof(TeamMemberPositions.PositionsId));
        }
    }
}
