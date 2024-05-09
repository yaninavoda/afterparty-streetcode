using AutoMapper;
using Streetcode.BLL.Dto.Sources;
using Streetcode.BLL.Entities.Sources;

namespace Streetcode.BLL.Mapping.Sources
{
    public class SourceLinkSubCategoryProfile : Profile
    {
        public SourceLinkSubCategoryProfile()
        {
            CreateMap<CategoryContentCreateDto, StreetcodeCategoryContent>().ReverseMap();
        }
    }
}
