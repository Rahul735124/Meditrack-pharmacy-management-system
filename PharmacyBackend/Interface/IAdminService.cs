using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PharmacyBackend.DTOs;
using PharmacyBackend.Models;

namespace PharmacyBackend.Interface
{
    public interface IAdminService
    {
        Task<ApiResponse<string>> AddSupplierAsync(SupplierDTO supplierDto);
        Task<List<DrugResponseDTO>> GetAllDrugsAsync();
        Task<string> AddDrugAsync(DrugDTO drugDto);
        Task<List<OrderDTO>> GetNewOrdersAsync();
        Task<string> ClassifyOrderItemAsync(int orderItemId);
        Task<string> VerifyOrderAsync(int orderId);
        Task<string> PickOrderAsync(int orderId);
        Task<List<SupplierResponseDTO>> GetAllSuppliersAsync();
        Task<string> EditDrugAsync(int drugId, DrugDTO dto);
        Task<string> DeleteDrugAsync(int drugId);
        Task<string> EditSupplierAsync(int supplierId, SupplierDTO dto);
        Task<string> DeleteSupplierAsync(int supplierId);
        Task<SalesReportResponseDTO> GetSalesReportAsync(DateTime startDate, DateTime endDate);

        Task<List<SupplierOrderStatusDTO>> GetSupplierOrderItemStatusesAsync();
        Task<List<OrderDTO>> GetVerifiedOrdersAsync();
        Task<IEnumerable<DrugResponseDTO>> GetExpiredDrugsAsync();

        

    }
}