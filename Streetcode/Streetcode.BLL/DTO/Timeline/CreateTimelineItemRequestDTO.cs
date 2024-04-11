using Streetcode.BLL.Dto.Timeline;
using Streetcode.DAL.Entities.Timeline;
using Streetcode.DAL.Enums;

namespace Streetcode.BLL.DTO.Timeline;

public class CreateTimelineItemRequestDto
{
    public string Title { get; set; }
    public string? Description { get; set; }
    public DateTime Date { get; set; }
    public DateViewPattern DateViewPattern { get; set; }
    public HistoricalContextDto? HistoricalContext { get; set; }
    public int StreetcodeId { get; set; }
}