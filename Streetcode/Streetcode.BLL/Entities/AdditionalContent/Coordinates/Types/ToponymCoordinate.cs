using System.ComponentModel.DataAnnotations;
using Streetcode.BLL.Entities.Toponyms;

namespace Streetcode.BLL.Entities.AdditionalContent.Coordinates.Types
{
    public class ToponymCoordinate : Coordinate
    {
        [Required]
        public int ToponymId { get; set; }

        public Toponym? Toponym { get; set; }
    }
}