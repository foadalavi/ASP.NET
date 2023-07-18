using Application.Services.Model.TypeSafe;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Infrastructure.Authorizaion.Requirements
{
    public class AdminRequirements : IAuthorizationRequirement
    {
    }

    public class AdminRequirementHandler : AuthorizationHandler<AdminRequirements>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AdminRequirements requirement)
        {
            var claims = context.User.Claims;

            var userPermissions = AuthorizeHelper.GetPermissionFromClaim(TS.Contoller.Module, claims);

            if (userPermissions is not null &&
               userPermissions.Contains(TS.Permissions.Read) &&
               userPermissions.Contains(TS.Permissions.Write) &&
               userPermissions.Contains(TS.Permissions.Update) &&
               userPermissions.Contains(TS.Permissions.Delete)
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
