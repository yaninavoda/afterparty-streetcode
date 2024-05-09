using System.ComponentModel.DataAnnotations;

namespace Streetcode.BLL.Entities.Timeline
{
    public class HistoricalContextTimeline
    {
        [Required]
        public int HistoricalContextId { get; set; }

        [Required]
        public int TimelineId { get; set; }

        public HistoricalContext? HistoricalContext { get; set; }

        public TimelineItem? Timeline { get; set; }
    }
}
