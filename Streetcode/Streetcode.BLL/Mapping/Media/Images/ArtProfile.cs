using AutoMapper;
using Streetcode.BLL.Dto.Media.Art;
using Streetcode.BLL.Entities.Media.Images;

namespace Streetcode.BLL.Mapping.Media.Images
{
    public class ArtProfile : Profile
    {
        public ArtProfile()
        {
            CreateMap<Art, ArtDto>().ReverseMap();
            CreateMap<CreateArtRequestDto, Art>();
            CreateMap<Art, CreateArtResponseDto>();
        }
    }
}
