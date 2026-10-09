using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmacyBackend.DTOs
{
    public class OrderDTO
    {
        public int OrderId { get; set; }
        public string UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public bool IsVerified { get; set; }
        public bool IsPickedUp { get; set; }

        public List<OrderItemsResponseDTO> OrderItems { get; set; } = new();
    }

    public class OrderItemsResponseDTO
    {
        public int OrderItemId{ get; set; }
        public int DrugId { get; set; }
        public string DrugName { get; set; }
        public int Quantity { get; set; }
        public bool IsInStock{get; set;}
        public bool IsRequestedFromSupplier{get; set;}
    }
}