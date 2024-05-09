using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Streetcode.BLL.Entities.Media.Images;
using Streetcode.BLL.Entities.Streetcode.TextContent;

namespace Streetcode.BLL.Dto.Streetcode
{
    public class StreetcodeMainPageDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Alias { get; set; }
        public string? Teaser { get; set; }
        public string? Text { get; set; }
        public int ImageId { get; set; }

        public string TransliterationUrl { get; set; }
    }
}
