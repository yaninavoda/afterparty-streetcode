using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.AdditionalContent.Tag;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.RepositoryInterfaces.Base;

namespace Streetcode.BLL.MediatR.AdditionalContent.Tag.Create
{
    public class CreateTagHandler : IRequestHandler<CreateTagQuery, Result<TagDto>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly ILoggerService _logger;

        public CreateTagHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILoggerService logger)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<TagDto>> Handle(CreateTagQuery request, CancellationToken cancellationToken)
        {
            var newTag = _repositoryWrapper.TagRepository.Create(new BLL.Entities.AdditionalContent.Tag()
            {
                Title = request.tag.Title
            });

            try
            {
                await _repositoryWrapper.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                _logger.LogError(request, ex.ToString());
                return Result.Fail(ex.ToString());
            }

            return Result.Ok(_mapper.Map<TagDto>(newTag));
        }
    }
}
