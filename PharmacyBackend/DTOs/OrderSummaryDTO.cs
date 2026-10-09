using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmacyBackend.DTOs
{
    public class OrderSummaryDTO
    {
        public List<OrderItemSummaryDTO> OrderItems { get; set; }
        public decimal TotalCost { get; set; }
    }

    public class OrderItemSummaryDTO
    {
        public string DrugName { get; set; }
        public int Quantity { get; set; }
        public decimal PricePerUnit { get; set; }
        public decimal ItemTotalCost { get; set; }
    }
}