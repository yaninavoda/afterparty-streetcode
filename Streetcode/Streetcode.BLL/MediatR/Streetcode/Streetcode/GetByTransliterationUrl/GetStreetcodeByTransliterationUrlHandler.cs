﻿using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Streetcode.BLL.Dto.AdditionalContent.Tag;
using Streetcode.BLL.Dto.Streetcode;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.RepositoryInterfaces.Base;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.GetByTransliterationUrl
{
  public class GetStreetcodeByTransliterationUrlHandler : IRequestHandler<GetStreetcodeByTransliterationUrlQuery, Result<StreetcodeDto>>
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;
        private readonly ILoggerService _logger;

        public GetStreetcodeByTransliterationUrlHandler(IRepositoryWrapper repository, IMapper mapper, ILoggerService logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<StreetcodeDto>> Handle(GetStreetcodeByTransliterationUrlQuery request, CancellationToken cancellationToken)
        {
            var streetcode = await _repository.StreetcodeRepository
                .GetFirstOrDefaultAsync(
                    predicate: st => st.TransliterationUrl == request.url);

            if (streetcode == null)
            {
                string errorMsg = $"Cannot find streetcode by transliteration url: {request.url}";
                _logger.LogError(request, errorMsg);
                return new Error(errorMsg);
            }

            var tagIndexed = await _repository.StreetcodeTagIndexRepository
                                            .GetAllAsync(
                                                t => t.StreetcodeId == streetcode.Id,
                                                include: q => q.Include(ti => ti.Tag));

            var streetcodeDto = _mapper.Map<StreetcodeDto>(streetcode);
            streetcodeDto.Tags = _mapper.Map<List<StreetcodeTagDto>>(tagIndexed);
            return Result.Ok(streetcodeDto);
        }
    }
}
