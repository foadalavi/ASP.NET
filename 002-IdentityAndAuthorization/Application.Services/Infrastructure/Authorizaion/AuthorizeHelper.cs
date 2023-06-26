using Application.Services.Helper;
using Application.Services.Model.TypeSafe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Infrastructure.Authorizaion
{
    internal class AuthorizeHelper
    {
        internal static IEnumerable<int> GetPermissionFromClaim(string controllerName, IEnumerable<Claim> claims)
        {
            if (!claims.Any(t => t.Type == controllerName))
            {
                return null;
            }
            return claims.Where(t => t.Type == controllerName).Select(t => t.Value.To<int>());
        }

        internal static int GetActionPermission(string actionName)
        {
            switch (actionName)
            {
                case "Get":
                    return TS.Permissions.Read;
                case "Add":
                    return TS.Permissions.Write;
                case "Update":
                    return TS.Permissions.Update;
                case "Delete":
                    return TS.Permissions.Delete;
                default:
                    return TS.Permissions.None;
            }
        }
    }
}
