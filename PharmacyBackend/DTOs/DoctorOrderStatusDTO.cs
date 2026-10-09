using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmacyBackend.DTOs
{
    public class DoctorOrderStatusDTO
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public bool IsVerified { get; set; }
        public bool IsPickedUp { get; set; }
        public List<DoctorOrderItemDTO> Items { get; set; }
    }

    public class DoctorOrderItemDTO
    {
        public string DrugName { get; set; }
        public int Quantity { get; set; }
        public decimal Price{ get; set; }
    }
}