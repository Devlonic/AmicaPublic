using Amica.API.Data;
using Amica.API.Data.Models;
using Amica.API.WebServer.ConfigurationManagers;
using Amica.API.WebServer.Data.DTO.Posts;
using Amica.API.WebServer.Data.DTO.Profiles;
using Amica.API.WebServer.Data.Repositories.Followers;
using Amica.API.WebServer.Data.Settings;
using Amica.API.WebServer.Extentions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Amica.API.WebServer.Data.Repositories.Profiles {
    public class ProfilesRepository : SqlRepository<Profile>, IProfilesRepository {
        private readonly IFollowersRepository followers;

        public ProfilesRepository(JsonConfig<Profile> config, ILogger<Repository> logger, AmicaDbContext db, IFollowersRepository followers) : base(config, logger, db) {
            this.followers = followers;
        }

        public async Task<ProfileDTO?> GetProfileByIdAsync(long profileId, long requesterProfileId) {
            var raw = await db.Profiles.FindAsync(profileId);
            if ( raw is null )
                return null;

            var res = new ProfileDTO(raw);

            // remake to repository
            res.CountPosts = await db.Posts.CountAsync(p => p.ID_Author == raw.ID);
            res.CountFollowing = await followers.GetCountFollowing(raw.ID);
            res.CountFollowers = await followers.GetCountFollowers(raw.ID);
            res.IsRequesterOwnsProfile = profileId == requesterProfileId;
            res.IsRequesterFollowsProfile =
                profileId == requesterProfileId ?
                false :
                await followers.IsFollow(requesterProfileId, profileId);
            return res;
        }

        public async Task<ProfileDTO?> GetProfileByNickNameAsync(string profileNickName, long requesterProfileId) {
            var profile =
                await db.Profiles.
                Where(p => p.NickName == profileNickName).
                SingleOrDefaultAsync();

            if ( profile is null )
                return null;

            var res = new ProfileDTO(profile);

            // remake to repository
            res.CountPosts = await db.Posts.CountAsync(p => p.ID_Author == profile.ID);
            res.CountFollowing = await followers.GetCountFollowing(profile.ID);
            res.CountFollowers = await followers.GetCountFollowers(profile.ID);

            res.IsRequesterOwnsProfile = profile.ID == requesterProfileId;
            res.IsRequesterFollowsProfile =
                profile.ID == requesterProfileId ?
                false :
                await followers.IsFollow(requesterProfileId, profile.ID);
            return res;
        }

        public async Task<List<ProfileDTO>> MatchProfilesByNickNameAsync(string profileNickName) {
            var result = await
                db.Profiles.Where(
                    p => p.NickName.ToLower().Contains(profileNickName.ToLower())).
                    AsNoTracking().Select(p => new ProfileDTO(p)).
                    Take(config["ScearchLimit", true]).
                    ToListAsync();
            return result;
        }
    }
}
