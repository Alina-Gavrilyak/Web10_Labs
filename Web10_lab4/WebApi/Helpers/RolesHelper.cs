using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Helpers {
    public static class RolesHelper {
        public const string SystemAdminRole = "System Admin";
        public const string AdminRole = "Admin";
        public const string UserRole = "User";

        public static Dictionary<string, string> Roles = new Dictionary<string, string>() {
            { SystemAdminRole, "System Administrator Role" },
            { AdminRole, "Administrator Role" },
            { UserRole, "User Role" }
        };
    }
}
