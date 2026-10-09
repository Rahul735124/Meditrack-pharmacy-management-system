using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmacyBackend.DTOs
{
    public class SupplierResponseDTO
    {
        public int Id{get; set;}
        public string Name { get; set; }
        public string Email { get; set; }
        public string Contact { get; set; }
    }
}