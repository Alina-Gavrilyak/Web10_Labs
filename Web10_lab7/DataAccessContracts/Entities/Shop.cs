using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessContracts.Entities {
    public class Shop {
        public int Id { get; set; }
        public string Address { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<RequestDelivery> RequestDeliveries { get; set; }
    }
}
