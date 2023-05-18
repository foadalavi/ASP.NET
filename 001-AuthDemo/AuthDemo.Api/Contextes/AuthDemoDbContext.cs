using AuthDemo.Api.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace AuthDemo.Api.Contextes
{
    public class AuthDemoDbContext:IdentityDbContext
    {
        public AuthDemoDbContext(DbContextOptions options):base(options)
        {
            
        }
        public DbSet<Employee> Employee{ get; set; }
    }
}
