using System.Linq.Expressions;
using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.Dto.Timeline;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.Resources.Errors;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.BLL.DTO.Timeline;

using HistoricalContextEntity = Streetcode.DAL.Entities.Timeline.HistoricalContext;
using TimelineItemEntity = Streetcode.DAL.Entities.Timeline.TimelineItem;
using Streetcode.DAL.Entities.Timeline;
using Streetcode.DAL.Entities.Streetcode;
using Org.BouncyCastle.Asn1.Ocsp;

namespace Streetcode.BLL.MediatR.Timeline.TimelineItem.Create;

public class CreateTimelineItemHandler : IRequestHandler<CreateTimelineItemCommand, Result<TimelineItemDto>>
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IMapper _mapper;
    private readonly ILoggerService _logger;

    public CreateTimelineItemHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILoggerService logger)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<TimelineItemDto>> Handle(CreateTimelineItemCommand command, CancellationToken cancellationToken)
    {
        TimelineItemEntity timeline;
        TimelineItemDto response;

        using var transaction = _repositoryWrapper.BeginTransaction();

        var request = command.CreateTimelineItemRequestDto;

        if (! await IsStreetcodeExistAsync(request.StreetcodeId))
        {
            return StreetcodeNotFoundError(request.StreetcodeId);
        }

        if (request.HistoricalContext is null)
        {
            timeline = _mapper.Map<TimelineItemEntity>(request);

            _repositoryWrapper.TimelineRepository.Create(timeline);

            if (!(await _repositoryWrapper.SaveChangesAsync() > 0))
            {
                CreateTimelineItemFailError(request);
            }

            transaction.Complete();

            response = _mapper.Map<TimelineItemDto>(timeline);

            return Result.Ok(response);
        }

        if (request.HistoricalContext.Id <= 0 && !string.IsNullOrEmpty(request.HistoricalContext.Title))
        {
            var historicalContext = _mapper.Map<HistoricalContextEntity>(request.HistoricalContext);

            _repositoryWrapper.HistoricalContextRepository.Create(historicalContext);

            if (!(await _repositoryWrapper.SaveChangesAsync() > 0))
            {
                CreateHistoricalContextFailError(historicalContext);
            }

            var hc = await _repositoryWrapper.HistoricalContextRepository
            .GetFirstOrDefaultAsync(
                hc => hc.Title == request.HistoricalContext.Title);

            var hcDto = _mapper.Map<HistoricalContextDto>(hc);

            request.HistoricalContext = hcDto;
        }

        timeline = _mapper.Map<TimelineItemEntity>(request);

        _repositoryWrapper.TimelineRepository.Create(timeline);

        if (!(await _repositoryWrapper.SaveChangesAsync() > 0))
        {
            CreateTimelineItemFailError(request);
        }

        HistoricalContextTimeline historicalContextTimeline = new HistoricalContextTimeline
        {
            TimelineId = timeline.Id,
            HistoricalContextId = request.HistoricalContext.Id,
        };

        _repositoryWrapper.HistoricalContextTimelineRepository.Create(historicalContextTimeline);

        if (!(await _repositoryWrapper.SaveChangesAsync() > 0))
        {
            CreateHistoricalContextTimelineFailError(historicalContextTimeline);
        }

        transaction.Complete();

        response = _mapper.Map<TimelineItemDto>(timeline);

        var list = new List<HistoricalContextDto>()
        {
            request.HistoricalContext
        };

        response.HistoricalContexts = list;

        return Result.Ok(response);
    }

    private async Task<bool> IsStreetcodeExistAsync(int streetcodeId)
    {
        var streetcode = await _repositoryWrapper.StreetcodeRepository.GetFirstOrDefaultAsync(x => x.Id == streetcodeId);

        return streetcode is not null;
    }

    private Result<TimelineItemDto> StreetcodeNotFoundError(int streetcodeId)
    {
        string errorMessage = string.Format(
            ErrorMessages.EntityByIdNotFound,
            typeof(StreetcodeContent).Name,
            streetcodeId);

        _logger.LogError(streetcodeId, errorMessage);

        return Result.Fail(errorMessage);
    }

    private Result<TimelineItemDto> CreateHistoricalContextFailError(HistoricalContextEntity request)
    {
        string errorMessage = string.Format(
            ErrorMessages.CreateFailed,
            typeof(HistoricalContextEntity).Name);

        _logger.LogError(request, errorMessage);

        return Result.Fail(errorMessage);
    }

    private Result<TimelineItemDto> CreateTimelineItemFailError(CreateTimelineItemRequestDto request)
    {
        string errorMessage = string.Format(
            ErrorMessages.CreateFailed,
            typeof(TimelineItemEntity).Name,
            request);

        _logger.LogError(request, errorMessage);

        return Result.Fail(errorMessage);
    }

    private Result<TimelineItemDto> CreateHistoricalContextTimelineFailError(HistoricalContextTimeline request)
    {
        string errorMessage = string.Format(
            ErrorMessages.CreateFailed,
            typeof(HistoricalContextTimeline).Name,
            request);

        _logger.LogError(request, errorMessage);

        return Result.Fail(errorMessage);
    }
}
