using Microsoft.AspNetCore.Identity;

namespace AuthServer.Api.Models
{
    public class ExtendedIdentityUser : IdentityUser
    {
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiry { get; set; }
    }
}