using Application.Services.Model.TypeSafe;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;

namespace Application.Services.Infrastructure.Authorizaion
{
    public class CustomAuthorizeAttribute : Attribute, IAsyncAuthorizationFilter
    {
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var controllerName = context.HttpContext.GetRouteData().Values["controller"]?.ToString();
            var actionName = context.HttpContext.GetRouteData().Values["action"]?.ToString();
            var requieredPermission = AuthorizeHelper.GetActionPermission(actionName);

            var claims = context.HttpContext.User.Claims;

            var userPermissions = AuthorizeHelper.GetPermissionFromClaim(controllerName, claims);

            if (userPermissions is not null &&
                requieredPermission != TS.Permissions.None &&
                userPermissions.Contains(requieredPermission))
            {
                return;
            }
            context.Result = new UnauthorizedObjectResult("You dont have access to this functionality.");
        }
    }
}
