using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmacyBackend.DTOs
{
    public class DrugDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int QuantityAvailable { get; set; }
        public decimal Price { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int SupplierId { get; set; }
        // public string SupplierName { get; set; }

    }
}