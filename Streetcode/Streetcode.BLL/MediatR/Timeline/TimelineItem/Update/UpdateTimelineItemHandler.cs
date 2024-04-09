using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Streetcode.BLL.Dto.Streetcode.TextContent.Fact;
using Streetcode.BLL.Dto.Timeline;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.Resources.Errors;
using Streetcode.DAL.Entities.Timeline;
using Streetcode.DAL.Repositories.Interfaces.Base;
using HistoricalContextEntity = Streetcode.DAL.Entities.Timeline.HistoricalContext;
using TimelineItemEntity = Streetcode.DAL.Entities.Timeline.TimelineItem;
using HistoricalContextTimelineEntity = Streetcode.DAL.Entities.Timeline.HistoricalContextTimeline;
namespace Streetcode.BLL.MediatR.Timeline.TimelineItem.Update;

public class UpdateTimelineItemHandler : IRequestHandler<UpdateTimelineItemCommand, Result<TimelineItemDto>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly ILoggerService _logger;

    public UpdateTimelineItemHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILoggerService logger)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<TimelineItemDto>> Handle(UpdateTimelineItemCommand command, CancellationToken cancellationToken)
    {
        var request = command.UpdateTimelineItemDto;
        TimelineItemEntity timelineItem = _mapper.Map<TimelineItemEntity>(request);
        TimelineItemDto responseDto;

        if (!string.IsNullOrEmpty(request.HistoricalContext))
        {
            HistoricalContextTimelineEntity historicalContextTimeline;

            // find context by title  in db - if found check if there is a record in cross table, if not create a record;
            HistoricalContextEntity? historicalContext = await _repositoryWrapper.HistoricalContextRepository
                .GetFirstOrDefaultAsync(
                hc => hc.Title == request.HistoricalContext,
                h => h.Include(x => x.HistoricalContextTimelines));

            if (historicalContext == null)
            {
                // create new historical context
                historicalContext = new HistoricalContextEntity
                {
                    Title = request.HistoricalContext,
                };

                _repositoryWrapper.HistoricalContextRepository.Create(historicalContext);
                await _repositoryWrapper.SaveChangesAsync();

                // get its id, add to cross table
                historicalContextTimeline = new ()
                {
                    TimelineId = request.Id,
                    HistoricalContextId = historicalContext.Id,
                };
                _repositoryWrapper.HistoricalContextTimelineRepository.Create(historicalContextTimeline);
            }
            else
            {
                historicalContextTimeline = await _repositoryWrapper.HistoricalContextTimelineRepository.GetFirstOrDefaultAsync(
                    hct => hct.TimelineId == request.Id && hct.HistoricalContextId == historicalContext.Id);
                if (historicalContextTimeline == null)
                {
                    historicalContextTimeline = new ()
                    {
                        TimelineId = request.Id,
                        HistoricalContextId = historicalContext.Id,
                    };
                    _repositoryWrapper.HistoricalContextTimelineRepository.Create(historicalContextTimeline);
                }
            }

            _repositoryWrapper.TimelineRepository.Update(timelineItem!);

            if (await _repositoryWrapper.SaveChangesAsync() <= 0)
            {
                return UpdateFailedError(request);
            }

            var updatedTimelineItem = await _repositoryWrapper.TimelineRepository
            .GetFirstOrDefaultAsync(
                predicate: ti => ti.Id == request.Id,
                include: ti => ti
                    .Include(til => til.HistoricalContextTimelines)
                        .ThenInclude(x => x.HistoricalContext)!);

            responseDto = _mapper.Map<TimelineItemDto>(updatedTimelineItem);

            var historicalContextDtos = (await _repositoryWrapper.HistoricalContextTimelineRepository
                .GetAllAsync(hct =>
                hct.HistoricalContextId == historicalContextTimeline.HistoricalContextId
                    && hct.TimelineId == historicalContextTimeline.TimelineId))
                        .Select(pair => new HistoricalContextDto
                        {
                            Id = pair.HistoricalContextId,
                            Title = request.HistoricalContext
                        });

            responseDto.HistoricalContexts = historicalContextDtos;

            return Result.Ok(responseDto);
        }
        else
        {
            // if request.context is nullorempty - update and return ok
            _repositoryWrapper.TimelineRepository.Update(timelineItem!);

            if(await _repositoryWrapper.SaveChangesAsync() <= 0)
            {
                return UpdateFailedError(request);
            }

            var updatedTimelineItem = await _repositoryWrapper.TimelineRepository
            .GetFirstOrDefaultAsync(
                predicate: ti => ti.Id == request.Id,
                include: ti => ti
                    .Include(til => til.HistoricalContextTimelines)
                        .ThenInclude(x => x.HistoricalContext)!);

            responseDto = _mapper.Map<TimelineItemDto>(updatedTimelineItem);

            return Result.Ok(responseDto);
        }
    }

    private Result<TimelineItemDto> UpdateFailedError(UpdateTimelineItemRequestDto request)
    {
        string errorMessage = string.Format(
            ErrorMessages.UpdateFailed,
            typeof(TimelineItemEntity).Name,
            request.Id);

        _logger.LogError(request, errorMessage);

        return Result.Fail(errorMessage);
    }
}
