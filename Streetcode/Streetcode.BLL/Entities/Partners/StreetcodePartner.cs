using System.ComponentModel.DataAnnotations;
using Streetcode.BLL.Entities.Streetcode;

namespace Streetcode.BLL.Entities.Partners
{
    public class StreetcodePartner
    {
        [Required]
        public int StreetcodeId { get; set; }

        [Required]
        public int PartnerId { get; set; }

        public StreetcodeContent? Streetcode { get; set; }

        public Partner? Partner { get; set; }
    }
}
