using Amica.API.Data.Models;
using Amica.API.WebServer.Data.DTO.Posts;
using Amica.API.WebServer.Data.DTO.Profiles;

namespace Amica.API.WebServer.Data.Repositories.Profiles {
    public interface IProfilesRepository {
        Task<ProfileDTO?> GetProfileByIdAsync(long profileId, long requesterProfileId);
        Task<ProfileDTO?> GetProfileByNickNameAsync(string profileNickName, long requesterProfileId);
        Task<List<ProfileDTO>> MatchProfilesByNickNameAsync(string profileNickName);
    }
}
