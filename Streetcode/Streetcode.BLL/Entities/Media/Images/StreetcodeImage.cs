using System.ComponentModel.DataAnnotations;
using Streetcode.BLL.Entities.Streetcode;

namespace Streetcode.BLL.Entities.Media.Images
{
    public class StreetcodeImage
    {
        [Required]
        public int StreetcodeId { get; set; }

        [Required]
        public int ImageId { get; set; }

        public Image? Image { get; set; }

        public StreetcodeContent? Streetcode { get; set; }
    }
}
