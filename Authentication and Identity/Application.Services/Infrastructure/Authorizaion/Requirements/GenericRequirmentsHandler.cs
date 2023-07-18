using Application.Services.Model.TypeSafe;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Infrastructure.Authorizaion.Requirements
{
    public class GenericRequirmentsHandler : IAuthorizationHandler
    {
        public Task HandleAsync(AuthorizationHandlerContext context)
        {
            var requirements = context.PendingRequirements.ToList();

            var claims = context.User.Claims;

            var userPermissions = AuthorizeHelper.GetPermissionFromClaim(TS.Contoller.Module, claims);

            foreach (var requirement in requirements)
            {
                if (requirement is AdminRequirements)
                {
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
                }
                else if (requirement is ContributorRequirements)
                {
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
                }
                else if (requirement is UserRequirements)
                {
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
                }

            }

            return Task.CompletedTask;
        }
    }
}
