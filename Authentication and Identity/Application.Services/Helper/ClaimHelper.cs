using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Helper
{
    internal static  class ClaimHelper
    {
        public static string SerializePermissions(params int[] permissions)
        {
            return permissions.Serialize();
        }

        public static List<int> DeserializePermissions(this Claim claim)
        {
            return claim.Value.Deserialize<List<int>>();
        }
    }
}
