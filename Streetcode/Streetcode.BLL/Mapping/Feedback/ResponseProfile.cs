﻿using AutoMapper;
using Streetcode.BLL.Dto.Feedback;
using Streetcode.BLL.Entities.Feedback;

namespace Streetcode.BLL.Mapping.Feedback
{
    public class ResponseProfile : Profile
    {
        public ResponseProfile()
        {
            CreateMap<Response, ResponseDto>().ReverseMap();
        }
    }
}