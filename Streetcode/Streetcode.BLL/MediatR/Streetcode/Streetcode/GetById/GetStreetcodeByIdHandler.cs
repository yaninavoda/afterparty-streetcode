﻿using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Streetcode.BLL.Dto.AdditionalContent.Subtitles;
using Streetcode.BLL.Dto.AdditionalContent.Tag;
using Streetcode.BLL.Dto.Streetcode;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.RepositoryInterfaces.Base;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.GetById
{
    public class GetStreetcodeByIdHandler : IRequestHandler<GetStreetcodeByIdQuery, Result<StreetcodeDto>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly ILoggerService _logger;

        public GetStreetcodeByIdHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILoggerService logger)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<StreetcodeDto>> Handle(GetStreetcodeByIdQuery request, CancellationToken cancellationToken)
        {
            var streetcode = await _repositoryWrapper.StreetcodeRepository.GetFirstOrDefaultAsync(
                predicate: st => st.Id == request.Id);

            if (streetcode is null)
            {
                string errorMsg = $"Cannot find any streetcode with corresponding id: {request.Id}";
                _logger.LogError(request, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }

            var tagIndexed = await _repositoryWrapper.StreetcodeTagIndexRepository
                                            .GetAllAsync(
                                                t => t.StreetcodeId == request.Id,
                                                include: q => q.Include(ti => ti.Tag));
            var streetcodeDto = _mapper.Map<StreetcodeDto>(streetcode);
            streetcodeDto.Tags = _mapper.Map<List<StreetcodeTagDto>>(tagIndexed);

            return Result.Ok(streetcodeDto);
        }
    }
}