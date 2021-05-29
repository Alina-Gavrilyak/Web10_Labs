using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contracts.Models {
    public class RequestDeliveryBaseDTO {
        public int Number { get; set; }
        public int WarehouseId { get; set; }
        public int ShopId { get; set; }
    }

    public class RequestDeliveryInputDTO : RequestDeliveryBaseDTO {
        public List<int> ProductIds { get; set; }
    }

    public class RequestDeliveryDTO : RequestDeliveryBaseDTO {
        public int Id { get; set; }
        public virtual WarehouseBaseDTO Warehouse { get; set; }
        public virtual ShopBaseDTO Shop { get; set; }
        public virtual List<ProductBaseDTO> Products { get; set; }
    }
}
