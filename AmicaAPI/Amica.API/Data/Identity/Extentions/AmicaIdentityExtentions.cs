using Amica.API.Data.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Amica.API.WebServer.Data.Identity.Extentions {
    public static class AmicaIdentityExtentions {
        /// <summary>
        /// Adds and configures the identity system for the specified User and Role types.
        /// </summary>
        /// <param name="services">The services available in the application.</param>
        /// <param name="setupAction">An action to configure the <see cref="IdentityOptions"/>.</param>
        /// <returns>An <see cref="IdentityBuilder"/> for creating and configuring the identity system.</returns>
        public static IdentityBuilder AddAmicaIdentity(
            this IServiceCollection services,
            Action<IdentityOptions> setupAction) {
            // Services used by identity
            services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
                options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
                options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
            })
            .AddCookie(IdentityConstants.ApplicationScheme, o => {
                o.LoginPath = new PathString("/Account/Login");
                o.Events = new CookieAuthenticationEvents {
                    OnValidatePrincipal = SecurityStampValidator.ValidatePrincipalAsync
                };
            })
            .AddCookie(IdentityConstants.ExternalScheme, o => {
                o.Cookie.Name = IdentityConstants.ExternalScheme;
                o.ExpireTimeSpan = TimeSpan.FromMinutes(5);
            })
            .AddCookie(IdentityConstants.TwoFactorRememberMeScheme, o => {
                o.Cookie.Name = IdentityConstants.TwoFactorRememberMeScheme;
                o.Events = new CookieAuthenticationEvents {
                    OnValidatePrincipal = SecurityStampValidator.ValidateAsync<ITwoFactorSecurityStampValidator>
                };
            })
            .AddCookie(IdentityConstants.TwoFactorUserIdScheme, o => {
                o.Cookie.Name = IdentityConstants.TwoFactorUserIdScheme;
                o.ExpireTimeSpan = TimeSpan.FromMinutes(5);
            });

            // Hosting doesn't add IHttpContextAccessor by default
            services.AddHttpContextAccessor();
            // Identity services
            services.TryAddScoped<IUserValidator<Account>, AmicaUserValidator>();
            services.TryAddScoped<IPasswordValidator<Account>, PasswordValidator<Account>>();
            services.TryAddScoped<IPasswordHasher<Account>, PasswordHasher<Account>>();
            services.TryAddScoped<ILookupNormalizer, UpperInvariantLookupNormalizer>();
            services.TryAddScoped<IRoleValidator<IdentityRole>, RoleValidator<IdentityRole>>();
            // No interface for the error describer so we can add errors without rev'ing the interface
            services.TryAddScoped<IdentityErrorDescriber>();
            services.TryAddScoped<ISecurityStampValidator, SecurityStampValidator<Account>>();
            services.TryAddScoped<ITwoFactorSecurityStampValidator, TwoFactorSecurityStampValidator<Account>>();
            services.TryAddScoped<IUserClaimsPrincipalFactory<Account>, UserClaimsPrincipalFactory<Account, IdentityRole>>();
            services.TryAddScoped<IUserConfirmation<Account>, DefaultUserConfirmation<Account>>();
            services.TryAddScoped<UserManager<Account>, AmicaUserManager>();
            services.TryAddScoped<SignInManager<Account>>();
            services.TryAddScoped<RoleManager<IdentityRole>>();

            if ( setupAction != null ) {
                services.Configure(setupAction);
            }

            return new IdentityBuilder(typeof(Account), typeof(IdentityRole), services);
        }
        //public static IdentityBuilder AddAmicaIdentity(
        //this IServiceCollection services,
        //Action<IdentityOptions> setupAction) {
        //    // Services used by identity
        //    services.AddAuthentication(options => {
        //        options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
        //        options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
        //        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
        //    })
        //    .AddCookie(IdentityConstants.ApplicationScheme, o => {
        //        o.LoginPath = new PathString("/Account/Login");
        //        o.Events = new CookieAuthenticationEvents {
        //            OnValidatePrincipal = SecurityStampValidator.ValidatePrincipalAsync
        //        };
        //    })
        //    .AddCookie(IdentityConstants.ExternalScheme, o => {
        //        o.Cookie.Name = IdentityConstants.ExternalScheme;
        //        o.ExpireTimeSpan = TimeSpan.FromMinutes(5);
        //    })
        //    .AddCookie(IdentityConstants.TwoFactorRememberMeScheme, o => {
        //        o.Cookie.Name = IdentityConstants.TwoFactorRememberMeScheme;
        //        o.Events = new CookieAuthenticationEvents {
        //            OnValidatePrincipal = SecurityStampValidator.ValidateAsync<ITwoFactorSecurityStampValidator>
        //        };
        //    })
        //    .AddCookie(IdentityConstants.TwoFactorUserIdScheme, o => {
        //        o.Cookie.Name = IdentityConstants.TwoFactorUserIdScheme;
        //        o.ExpireTimeSpan = TimeSpan.FromMinutes(5);
        //    });

        //    // Hosting doesn't add IHttpContextAccessor by default
        //    services.AddHttpContextAccessor();
        //    // Identity services
        //    services.AddScoped<IUserValidator<Account>, UserValidator<Account>>();
        //    services.AddScoped<IPasswordValidator<Account>, PasswordValidator<Account>>();
        //    services.AddScoped<IPasswordHasher<Account>, PasswordHasher<Account>>();
        //    services.AddScoped<ILookupNormalizer, UpperInvariantLookupNormalizer>();
        //    services.AddScoped<IRoleValidator<IdentityRole>, RoleValidator<IdentityRole>>();
        //    // No interface for the error describer so we can add errors without rev'ing the interface
        //    services.AddScoped<IdentityErrorDescriber>();
        //    services.AddScoped<ISecurityStampValidator, SecurityStampValidator<Account>>();
        //    services.AddScoped<ITwoFactorSecurityStampValidator, TwoFactorSecurityStampValidator<Account>>();
        //    services.AddScoped<IUserClaimsPrincipalFactory<Account>, UserClaimsPrincipalFactory<Account, IdentityRole>>();
        //    services.AddScoped<IUserConfirmation<Account>, DefaulAccountConfirmation<Account>>();
        //    services.AddScoped<AmicaUserManager>();
        //    services.AddScoped<SignInManager<Account>>();
        //    services.AddScoped<RoleManager<IdentityRole>>();

        //    if ( setupAction != null ) {
        //        services.Configure(setupAction);
        //    }

        //    return new IdentityBuilder(typeof(Account), typeof(IdentityRole), services);
        //}
    }
}
