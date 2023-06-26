using Application.Services.Model;
using Application.Services.Infrastructure;
using Application.Services.Model.TypeSafe;

namespace Application.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.Configure<JwtConfiguration>(builder.Configuration.GetSection("Jwt"));
            builder.Services.AddApplicationServices(builder.Configuration.GetConnectionString("DefaultConnection"));
            builder.Services.AddApplicationIdentity();
            builder.Services.AddApplicationJwtAuth(builder.Configuration.GetSection("Jwt").Get<JwtConfiguration>());

            builder.Services.AddApplicationAuthorization();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            await app.SeedDataAsync();

            app.Run();
        }
    }
}