﻿using AutoMapper;
using Streetcode.BLL.Dto.AdditionalContent.Coordinates.Types;
using Streetcode.BLL.DTO.AdditionalContent.Coordinates.Types;
using Streetcode.BLL.Entities.AdditionalContent.Coordinates.Types;

namespace Streetcode.BLL.Mapping.AdditionalContent.Coordinates
{
    public class StreetcodeCoordinateProfile : Profile
    {
       public StreetcodeCoordinateProfile()
       {
            CreateMap<StreetcodeCoordinate, StreetcodeCoordinateDto>().ReverseMap();
            CreateMap<CreateStreetcodeCoordinateRequestDto, StreetcodeCoordinate>();
            CreateMap<StreetcodeCoordinate, CreateStreetcodeCoordinateResponseDto>();
        }
    }
}