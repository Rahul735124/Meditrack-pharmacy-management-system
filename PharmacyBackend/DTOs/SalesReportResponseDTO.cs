using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmacyBackend.DTOs
{
    public class SalesReportResponseDTO
    {
        public decimal TotalSales { get; set; }
        public List<SalesReportDTO> Orders { get; set; }
    }
}