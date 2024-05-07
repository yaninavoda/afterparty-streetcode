using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.Dto.Media.Art;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.Resources.Errors;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Repositories.Interfaces.Base;
using ArtEntity = Streetcode.DAL.Entities.Media.Images.Art;
using ImageEntity = Streetcode.DAL.Entities.Media.Images.Image;
using StreetcodeArtEntity = Streetcode.DAL.Entities.Streetcode.StreetcodeArt;

namespace Streetcode.BLL.MediatR.Media.Art.CreateMultiple;

public class CreateArtsHandler : IRequestHandler<CreateArtsCommand, Result<IEnumerable<CreateArtResponseDto>>>
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IMapper _mapper;
    private readonly ILoggerService _logger;

    public CreateArtsHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILoggerService logger)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<IEnumerable<CreateArtResponseDto>>> Handle(
        CreateArtsCommand command,
        CancellationToken cancellationToken)
    {
        using var transactionScope = _repositoryWrapper.BeginTransaction();

        var request = command.Arts.Arts;
        var newArts = new List<ArtEntity>();

        foreach (var art in request)
        {
            if (!await ImageExistsAsync(art.ImageId))
            {
                return ImageNotFoundError(art);
            }

            if (await ArtWithThisImageAlreadyExists(art.ImageId))
            {
                return ArtExistsError(art);
            }

            if (!await StreetcodeExistsAsync(art.StreetcodeId))
            {
                return StreetcodeNotFoundError(art);
            }

            ArtEntity newArt = _mapper.Map<ArtEntity>(art);
            newArts.Add(newArt);
        }

        _repositoryWrapper.ArtRepository.CreateRange(newArts);
        await _repositoryWrapper.SaveChangesAsync();

        var streetcodeId = request.FirstOrDefault()!.StreetcodeId;
        var startIndex = await CalculateMaxIndexAsync(streetcodeId);
        foreach (var art in newArts)
        {
            var streetcodeArt = new StreetcodeArtEntity
            {
                StreetcodeId = request.FirstOrDefault()!.StreetcodeId,
                ArtId = art.Id,
                Index = ++startIndex
            };
            art.StreetcodeArts.Add(streetcodeArt);
            _repositoryWrapper.StreetcodeArtRepository.Create(streetcodeArt);
        }

        var response = new List<CreateArtResponseDto>();
        foreach (var art in newArts)
        {
            var dto = _mapper.Map<CreateArtResponseDto>(art);
            dto.StreetcodeId = streetcodeId;

            response.Add(dto);
        }

        await _repositoryWrapper.SaveChangesAsync();

        transactionScope.Complete();

        return Result.Ok(response.AsEnumerable());
    }

    private async Task<int> CalculateMaxIndexAsync(int streetcodeId)
    {
        var streetcodeArts = await _repositoryWrapper.StreetcodeArtRepository.GetAllAsync(
            predicate: streetcodeArt => streetcodeArt.StreetcodeId == streetcodeId);

        if (streetcodeArts is null || !streetcodeArts.Any())
        {
            return 0;
        }

        return streetcodeArts.Max(sa => sa.Index);
    }

    private Result<IEnumerable<CreateArtResponseDto>> StreetcodeNotFoundError(CreateArtRequestDto request)
    {
        string errorMsg = string.Format(
            ErrorMessages.EntityByIdNotFound,
            typeof(StreetcodeContent).Name,
            request.StreetcodeId);
        _logger.LogError(request, errorMsg);

        return Result.Fail(errorMsg);
    }

    private Result<IEnumerable<CreateArtResponseDto>> ImageNotFoundError(CreateArtRequestDto request)
    {
        string errorMsg = string.Format(
            ErrorMessages.EntityByIdNotFound,
            typeof(ImageEntity).Name,
            request.ImageId);
        _logger.LogError(request, errorMsg);

        return Result.Fail(errorMsg);
    }

    private Result<IEnumerable<CreateArtResponseDto>> ArtExistsError(CreateArtRequestDto request)
    {
        string errorMsg = $"Art with image (Id: {request.ImageId}) already exists.";
        _logger.LogError(request, errorMsg);

        return Result.Fail(errorMsg);
    }

    private async Task<bool> ArtWithThisImageAlreadyExists(int imageId)
    {
        var art = await _repositoryWrapper.ArtRepository
            .GetFirstOrDefaultAsync(a => a.ImageId == imageId);

        return art != null;
    }

    private async Task<bool> StreetcodeExistsAsync(int streetcodeId)
    {
        var streetcode = await _repositoryWrapper.StreetcodeRepository
            .GetFirstOrDefaultAsync(s => s.Id == streetcodeId);

        return streetcode is not null;
    }

    private async Task<bool> ImageExistsAsync(int imageId)
    {
        var image = await _repositoryWrapper.ImageRepository
            .GetFirstOrDefaultAsync(i => i.Id == imageId);

        return image is not null;
    }
}
