using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models {
    public class AuthSettings {
        public string Key { get; set; }
        public int ExpirationMinutes { get; set; }
    }
}
