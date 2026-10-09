using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmacyBackend.DTOs
{
    // public class SalesReportDTO
    // {
    //     public int OrderId { get; set; }
    //     public DateTime OrderDate { get; set; }
    //     public List<SalesReportItemDTO> Items { get; set; }
    //     public decimal TotalOrderAmount { get; set; }
    // }
    // public class SalesReportItemDTO
    // {
    //     public string DrugName { get; set; }
    //     public int Quantity { get; set; }
    //     public decimal PricePerUnit { get; set; }
    //     public decimal ItemTotalPrice { get; set; }
    // }
    public class SalesReportDTO
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public List<SalesReportItemDTO> Items { get; set; }
        public decimal TotalOrderAmount { get; set; }

        public string DoctorName { get; set; }
        public string DoctorEmail { get; set; }
        public string DoctorContact { get; set; }
    }

    public class SalesReportItemDTO
    {
        public string DrugName { get; set; }
        public int Quantity { get; set; }
        public decimal PricePerUnit { get; set; }
        public decimal ItemTotalPrice { get; set; }
    }
}