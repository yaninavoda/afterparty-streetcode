﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Streetcode.BLL.Dto.Streetcode.TextContent.Fact
{
    public sealed record FactForUpdateDto(int Id, string Title, string FactContent, int ImageId);
}
