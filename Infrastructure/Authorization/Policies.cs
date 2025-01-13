using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Authorization
{
        // Infrastructure/Authorization/Policies.cs
        public static class Policies
        {
            public const string RequireAdminRole = "RequireAdminRole";
            public const string RequireFarmerRole = "RequireFarmerRole";
            public const string RequireBuyerRole = "RequireBuyerRole";
        }
}
