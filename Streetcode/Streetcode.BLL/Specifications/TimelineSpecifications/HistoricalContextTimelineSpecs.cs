using Ardalis.Specification;
using Streetcode.BLL.Entities.Timeline;

namespace Streetcode.BLL.Specifications.TimelineSpecifications
{
    public static class HistoricalContextTimelineSpecs
    {
        public class GetAllByHistoricalContextIdAndTimelineId : Specification<HistoricalContextTimeline>
        {
            public GetAllByHistoricalContextIdAndTimelineId(int historicalContextId, int TimelineId)
            {
                Query
                    .Where(hct => hct.HistoricalContextId == historicalContextId
                        && hct.TimelineId == TimelineId);
            }
        }

        public class GetByHistoricalContextIdAndTimelineId : Specification<HistoricalContextTimeline>,
            ISingleResultSpecification<HistoricalContextTimeline>
        {
            public GetByHistoricalContextIdAndTimelineId(int historicalContextId, int TimelineId)
            {
                Query
                .Where(hct => hct.HistoricalContextId == historicalContextId
                        && hct.TimelineId == TimelineId);
            }
        }
    }
}