using Amica.API.Data.Models;
using Amica.API.Data.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore.Storage;
using Amica.API.WebServer.Data.Settings;
using Microsoft.EntityFrameworkCore;
using Amica.API.WebServer.Data.Identity;
using Amica.API.WebServer.Data.Repositories;
using Amica.API.WebServer.ConfigurationManagers;
using System.Security.Claims;
using Amica.API.WebServer.Data.Services;

namespace Amica.API.Data.Repositories {
    public class AccountsRepository : SqlRepository<Account>, IAccountsRepository {
        private readonly ICaptchaValidatorService captcha;
        private readonly RoleManager<IdentityRole> roles;
        private readonly UserManager<Account> accounts;
        private readonly JwtManager jwt;
        private readonly IWebHostEnvironment environment;
        private readonly IImagesRepository images;

        public AccountsRepository(
            ICaptchaValidatorService captcha,
            JsonConfig<Account> config,
            AmicaDbContext db,
            RoleManager<IdentityRole> roles,
            UserManager<Account> accounts,
            JwtManager jwt,
            IWebHostEnvironment environment,
            IImagesRepository images,
            ILogger<AccountsRepository> logger) : base(config, logger, db) {
            this.captcha = captcha;
            this.roles = roles;
            this.accounts = accounts;
            this.jwt = jwt;
            this.environment = environment;
            this.images = images;

            // todo
            Task.Run(async () => {
                var created = await EnsureDefaultRolesExist();
                this.logger.LogDebug($"{created} default roles created");
            }).Wait();
        }

        private async Task<int> EnsureDefaultRolesExist() {
            int countCreated = 0;

            foreach ( var r in AmicaRolesSettings.AllRoles ) {
                if ( ( await CreateRole(r) ).Succeeded == true )
                    countCreated++;
            }

            return countCreated;
        }

        public async Task<IdentityResult> CreateRole(string rolename) {
            if ( await roles.FindByNameAsync(rolename) is not null ) {
                return IdentityResult.Failed(new[] { new IdentityError() {
                    Description = $"Role {rolename} already exists",
                }});
            }

            return await roles.CreateAsync(new IdentityRole(rolename));
        }

        public async Task<SignInResponce> SignInAccount(SignInRequest request) {
            Account? account;
            Profile? profile;

            // check profile existence by request request.Login
            profile = await db.Profiles.Where(p => p.NickName == request.Login).SingleOrDefaultAsync();
            if ( profile is null ) {
                return new SignInResponce() {
                    IsSignedIn = false,
                    Message = $"Wrong login or password",
                };
            }

            // profile with request.Login exist. Getting profile account
            await db.Entry(profile).Reference(p => p.Account).LoadAsync();

            if ( profile.Account is null )
                throw new Exception("Profile not linked to account");

            account = profile.Account;


            // account found. Check request.Password
            if ( await accounts.CheckPasswordAsync(account, request.Password) is false ) {
                // wrong password
                return new SignInResponce() {
                    IsSignedIn = false,
                    Message = $"Wrong login or password",
                };
            }

            // profile exists, linked to account and password correct. Success.
            // Getting token and return
            var res = new SignInResponce() {
                IsSignedIn = true,
                Token = await jwt.GetTokenAsync(profile),
                Profile = new WebServer.Data.DTO.Profiles.ProfileDTO(profile),
            };
            res.Profile.IsRequesterFollowsProfile = false;
            res.Profile.IsRequesterOwnsProfile = true;
            return res;
        }

        public async Task<SignUpResponce> SignUpAccount(SignUpRequest request, string[] roles, bool checkCaptcha = true) {
            IDbContextTransaction? newUserTransaction = null;
            try {
                // ensure existence captcha
                if ( checkCaptcha && string.IsNullOrWhiteSpace(request.CaptchaV2) )
                    return new SignUpResponce() {
                        IsSignedUp = false,
                        Message = $"ReCaptcha {nameof(SignUpRequest.CaptchaV2)} required"
                    };

                // validate captcha
                if ( checkCaptcha && await captcha.IsCaptchaPassedAsync(request.CaptchaV2) == false )
                    return new SignUpResponce() {
                        IsSignedUp = false,
                        Message = $"ReCaptcha {nameof(SignUpRequest.CaptchaV2)} validation failed"
                    };

                var existingProfile = await db.Profiles.Where(p => p.NickName == request.Nickname).SingleOrDefaultAsync();

                if ( existingProfile is not null )
                    throw new Exception($"Profile {request.Nickname} already exists");

                newUserTransaction = await db.Database.BeginTransactionAsync();


                var account = new Account() {
                    Email = request.Email,
                };

                var result = await accounts.CreateAsync(account, request.Password);

                if ( !result.Succeeded ) {
                    string? message = environment.IsDevelopment() ? string.Join("\n", result.Errors.Select(e => $"{e.Code}: {e.Description}")) : null;
                    throw new Exception(message);
                }

                await accounts.AddToRolesAsync(account, roles);

                var profile = new Profile() {
                    Account = account,
                    AccountID = account.Id,
                    FullName = request.FullName,
                    NickName = request.Nickname,
                };

                db.Profiles.Add(profile);

                var uploaded = await images.UploadImageAsync(request.ProfilePhoto);

                profile.ProfilePhoto = uploaded;

                await db.SaveChangesAsync();

                await newUserTransaction.CommitAsync();

                return new SignUpResponce() {
                    IsSignedUp = true,
                    ProfileID = profile.ID
                };
            }
            catch ( Exception e ) {
                logger.LogWarning($"user creating error: {e}");
                if ( newUserTransaction is not null )
                    await newUserTransaction.RollbackAsync();
                return new SignUpResponce() {
                    IsSignedUp = false,
                    Message = e.Message,
                };
            }
        }
    }
}
