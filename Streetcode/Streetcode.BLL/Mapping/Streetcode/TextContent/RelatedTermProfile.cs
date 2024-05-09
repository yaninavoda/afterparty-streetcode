using AutoMapper;
using Streetcode.BLL.Dto.Streetcode.TextContent;
using Streetcode.BLL.Entities.Streetcode.TextContent;

namespace Streetcode.BLL.Mapping.Streetcode.TextContent
{
    public class RelatedTermProfile : Profile
    {
        public RelatedTermProfile()
        {
            CreateMap<RelatedTerm, RelatedTermDto>().ReverseMap();
        }
    }
}
