using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessContracts.Entities {
    public class TurnoverRole : IdentityRole<int> {
        public string Description { get; set; }
        public virtual ICollection<TurnoverUser> Users { get; set; }
    }
}
