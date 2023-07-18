using Application.Services.Model.TypeSafe;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace Application.Services.Infrastructure.Authorizaion.Requirements
{
    public class ContributorRequirements : IAuthorizationRequirement
    {

    }
    public class ContributorRequirementHandler : AuthorizationHandler<ContributorRequirements>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ContributorRequirements requirement)
        {
            var claims = context.User.Claims;
            var userPermissions = AuthorizeHelper.GetPermissionFromClaim(TS.Contoller.Module, claims);

            if (userPermissions is not null &&
               userPermissions.Contains(TS.Permissions.Read) &&
               userPermissions.Contains(TS.Permissions.Write)
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
