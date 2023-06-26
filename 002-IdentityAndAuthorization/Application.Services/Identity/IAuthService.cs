using Application.Services.Model;
using System.Security.Claims;

namespace Application.Services.Identity
{
    public interface IAuthService
    {
        Task<bool> AddUserClaim(string user, Claim claim);
        Task GenerateCookieAuthentication(string username);
        Task<string> GenerateTokenString(string user, JwtConfiguration jwtConfig);
        Task<bool> Login(LoginUser credentials);
        Task Logout();
        Task<bool> RegisterUser(LoginUser user);
    }
}