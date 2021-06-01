using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessContracts.Entities {
    public class Warehouse {
        public int Id { get; set; }
        public int Number { get; set; }
        public string Address { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<RequestDelivery> RequestDeliveries { get; set; }
    }
}
