using AutoMapper;
using Streetcode.BLL.Dto.Media.Art;
using Streetcode.BLL.Entities.Streetcode;
using Streetcode.BLL.Entities.Media.Images;

namespace Streetcode.BLL.Mapping.Media.Images
{
    public class StreetcodeArtProfile : Profile
    {
        public StreetcodeArtProfile()
        {
            CreateMap<StreetcodeArt, StreetcodeArtDto>().ReverseMap();
        }
    }
}
