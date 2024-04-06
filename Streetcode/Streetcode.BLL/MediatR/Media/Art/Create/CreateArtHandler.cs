using AutoMapper;
using FluentResults;
using MediatR;
using Org.BouncyCastle.Asn1.Ocsp;
using Streetcode.BLL.Dto.Media.Art;
using Streetcode.BLL.DTO.StreetcodeToponym;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.Resources.Errors;
using Streetcode.DAL.Contracts;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Media.Art.Create;

public class CreateArtHandler : IRequestHandler<CreateArtCommand, Result<CreateArtResponseDto>>
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IMapper _mapper;
    private readonly ILoggerService _logger;

    public CreateArtHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILoggerService logger)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<CreateArtResponseDto>> Handle(CreateArtCommand command, CancellationToken cancellationToken)
    {
        var request = command.CreateArtRequestDto;

        if (!await ImageExistsAsync(request.ImageId))
        {
            return ImageNotFoundError(request);
        }

        if (!await StreetcodeExistsAsync(request.StreetcodeId))
        {
            return StreetcodeNotFoundError(request);
        }

        var newArt = _mapper.Map<DAL.Entities.Media.Images.Art>(request);

        _repositoryWrapper.ArtRepository.Create(newArt);

        var isArtSaveSuccessful = await _repositoryWrapper.SaveChangesAsync() > 0;

        if (!isArtSaveSuccessful)
        {
            return FailedToCreateArtError(request);
        }

        var streetcodeArt = new DAL.Entities.Streetcode.StreetcodeArt()
        {
            StreetcodeId = request.StreetcodeId,
            ArtId = newArt.Id,
            Index = await CalculateIndexAsync(request.StreetcodeId),
        };

        if (!await IsPrimarKeyUnique(request.StreetcodeId, newArt.Id))
        {
            return PrimaryKeyIsNotUniqueError(request);
        }

        _repositoryWrapper.StreetcodeArtRepository.Create(streetcodeArt);

        var isStreetcodeArtSaveSuccessful = await _repositoryWrapper.SaveChangesAsync() > 0;

        if (!isStreetcodeArtSaveSuccessful)
        {
            return FailedToCreateStreetcodeArtError(request);
        }

        var artResponseDto = _mapper.Map<CreateArtResponseDto>(newArt);
        artResponseDto.StreetcodeId = request.StreetcodeId;

        return Result.Ok(artResponseDto);
    }

    private async Task<int> CalculateIndexAsync(int streetcodeId)
    {
        var streetcodeArts = await _repositoryWrapper.StreetcodeArtRepository.GetAllAsync(
            predicate: streetcodeArt => streetcodeArt.StreetcodeId == streetcodeId);

        return streetcodeArts != null ?
            streetcodeArts.Max(sa => sa.Index) + 1 :
            1;
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

    private async Task<bool> IsPrimarKeyUnique(int streetcodeId, int artId)
    {
        var streetcodeArt = await _repositoryWrapper.StreetcodeArtRepository
            .GetFirstOrDefaultAsync(st => st.StreetcodeId == streetcodeId && st.ArtId == artId);

        return streetcodeArt is null;
    }

    private Result<CreateArtResponseDto> PrimaryKeyIsNotUniqueError(CreateArtRequestDto request)
    {
        string errorMsg = string.Format(
            ErrorMessages.PrimaryKeyIsNotUnique,
            nameof(DAL.Entities.Streetcode.StreetcodeArt));
        _logger.LogError(request, errorMsg);
        return Result.Fail(errorMsg);
    }

    private Result<CreateArtResponseDto> ImageNotFoundError(CreateArtRequestDto request)
    {
        string errorMsg = string.Format(
            ErrorMessages.EntityByIdNotFound,
            nameof(DAL.Entities.Media.Images.Image),
            request.ImageId);
        _logger.LogError(request, errorMsg);
        return Result.Fail(errorMsg);
    }

    private Result<CreateArtResponseDto> StreetcodeNotFoundError(CreateArtRequestDto request)
    {
        string errorMsg = string.Format(
            ErrorMessages.EntityByIdNotFound,
            nameof(StreetcodeContent),
            request.StreetcodeId);
        _logger.LogError(request, errorMsg);
        return Result.Fail(errorMsg);
    }

    private Result<CreateArtResponseDto> FailedToCreateArtError(CreateArtRequestDto request)
    {
        string errorMsg = string.Format(
                ErrorMessages.CreateFailed,
                nameof(DAL.Entities.Media.Images.Art));
        _logger.LogError(request, errorMsg);

        return Result.Fail(errorMsg);
    }

    private Result<CreateArtResponseDto> FailedToCreateStreetcodeArtError(CreateArtRequestDto request)
    {
        string errorMsg = string.Format(
                ErrorMessages.CreateFailed,
                nameof(DAL.Entities.Streetcode.StreetcodeArt));
        _logger.LogError(request, errorMsg);

        return Result.Fail(errorMsg);
    }
}
