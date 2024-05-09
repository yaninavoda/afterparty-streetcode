﻿using FluentResults;
using MediatR;
using Streetcode.BLL.Dto.Partners;

namespace Streetcode.BLL.MediatR.Partners.GetAll
{
    public record GetAllPartnersQuery : IRequest<Result<IEnumerable<PartnerDto>>>;
}
