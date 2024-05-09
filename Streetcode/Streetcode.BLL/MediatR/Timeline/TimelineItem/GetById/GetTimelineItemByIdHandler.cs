using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Streetcode.BLL.Dto.Timeline;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.Resources.Errors;
using Streetcode.BLL.RepositoryInterfaces.Base;
using TimelineItemEntity = Streetcode.BLL.Entities.Timeline.TimelineItem;

namespace Streetcode.BLL.MediatR.Timeline.TimelineItem.GetById
{
    public class GetTimelineItemByIdHandler : IRequestHandler<GetTimelineItemByIdQuery, Result<TimelineItemDto>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly ILoggerService _logger;
        public GetTimelineItemByIdHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILoggerService logger)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<TimelineItemDto>> Handle(GetTimelineItemByIdQuery request, CancellationToken cancellationToken)
        {
            var timelineItem = await _repositoryWrapper.TimelineRepository
                .GetFirstOrDefaultAsync(
                    predicate: ti => ti.Id == request.Id,
                    include: ti => ti
                        .Include(til => til.HistoricalContextTimelines)
                            .ThenInclude(x => x.HistoricalContext)!);

            if (timelineItem is null)
            {
                string errorMsg = string.Format(
                    ErrorMessages.EntityByIdNotFound,
                    typeof(TimelineItemEntity).Name,
                    request.Id);
                _logger.LogError(request, errorMsg);

                return Result.Fail(new Error(errorMsg));
            }

            return Result.Ok(_mapper.Map<TimelineItemDto>(timelineItem));
        }
    }
}
