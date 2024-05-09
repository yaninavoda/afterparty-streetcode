using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Streetcode.BLL.Entities.AdditionalContent;
using Streetcode.BLL.Entities.AdditionalContent.Coordinates.Types;
using Streetcode.BLL.Entities.Analytics;
using Streetcode.BLL.Entities.Media;
using Streetcode.BLL.Entities.Media.Images;
using Streetcode.BLL.Entities.Partners;
using Streetcode.BLL.Entities.Sources;
using Streetcode.BLL.Entities.Streetcode.TextContent;
using Streetcode.BLL.Entities.Timeline;
using Streetcode.BLL.Entities.Toponyms;
using Streetcode.BLL.Entities.Transactions;
using Streetcode.BLL.Enums;

namespace Streetcode.BLL.Entities.Streetcode
{
    [Table("streetcodes", Schema = "streetcode")]
    [Index(nameof(TransliterationUrl), IsUnique = true)]
    [Index(nameof(Index), IsUnique = true)]
    public class StreetcodeContent
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int Index { get; set; }

        [MaxLength(650)]
        public string? Teaser { get; set; }

        [Required]
        [MaxLength(50)]
        public string? DateString { get; set; }

        [MaxLength(50)]
        public string? Alias { get; set; }

        public StreetcodeStatus Status { get; set; }

        [Required]
        [MaxLength(100)]
        public string? Title { get; set; }
        [Required]
        [MaxLength(150)]
        public string? TransliterationUrl { get; set; }

        public int ViewCount { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        [Required]
        public DateTime EventStartOrPersonBirthDate { get; set; }

        public DateTime? EventEndOrPersonDeathDate { get; set; }
        public string? InstagramARLink { get; set; }
        public string? InvolvedPeople { get; set; }

        public int? AudioId { get; set; }

        public Text? Text { get; set; }

        public Audio? Audio { get; set; }

        public List<StatisticRecord> StatisticRecords { get; set; } = new();

        public List<StreetcodeCoordinate> Coordinates { get; set; } = new();

        public TransactionLink? TransactionLink { get; set; }

        public List<Toponym> Toponyms { get; set; } = new();

        public List<Image> Images { get; set; } = new();

        public List<StreetcodeTagIndex> StreetcodeTagIndices { get; set; } = new();

        public List<Tag> Tags { get; set; } = new();

        public List<Subtitle> Subtitles { get; set; } = new();

        public List<Fact> Facts { get; set; } = new();

        public List<Video> Videos { get; set; } = new();

        public List<SourceLinkCategory> SourceLinkCategories { get; set; } = new();

        public List<TimelineItem> TimelineItems { get; set; } = new();

        public List<RelatedFigure> Observers { get; set; } = new();

        public List<RelatedFigure> Targets { get; set; } = new();

        public List<Partner> Partners { get; set; } = new();

        public List<StreetcodeArt> StreetcodeArts { get; set; } = new();

        public List<StreetcodeCategoryContent> StreetcodeCategoryContents { get; set; } = new();
    }
}
