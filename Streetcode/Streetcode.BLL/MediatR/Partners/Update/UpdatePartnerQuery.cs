using FluentResults;
using MediatR;
using Streetcode.BLL.Dto.Partners;
using Streetcode.BLL.Dto.Partners.Create;
using Streetcode.BLL.DTO.Partners.Create;

namespace Streetcode.BLL.MediatR.Partners.Update
{
    public record UpdatePartnerQuery(CreatePartnerDto Partner) : IRequest<Result<PartnerDto>>;
}
