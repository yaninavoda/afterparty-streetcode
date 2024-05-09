using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.Dto.AdditionalContent.Subtitles;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.ResultVariations;
using Streetcode.BLL.Entities.Media;
using Streetcode.BLL.RepositoryInterfaces.Base;

namespace Streetcode.BLL.MediatR.AdditionalContent.Subtitle.GetByStreetcodeId
{
    public class GetSubtitlesByStreetcodeIdHandler : IRequestHandler<GetSubtitlesByStreetcodeIdQuery, Result<SubtitleDto>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;

        public GetSubtitlesByStreetcodeIdHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILoggerService logger)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
        }

        public async Task<Result<SubtitleDto>> Handle(GetSubtitlesByStreetcodeIdQuery request, CancellationToken cancellationToken)
        {
            var subtitle = await _repositoryWrapper.SubtitleRepository
                .GetFirstOrDefaultAsync(Subtitle => Subtitle.StreetcodeId == request.StreetcodeId);

            NullResult<SubtitleDto> result = new NullResult<SubtitleDto>();
            result.WithValue(_mapper.Map<SubtitleDto>(subtitle));
            return result;
        }
    }
}
