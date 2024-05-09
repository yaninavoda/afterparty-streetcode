using AutoMapper;
using Streetcode.BLL.Dto.Media.Images;
using Streetcode.BLL.Entities.Media.Images;
using Streetcode.BLL.Entities.Streetcode;

namespace Streetcode.BLL.Mapping.Media.Images
{
    public class ImageProfile : Profile
    {
        public ImageProfile()
        {
            CreateMap<Image, ImageDto>().ReverseMap();

            CreateMap<ImageFileBaseCreateDto, Image>().ForMember(d => d.ImageDetails, opt => opt.MapFrom(s => s));
    	}
    }
}
