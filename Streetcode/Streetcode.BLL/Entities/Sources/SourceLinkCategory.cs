using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Streetcode.BLL.Entities.Media.Images;
using Streetcode.BLL.Entities.Streetcode;

namespace Streetcode.BLL.Entities.Sources
{
    [Table("source_link_categories", Schema = "sources")]
    public class SourceLinkCategory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string? Title { get; set; }

        [Required]
        public int ImageId { get; set; }

        public Image? Image { get; set; }

        public List<StreetcodeContent> Streetcodes { get; set; } = new();

        public List<StreetcodeCategoryContent> StreetcodeCategoryContents { get; set; } = new();
    }
}