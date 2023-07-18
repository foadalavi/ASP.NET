using Application.Services.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.DataContexts
{
    public class UserIdentityContext : IdentityDbContext
    {
        public UserIdentityContext(DbContextOptions<UserIdentityContext> options) : base(options)
        {
        }
    }
}
