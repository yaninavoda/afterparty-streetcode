using AutoMapper;
using Streetcode.BLL.Dto.Media.Video;
using Streetcode.BLL.DTO.Media.Video;
using Streetcode.DAL.Entities.Media;

namespace Streetcode.BLL.Mapping.Media;

public class VideoProfile : Profile
{
    public VideoProfile()
    {
        CreateMap<Video, VideoDto>();
        CreateMap<CreateVideoRequestDto, Video>().ReverseMap();
    }
}
