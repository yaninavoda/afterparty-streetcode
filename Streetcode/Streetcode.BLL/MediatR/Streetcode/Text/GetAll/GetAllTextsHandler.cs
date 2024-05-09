﻿using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.Dto.AdditionalContent.Subtitles;
using Streetcode.BLL.Dto.Streetcode.TextContent.Text;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.Entities.AdditionalContent.Coordinates;
using Streetcode.BLL.RepositoryInterfaces.Base;

namespace Streetcode.BLL.MediatR.Streetcode.Text.GetAll
{
    public class GetAllTextsHandler : IRequestHandler<GetAllTextsQuery, Result<IEnumerable<TextDto>>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly ILoggerService _logger;

        public GetAllTextsHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILoggerService logger)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<IEnumerable<TextDto>>> Handle(GetAllTextsQuery request, CancellationToken cancellationToken)
        {
            var texts = await _repositoryWrapper.TextRepository.GetAllAsync();

            if (texts is null)
            {
                const string errorMsg = $"Cannot find any text";
                _logger.LogError(request, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }

            return Result.Ok(_mapper.Map<IEnumerable<TextDto>>(texts));
        }
    }
}