using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Streetcode.BLL.Entities.Streetcode.TextContent;

namespace Streetcode.DAL.Persistence.Configurations
{
    public class RelatedTermEntityTypeConfiguration : IEntityTypeConfiguration<RelatedTerm>
    {
        public void Configure(EntityTypeBuilder<RelatedTerm> builder)
        {
            builder
                .HasOne(rt => rt.Term)
                .WithMany(t => t.RelatedTerms)
                .HasForeignKey(rt => rt.TermId);
        }
    }
}
