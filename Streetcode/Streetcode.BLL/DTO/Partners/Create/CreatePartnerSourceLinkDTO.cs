﻿using Streetcode.BLL.Enums;

namespace Streetcode.BLL.Dto.Partners.Create
{
    public class CreatePartnerSourceLinkDto
  {
    public int Id { get; set; }

    public LogoType LogoType { get; set; }

    public string TargetUrl { get; set; }
  }
}
