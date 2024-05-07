namespace Streetcode.BLL.Dto.Media.Art;

public class CreateArtsRequestDto
{
    public ICollection<CreateArtRequestDto> Arts { get; set; } = new List<CreateArtRequestDto>();
}
