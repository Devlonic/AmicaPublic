using Amica.API.Data.Models;
using Amica.API.WebServer.ConfigurationManagers;
using Amica.API.WebServer.Data.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Amica.API.Data {
    public class JwtManager {
        private readonly JwtConfig jwt;
        private readonly UserManager<Account> accounts;

        public JwtManager(JwtConfig jwt, UserManager<Account> accounts) {
            this.jwt = jwt;
            this.accounts = accounts;
        }

        public async Task<string> GetTokenAsync(Profile profile) {
            return @$"{jwt["Scheme"]} {new JwtSecurityTokenHandler().
                WriteToken(await GetSecurityTokenAsync(profile))}";
        }

        private async Task<JwtSecurityToken> GetSecurityTokenAsync(Profile profile) {
            var token = new JwtSecurityToken(
                issuer: jwt["Issuer"],
                audience: jwt["Audience"],
                expires: DateTime.Now.AddMinutes(jwt["ExpirationTimeInMinutes", true]),
                claims: await GetClaimsAsync(profile),
                signingCredentials: GetSigningCredentials());

            return token;
        }

        private SigningCredentials GetSigningCredentials() {
            var key = Encoding.UTF8.GetBytes(jwt["SecurityKey"]);
            var secret = new SymmetricSecurityKey(key);

            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }


        private async Task<IEnumerable<Claim>> GetClaimsAsync(Profile profile) {
            var claims = new List<Claim>();
            foreach ( var role in await accounts.GetRolesAsync(profile.Account ?? throw new Exception("Profile not linked to account")) ) {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            claims.Add(new Claim(JwtClaims.TokenIdentity, profile.ID.ToString() ?? null!));
            claims.Add(new Claim(JwtClaims.TokenIdentityAccount, profile.Account.Id.ToString() ?? null!));
            return claims;
        }
    }
}
