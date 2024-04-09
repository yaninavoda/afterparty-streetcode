using FluentResults;
using MediatR;
using Streetcode.BLL.Dto.Timeline;

namespace Streetcode.BLL.MediatR.Timeline.TimelineItem.Create;

public class CreateTimelineItemHandler : IRequestHandler<CreateTimelineItemCommand, Result<TimelineItemDto>>
{
    public Task<Result<TimelineItemDto>> Handle(CreateTimelineItemCommand command, CancellationToken cancellationToken)
    {
        var request = command.CreateTimelineItemRequestDto;

        throw new NotImplementedException();
    }
}
