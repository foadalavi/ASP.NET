using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using System.IdentityModel.Tokens.Jwt;

namespace AuthServer.Api.Services
{
    public class Logger
    {

        public static Task Write(HttpContext context, string eventName)
        {
            var loggerFactory = LoggerFactory.Create(b => b.SetMinimumLevel(LogLevel.Trace).AddConsole());
            var logger = loggerFactory.CreateLogger<Program>();
            logger.LogInformation("******************************************");
            var authorizationHeader = context.Request.Headers["Authorization"].FirstOrDefault();

            if (authorizationHeader is null)
                logger.LogInformation($"{eventName}. Not Authenticated");
            else
            {
                string jwtTokenString = authorizationHeader.Replace("Bearer ", "");

                var jwt = new JwtSecurityToken(jwtTokenString);

                logger.LogInformation($"{eventName}. Exp Time: {jwt.ValidTo.ToLongTimeString()}. Time: {DateTime.UtcNow.ToLongTimeString()}");
            }

            return Task.CompletedTask;
        }

    }
}
