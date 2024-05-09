using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Streetcode.BLL.Dto.AdditionalContent.Subtitles;
using Streetcode.BLL.Dto.Toponyms;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.RepositoryInterfaces.Base;

namespace Streetcode.BLL.MediatR.Toponyms.GetByStreetcodeId
{
    public class GetToponymsByStreetcodeIdHandler : IRequestHandler<GetToponymsByStreetcodeIdQuery, Result<IEnumerable<ToponymDto>>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly ILoggerService _logger;

        public GetToponymsByStreetcodeIdHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILoggerService logger)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<IEnumerable<ToponymDto>>> Handle(GetToponymsByStreetcodeIdQuery request, CancellationToken cancellationToken)
        {
            var toponyms = await _repositoryWrapper
                .ToponymRepository
                .GetAllAsync(
                    predicate: sc => sc.Streetcodes.Any(s => s.Id == request.StreetcodeId),
                    include: scl => scl
                        .Include(sc => sc.Coordinate));

            if (toponyms is null)
            {
                string errorMsg = $"Cannot find any toponym by the streetcode id: {request.StreetcodeId}";
                _logger.LogError(request, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }

            var toponymDtos = toponyms
                .DistinctBy(x => x.StreetName)
                .Select(x => _mapper.Map<ToponymDto>(x));
            return Result.Ok(toponymDtos);
        }
    }
}
