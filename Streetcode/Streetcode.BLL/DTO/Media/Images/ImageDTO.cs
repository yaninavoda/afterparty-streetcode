using Streetcode.BLL.Dto.Streetcode;

namespace Streetcode.BLL.Dto.Media.Images
{
    public class ImageDto
    {
        public int Id { get; set; }

        public string? BlobName { get; set; }
        public string? Base64 { get; set; }
        public string? MimeType { get; set; }
        public ImageDetailsDto? ImageDetails { get; set; }
    }
}
