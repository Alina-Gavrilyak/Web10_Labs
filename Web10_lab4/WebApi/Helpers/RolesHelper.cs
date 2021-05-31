using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Helpers {
    public static class RolesHelper {
        public const string AdminRole = "Admin";
        public const string UserRole = "User";

        public static Dictionary<string, string> Roles = new Dictionary<string, string>() {
            { AdminRole, "System Administrator Role" },
            { UserRole, "User Role" }
        };
    }
}
