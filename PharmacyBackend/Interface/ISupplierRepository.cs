using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PharmacyBackend.DTOs;

namespace PharmacyBackend.Interface
{
    public interface ISupplierRepository
    {
        Task<List<SupplierRequestDTO>> GetPendingRequestsAsync(string supplierId);
        // Task<string> ConfirmSupplyAsync(int orderItemId, string supplierId);
        // Task<string> SendDrugAsync(int orderItemId, string supplierId);
        Task<string> ConfirmAndSendDrugAsync(int orderItemId, string supplierId);

    }
}