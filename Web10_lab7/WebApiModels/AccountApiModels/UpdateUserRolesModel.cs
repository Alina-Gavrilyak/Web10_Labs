using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiModels.AccountApiModels {
    public class UpdateUserRolesModel {
        public int UserId { get; set; }
        public List<string> Roles { get; set; }
    }
}
