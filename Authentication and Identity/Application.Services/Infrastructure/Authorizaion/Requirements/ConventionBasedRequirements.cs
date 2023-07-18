using Application.Services.Model.TypeSafe;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Infrastructure.Authorizaion.Requirements
{
    public class ConventionBasedRequirements : IAuthorizationRequirement
    {
    }

    public class ConventionBasedRequirementHandler : AuthorizationHandler<ConventionBasedRequirements>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ConventionBasedRequirementHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ConventionBasedRequirements requirement)
        {
            var controllerName = _httpContextAccessor.HttpContext.GetRouteData().Values["controller"]?.ToString();
            var actionName = _httpContextAccessor.HttpContext.GetRouteData().Values["action"]?.ToString();
            var requieredPermission = AuthorizeHelper.GetActionPermission(actionName);

            var user = context.User;
            var claims = user.Claims;

            var userPermissions = AuthorizeHelper.GetPermissionFromClaim(controllerName, claims);

            if (userPermissions is not null &&
               requieredPermission != TS.Permissions.None &&
               userPermissions.Contains(requieredPermission))
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
