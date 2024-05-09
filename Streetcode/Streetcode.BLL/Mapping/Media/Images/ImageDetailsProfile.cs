﻿using AutoMapper;
using Streetcode.BLL.Dto.Media.Images;
using Streetcode.BLL.Entities.Media.Images;

namespace Streetcode.BLL.Mapping.Media.Images
{
    public class ImageDetailsProfile : Profile
    {
        public ImageDetailsProfile()
        {
            CreateMap<ImageDetails, ImageDetailsDto>().ReverseMap();
            CreateMap<ImageFileBaseCreateDto, ImageDetails>();
        }
    }
}
