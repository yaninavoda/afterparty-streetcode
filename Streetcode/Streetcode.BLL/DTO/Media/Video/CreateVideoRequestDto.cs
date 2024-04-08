namespace Streetcode.BLL.DTO.Media.Video
{
    public sealed record CreateVideoRequestDto(string? Description, string Url, int StreetcodeId);
}
