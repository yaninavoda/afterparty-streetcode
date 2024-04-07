using Streetcode.DAL.Enums;

namespace Streetcode.BLL.Dto.Timeline;

public class UpdateTimelineItemRequestDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public DateViewPattern DateViewPattern { get; set; }
    public string? HistoricalContext { get; set; }
}
