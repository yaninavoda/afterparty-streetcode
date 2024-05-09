using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Streetcode.BLL.Dto.AdditionalContent.Subtitles;
using Streetcode.BLL.Dto.Timeline;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.RepositoryInterfaces.Base;

namespace Streetcode.BLL.MediatR.Timeline.TimelineItem.GetByStreetcodeId
{
    public class GetTimelineItemsByStreetcodeIdHandler : IRequestHandler<GetTimelineItemsByStreetcodeIdQuery, Result<IEnumerable<TimelineItemDto>>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly ILoggerService _logger;

        public GetTimelineItemsByStreetcodeIdHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILoggerService logger)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<IEnumerable<TimelineItemDto>>> Handle(GetTimelineItemsByStreetcodeIdQuery request, CancellationToken cancellationToken)
        {
            var timelineItems = await _repositoryWrapper.TimelineRepository
                .GetAllAsync(
                    predicate: f => f.StreetcodeId == request.StreetcodeId,
                    include: ti => ti
                        .Include(til => til.HistoricalContextTimelines)
                            .ThenInclude(x => x.HistoricalContext)!);

            if (timelineItems is null)
            {
                string errorMsg = $"Cannot find any timeline item by the streetcode id: {request.StreetcodeId}";
                _logger.LogError(request, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }

            return Result.Ok(_mapper.Map<IEnumerable<TimelineItemDto>>(timelineItems));
        }
    }
}
