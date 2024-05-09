﻿using AutoMapper;
using Streetcode.BLL.Dto.AdditionalContent.Coordinates.Types;
using Streetcode.BLL.Entities.AdditionalContent.Coordinates.Types;

namespace Streetcode.BLL.Mapping.AdditionalContent.Coordinates
{
    public class ToponymCoordinateProfile : Profile
    {
        public ToponymCoordinateProfile()
        {
            CreateMap<ToponymCoordinate, ToponymCoordinateDto>().ReverseMap();
        }
    }
}