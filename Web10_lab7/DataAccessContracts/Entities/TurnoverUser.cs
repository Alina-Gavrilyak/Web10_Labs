using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessContracts.Entities {
    public class TurnoverUser : IdentityUser<int> {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public virtual ICollection<TurnoverRole> Roles { get; set; }
    }
}
