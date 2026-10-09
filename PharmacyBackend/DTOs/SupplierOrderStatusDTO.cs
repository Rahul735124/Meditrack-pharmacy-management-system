using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmacyBackend.DTOs
{
    public class SupplierOrderStatusDTO
    {
        public int OrderItemId { get; set; }
        public string DrugName { get; set; }
        public int Quantity { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
    }
}