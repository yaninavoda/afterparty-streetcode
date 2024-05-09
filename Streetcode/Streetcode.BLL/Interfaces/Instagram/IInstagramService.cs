using Streetcode.BLL.Entities.Instagram;

namespace Streetcode.BLL.Interfaces.Instagram
{
    public interface IInstagramService
    {
        Task<IEnumerable<InstagramPost>> GetPostsAsync();
    }
}
