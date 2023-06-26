using Application.Services.Infrastructure.Authorizaion;
using Application.Services.Model.TypeSafe;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace Services.Infrastructure.Authorizaion.Requirements
{
    public class UserRequirements : IAuthorizationRequirement
    {
    }

    public class UserRequirementHandler : AuthorizationHandler<UserRequirements>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, UserRequirements requirement)
        {
            var claims = context.User.Claims;

            var userPermissions = AuthorizeHelper.GetPermissionFromClaim(TS.Contoller.Module, claims);

            if (userPermissions is not null &&
               userPermissions.Contains(TS.Permissions.Read)
               )
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }

            return Task.CompletedTask;
        }
    }
}
