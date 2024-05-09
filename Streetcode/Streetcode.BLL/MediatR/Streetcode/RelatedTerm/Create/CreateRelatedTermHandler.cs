﻿using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.Dto.Streetcode.TextContent;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.RepositoryInterfaces.Base;

using Entity = Streetcode.BLL.Entities.Streetcode.TextContent.RelatedTerm;

namespace Streetcode.BLL.MediatR.Streetcode.RelatedTerm.Create
{
    public class CreateRelatedTermHandler : IRequestHandler<CreateRelatedTermCommand, Result<RelatedTermDto>>
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;
        private readonly ILoggerService _logger;

        public CreateRelatedTermHandler(IRepositoryWrapper repository, IMapper mapper, ILoggerService logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<RelatedTermDto>> Handle(CreateRelatedTermCommand request, CancellationToken cancellationToken)
        {
            var relatedTerm = _mapper.Map<Entity>(request.RelatedTerm);

            if (relatedTerm is null)
            {
                const string errorMsg = "Cannot create new related word for a term!";
                _logger.LogError(request, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }

            var existingTerms = await _repository.RelatedTermRepository
                .GetAllAsync(
                predicate: rt => rt.TermId == request.RelatedTerm.TermId && rt.Word == request.RelatedTerm.Word);

            if (existingTerms is null || existingTerms.Any())
            {
                const string errorMsg = "Слово з цим визначенням уже існує";
                _logger.LogError(request, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }

            var createdRelatedTerm = _repository.RelatedTermRepository.Create(relatedTerm);

            var isSuccessResult = await _repository.SaveChangesAsync() > 0;

            if(!isSuccessResult)
            {
                const string errorMsg = "Cannot save changes in the database after related word creation!";
                _logger.LogError(request, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }

            var createdRelatedTermDto = _mapper.Map<RelatedTermDto>(createdRelatedTerm);

            if(createdRelatedTermDto != null)
            {
                return Result.Ok(createdRelatedTermDto);
            }
            else
            {
                const string errorMsg = "Cannot map entity!";
                _logger.LogError(request, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }
        }
    }
}
