using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contracts.Models {
    public class ProductBaseDTO {
        public int Number { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public double Price { get; set; }
    }

    public class ProductInputDTO : ProductBaseDTO {
        public List<int> ShopIds { get; set; }
        public List<int> WarehouseIds { get; set; }
    }

    public class ProductDTO : ProductBaseDTO {
        public int Id { get; set; }
        public virtual List<ShopBaseDTO> Shops { get; set; }
        public virtual List<RequestDeliveryBaseDTO> RequestDeliveries { get; set; }
        public virtual List<WarehouseBaseDTO> Warehouses { get; set; }
    }
}
