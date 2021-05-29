using DataAccessContracts.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessContracts.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public double Price { get; set; }
        public virtual ICollection<Shop> Shops { get; set; }
        public virtual ICollection<Warehouse> Warehouses { get; set; }
        public virtual ICollection<RequestDelivery> RequestDeliveries { get; set; }
    }
}
