using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.AdditionalContent.Tag;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.RepositoryInterfaces.Base;

namespace Streetcode.BLL.MediatR.AdditionalContent.Tag.GetById
{
    public class GetTagByIdHandler : IRequestHandler<GetTagByIdQuery, Result<TagDto>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly ILoggerService _logger;

        public GetTagByIdHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILoggerService logger)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<TagDto>> Handle(GetTagByIdQuery request, CancellationToken cancellationToken)
        {
            var tag = await _repositoryWrapper.TagRepository.GetFirstOrDefaultAsync(f => f.Id == request.Id);

            if (tag is null)
            {
                string errorMsg = $"Cannot find a Tag with corresponding id: {request.Id}";
                _logger.LogError(request, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }

            return Result.Ok(_mapper.Map<TagDto>(tag));
        }
    }
}
