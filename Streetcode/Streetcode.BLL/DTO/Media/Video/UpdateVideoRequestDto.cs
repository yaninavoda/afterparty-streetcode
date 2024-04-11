using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Streetcode.BLL.DTO.Media.Video
{
    public sealed record UpdateVideoRequestDto(int Id, string? Title, string? Description, string Url, int StreetcodeId);
}
