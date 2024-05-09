using System.ComponentModel.DataAnnotations;
using Streetcode.BLL.Entities.Analytics;
using Streetcode.BLL.Entities.Streetcode;

namespace Streetcode.BLL.Entities.AdditionalContent.Coordinates.Types
{
    public class StreetcodeCoordinate : Coordinate
    {
        [Required]
        public int StreetcodeId { get; set; }

        public StreetcodeContent? Streetcode { get; set; }

        public StatisticRecord StatisticRecord { get; set; }
    }
}
