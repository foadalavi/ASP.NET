using Application.Services.Identity;
using Application.Services.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Application.Auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly JwtConfiguration _config;

        public AuthController(
            IAuthService authService,
            IOptions<JwtConfiguration> config
            )
        {
            _authService = authService;
            _config = config.Value;
        }

        [HttpPost("Login")]
        public async Task<string> Login(LoginUser credentials)
        {
            if (credentials == null) {
                throw new ArgumentNullException("Login credentials");
            }
            if (await _authService.Login(credentials))
            {
                return await _authService.GenerateTokenString(credentials.Username, _config);
            }
            return null;
        }
    }
}
