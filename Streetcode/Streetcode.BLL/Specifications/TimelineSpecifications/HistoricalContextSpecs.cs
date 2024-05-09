using Ardalis.Specification;
using Streetcode.BLL.Entities.Timeline;

namespace Streetcode.BLL.Specifications.TimelineSpecifications
{
    public static class HistoricalContextSpecs
    {
        public class GetByTitleWithHistoricalContextTimelines : Specification<HistoricalContext>,
            ISingleResultSpecification<HistoricalContext>
        {
            public GetByTitleWithHistoricalContextTimelines(string title)
            {
                Query
                    .Where(hc => hc.Title == title)
                    .Include(hc => hc.HistoricalContextTimelines);
            }
        }
    }
}