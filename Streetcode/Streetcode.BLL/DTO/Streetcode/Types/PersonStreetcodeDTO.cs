namespace Streetcode.BLL.Dto.Streetcode.Types
{
    public class PersonStreetcodeDto : StreetcodeDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string? Rank { get; set; }
        public string LastName { get; set; } = string.Empty;
    }
}