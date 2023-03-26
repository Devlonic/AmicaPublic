using Amica.API.Data.Models;
using Amica.API.Data.DTO;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Amica.API.Data.Repositories {
    public interface IAccountsRepository {
        Task<SignUpResponce> SignUpAccount(SignUpRequest request, string[] roles, bool checkCaptcha = true);
        Task<SignInResponce> SignInAccount(SignInRequest request);
        Task<IdentityResult> CreateRole(string rolename);
    }
}
