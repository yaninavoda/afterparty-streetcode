using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Streetcode.BLL.Dto.AdditionalContent.Subtitles;
using Streetcode.BLL.Dto.AdditionalContent.Tag;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.Entities.Streetcode;
using Streetcode.BLL.RepositoryInterfaces.Base;

namespace Streetcode.BLL.MediatR.AdditionalContent.Tag.GetByStreetcodeId
{
    public class GetTagByStreetcodeIdHandler : IRequestHandler<GetTagByStreetcodeIdQuery, Result<IEnumerable<StreetcodeTagDto>>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly ILoggerService _logger;

        public GetTagByStreetcodeIdHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILoggerService logger)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<IEnumerable<StreetcodeTagDto>>> Handle(GetTagByStreetcodeIdQuery request, CancellationToken cancellationToken)
        {
            /*
            StreetcodeContent streetcode = await _repositoryWrapper.StreetcodeRepository.GetFirstOrDefaultAsync(s => s.Id == request.StreetcodeId);
            if(streetcode is null)
            {
                return Result.Fail(new Error($"Streetcode with id: {request.StreetcodeId} doesn`t exist"));
            }
            */
            var tagIndexed = await _repositoryWrapper.StreetcodeTagIndexRepository
                .GetAllAsync(
                    t => t.StreetcodeId == request.StreetcodeId,
                    include: q => q.Include(t => t.Tag));

            if (tagIndexed is null)
            {
                string errorMsg = $"Cannot find any tag by the streetcode id: {request.StreetcodeId}";
                _logger.LogError(request, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }

            return Result.Ok(_mapper.Map<IEnumerable<StreetcodeTagDto>>(tagIndexed.OrderBy(ti => ti.Index)));
        }
    }
}
