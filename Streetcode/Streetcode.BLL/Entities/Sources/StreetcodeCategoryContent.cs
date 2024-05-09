using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Streetcode.BLL.Entities.Streetcode;

namespace Streetcode.BLL.Entities.Sources
{
    [Table("streetcode_categoryContent", Schema = "sources")]
    public class StreetcodeCategoryContent
    {
        [Required]
        [MaxLength(4000)]
        public string? Text { get; set; }

        [Required]
        public int SourceLinkCategoryId { get; set; }

        [Required]
        public int StreetcodeId { get; set; }

        public SourceLinkCategory? SourceLinkCategory { get; set; }
        public StreetcodeContent? Streetcode { get; set; }
    }
}
