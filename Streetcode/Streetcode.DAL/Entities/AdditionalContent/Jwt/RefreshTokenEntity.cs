using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Streetcode.DAL.Entities.Users;

namespace Streetcode.DAL.Entities.AdditionalContent.Jwt;

[Table("refresh_tokens", Schema = "account")]
public class RefreshTokenEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime? RefreshTokenExpirationDateTime { get; set; }
    public int ApplicationUserId { get; set; }
    public ApplicationUser? ApplicationUser { get; set; }
}
