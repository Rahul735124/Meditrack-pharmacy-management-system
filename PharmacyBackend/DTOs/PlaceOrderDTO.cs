using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmacyBackend.DTOs
{
    public class PlaceOrderDTO
    {
        public List<OrderItemDTO> OrderItems { get; set; }
    }

    public class OrderItemDTO
    {
        public int DrugId { get; set; }
        public int Quantity { get; set; }
    }
}