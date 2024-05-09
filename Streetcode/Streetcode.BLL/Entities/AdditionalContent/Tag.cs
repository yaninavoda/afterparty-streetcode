using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Streetcode.BLL.Entities.Streetcode;

namespace Streetcode.BLL.Entities.AdditionalContent
{
    [Table("tags", Schema = "add_content")]
    public class Tag
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Title { get; set; }

        public IEnumerable<StreetcodeTagIndex> StreetcodeTagIndices { get; set; }

        public IEnumerable<StreetcodeContent> Streetcodes { get; set; }
    }
}