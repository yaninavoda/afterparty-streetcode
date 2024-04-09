using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Streetcode.BLL.DTO.Media.Art;

public sealed record DeleteStreetcodeArtRequestDto(int ArtId, int StreetcodeId);
