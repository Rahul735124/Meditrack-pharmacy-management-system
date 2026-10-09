using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PharmacyBackend.DTOs;
using PharmacyBackend.Interface;

namespace PharmacyBackend.Service
{
    public class SupplierService : ISupplierService
    {
        private readonly ISupplierRepository _supplierRepository;

        public SupplierService(ISupplierRepository supplierRepository)
        {
            _supplierRepository = supplierRepository;
        }

        public async Task<List<SupplierRequestDTO>> GetPendingRequestsAsync(string supplierId)
        {
            return await _supplierRepository.GetPendingRequestsAsync(supplierId);
        }

        // public async Task<string> ConfirmSupplyAsync(int orderItemId, string supplierId)
        // {
        //     return await _supplierRepository.ConfirmSupplyAsync(orderItemId, supplierId);
        // }

        // public async Task<string> SendDrugAsync(int orderItemId, string supplierId)
        // {
        //     return await _supplierRepository.SendDrugAsync(orderItemId, supplierId);
        // }
        public async Task<string> ConfirmAndSendDrugAsync(int orderItemId, string supplierId)
        {
            return await _supplierRepository.ConfirmAndSendDrugAsync(orderItemId, supplierId);
        }



    }
}