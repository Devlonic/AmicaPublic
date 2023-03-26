using Amica.API.Data.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Amica.API.WebServer.Data.Identity {
    public class AmicaUserValidator : UserValidator<Account> {
        // make sure email is not empty, valid, and unique
        private async Task ValidateEmail(UserManager<Account> manager, Account user, List<IdentityError> errors) {
            var email = await manager.GetEmailAsync(user);
            if ( string.IsNullOrWhiteSpace(email) ) {
                errors.Add(Describer.InvalidEmail(email));
                return;
            }
            if ( !new EmailAddressAttribute().IsValid(email) ) {
                errors.Add(Describer.InvalidEmail(email));
                return;
            }
            var owner = await manager.FindByEmailAsync(email);
            if ( owner != null &&
                !string.Equals(await manager.GetUserIdAsync(owner), await manager.GetUserIdAsync(user)) ) {
                errors.Add(Describer.DuplicateEmail(email));
            }
        }

        public async override Task<IdentityResult> ValidateAsync(UserManager<Account> manager, Account user) {
            if ( manager == null ) {
                throw new ArgumentNullException(nameof(manager));
            }
            if ( user == null ) {
                throw new ArgumentNullException(nameof(user));
            }
            var errors = new List<IdentityError>();

            if ( manager.Options.User.RequireUniqueEmail ) {
                await ValidateEmail(manager, user, errors);
            }

            return errors.Count > 0 ? IdentityResult.Failed(errors.ToArray()) : IdentityResult.Success;
        }
    }
}
