namespace Streetcode.BLL.Dto.Streetcode.TextContent.Text
{
    public class TextDto
    {
      public int Id { get; set; }
      public string Title { get; set; }
      public string TextContent { get; set; }
      public int StreetcodeId { get; set; }
      public string? AdditionalText { get; set; }
    }
}
