using Amica.API.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Buffers.Text;
using System.Resources;
using System.Security.Cryptography;

namespace Amica.API.WebServer.Data.Identity {
    public class AmicaUserManager : UserManager<Account> {
        public AmicaUserManager(
            IUserStore<Account> store,
            IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<Account> passwordHasher,
            IEnumerable<IUserValidator<Account>> userValidators,
            IEnumerable<IPasswordValidator<Account>> passwordValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            IServiceProvider services,
            ILogger<UserManager<Account>> logger) :
            base(
                store,
                optionsAccessor,
                passwordHasher,
                userValidators,
                passwordValidators,
                keyNormalizer,
                errors,
                services,
                logger) {
        }

        private static string NewSecurityStamp() {
            byte[] bytes = new byte[20];
#if NETSTANDARD2_0 || NET461
            _rng.GetBytes(bytes);
#else
            RandomNumberGenerator.Fill(bytes);
#endif
            return Base32.ToBase32(bytes);
        }

        private IUserSecurityStampStore<Account> GetSecurityStore() {
            var cast = Store as IUserSecurityStampStore<Account>;
            if ( cast == null ) {
                throw new NotSupportedException("Resources.StoreNotIUserSecurityStampStore");
            }
            return cast;
        }
        private IUserLockoutStore<Account> GetUserLockoutStore() {
            var cast = Store as IUserLockoutStore<Account>;
            if ( cast == null ) {
                throw new NotSupportedException("Resources.StoreNotIUserLockoutStore");
            }
            return cast;
        }

        private async Task UpdateSecurityStampInternal(Account user) {
            if ( SupportsUserSecurityStamp ) {
                await GetSecurityStore().SetSecurityStampAsync(user, NewSecurityStamp(), CancellationToken);
            }
        }

        public async override Task<IdentityResult> CreateAsync(Account user, string password) {
            ThrowIfDisposed();
            if ( user == null ) {
                throw new ArgumentNullException(nameof(user));
            }
            if ( password == null ) {
                throw new ArgumentNullException(nameof(password));
            }
            var result = await UpdatePasswordHash(user, password, true);
            if ( !result.Succeeded ) {
                return result;
            }
            return await CreateAsync(user);
        }

        public async override Task<IdentityResult> CreateAsync(Account user) {
            ThrowIfDisposed();

            await UpdateSecurityStampInternal(user);

            var result = await ValidateUserAsync(user);
            if ( !result.Succeeded ) {
                return result;
            }

            if ( Options.Lockout.AllowedForNewUsers && SupportsUserLockout ) {
                await GetUserLockoutStore().SetLockoutEnabledAsync(user, true, CancellationToken);
            }

            await UpdateNormalizedEmailAsync(user);

            return await Store.CreateAsync(user, CancellationToken);
        }
    }
}
