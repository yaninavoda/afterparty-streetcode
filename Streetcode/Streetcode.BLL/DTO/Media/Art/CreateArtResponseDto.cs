namespace Streetcode.BLL.Dto.Media.Art;

public class CreateArtResponseDto
{
    public int Id { get; set; }
    public string? Description { get; set; }
    public string? Title { get; set; }
    public int ImageId { get; set; }
    public int StreetcodeId { get; set; }
}