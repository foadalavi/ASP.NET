using Application.Services.Helper;
using Application.Services.Model;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Application.Services.Identity
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IHttpContextAccessor _httpContext;

        public AuthService(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IHttpContextAccessor httpContext)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _httpContext = httpContext;
        }

        public async Task<bool> Login(LoginUser credentials)
        {
            var user = await _userManager.FindByNameAsync(credentials.Username);
            if (user != null)
            {
                return await _userManager.CheckPasswordAsync(user, credentials.Password);
            }
            return false;
        }

        public async Task<bool> RegisterUser(LoginUser user)
        {
            var identityUser = new IdentityUser
            {
                UserName = user.Username,
                Email = user.Username
            };

            var result = await _userManager.CreateAsync(identityUser, user.Password);
            return result.Succeeded;
        }

        public async Task Logout()
        {
            await _httpContext.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        public async Task GenerateCookieAuthentication(string username)
        {
            var claims = await GetClaims(username);

            ClaimsIdentity identity =
                new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            ClaimsPrincipal pricipal = new ClaimsPrincipal(identity);
            _httpContext.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, pricipal);
        }

        public async Task<bool> AddUserClaim(string user, Claim claim)
        {
            var identityUser = await _userManager.FindByNameAsync(user);
            if (identityUser is null)
            {
                return false;
            }

            var result = await _userManager.AddClaimAsync(identityUser, claim);
            return result.Succeeded;
        }

        private async Task<List<Claim>> GetClaims(string username)
        {
            var user = await _userManager.FindByNameAsync(username);

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, username),
            };

            claims.AddRange(GetClaimsSeperated(await _userManager.GetClaimsAsync(user)));
            var roles = await _userManager.GetRolesAsync(user);

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));

                var identityRole = await _roleManager.FindByNameAsync(role);
                claims.AddRange(GetClaimsSeperated(await _roleManager.GetClaimsAsync(identityRole)));
            }

            return claims;
        }

        private List<Claim> GetClaimsSeperated(IList<Claim> claims)
        {
            var result = new List<Claim>();
            foreach (var claim in claims)
            {
                result.AddRange(claim.DeserializePermissions().Select(t => new Claim(claim.Type, t.ToString())));
            }
            return result;
        }

        public async Task<string> GenerateTokenString(string user, JwtConfiguration jwtConfig)
        {
            var claims = await GetClaims(user);

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.Key));

            var signingCred = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);

            var securityToken = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(60),
                issuer: jwtConfig.Issuer,
                audience: jwtConfig.Audience,
                signingCredentials: signingCred);

            string tokenString = new JwtSecurityTokenHandler().WriteToken(securityToken);
            return tokenString;
        }
    }
}
