using Application.Services.Infrastructure;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Application.Ui
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddApplicationServices(builder.Configuration.GetConnectionString("DefaultConnection"));
            builder.Services.AddApplicationIdentity();
            builder.Services.AddApplicationCookieAuth();

            // Add services to the container.
            builder.Services.AddRazorPages();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapRazorPages();

            await app.SeedDataAsync();

            app.Run();
        }
    }
}