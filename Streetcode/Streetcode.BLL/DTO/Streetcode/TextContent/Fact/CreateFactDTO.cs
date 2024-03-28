using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Streetcode.BLL.Dto.Streetcode.TextContent.Fact
{
    public sealed record CreateFactDto(string Title, string FactContent, int ImageId, int StreetcodeId);
}
