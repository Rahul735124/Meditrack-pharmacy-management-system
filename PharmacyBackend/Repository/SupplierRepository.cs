using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PharmacyBackend.Data;
using PharmacyBackend.DTOs;
using PharmacyBackend.Interface;

namespace PharmacyBackend.Repository
{
    public class SupplierRepository : ISupplierRepository
    {
        private readonly PharmacyDbContext _context;

        public SupplierRepository(PharmacyDbContext context)
        {
            _context = context;
        }

        public async Task<List<SupplierRequestDTO>> GetPendingRequestsAsync(string supplierId)
        {
            var supplier = await _context.Suppliers.FirstOrDefaultAsync(s => s.UserId == supplierId);

            if (supplier == null)
                return new List<SupplierRequestDTO>();

            var requests = await _context.OrderItems
                .Include(oi => oi.Drug)
                .Where(oi => oi.Drug.SupplierId == supplier.SupplierId
                          && oi.IsRequestedFromSupplier == true
                          && oi.IsSupplierConfirmed == false)
                .Select(oi => new SupplierRequestDTO
                {
                    OrderItemId = oi.OrderItemId,
                    DrugName = oi.Drug.Name,
                    Quantity = oi.Quantity,
                    IsSupplierConfirmed = oi.IsSupplierConfirmed
                })
                .ToListAsync();

            return requests;
        }

        // public async Task<string> ConfirmSupplyAsync(int orderItemId, string supplierId)
        // {
        //     var supplier = await _context.Suppliers.FirstOrDefaultAsync(s => s.UserId == supplierId);

        //     if (supplier == null)
        //         return "Supplier not found.";

        //     var orderItem = await _context.OrderItems
        //         .Include(oi => oi.Drug)
        //         .FirstOrDefaultAsync(oi => oi.OrderItemId == orderItemId);

        //     if (orderItem == null)
        //         return "Order item not found.";

        //     if (orderItem.Drug.SupplierId != supplier.SupplierId)
        //         return "You are not authorized to confirm this drug.";

        //     orderItem.IsSupplierConfirmed = true;
        //     await _context.SaveChangesAsync();

        //     return "Supply confirmed successfully.";
        // }

        // public async Task<string> SendDrugAsync(int orderItemId, string supplierId)
        // {
        //     var supplier = await _context.Suppliers.FirstOrDefaultAsync(s => s.UserId == supplierId);

        //     if (supplier == null)
        //         return "Supplier not found.";

        //     var orderItem = await _context.OrderItems
        //         .Include(oi => oi.Drug)
        //         .FirstOrDefaultAsync(oi => oi.OrderItemId == orderItemId);

        //     if (orderItem == null)
        //         return "Order item not found.";

        //     if (orderItem.Drug.SupplierId != supplier.SupplierId)
        //         return "Unauthorized for this drug.";

        //     if (orderItem.IsSentBySupplier)
        //         return "Already sent.";

        //     orderItem.IsSentBySupplier = true;
        //     orderItem.Drug.QuantityAvailable += orderItem.Quantity;

        //     await _context.SaveChangesAsync();

        //     return "Drug sent and inventory updated successfully.";
        // }
        public async Task<string> ConfirmAndSendDrugAsync(int orderItemId, string supplierId)
        {
            try
            {
                var supplier = await _context.Suppliers.FirstOrDefaultAsync(s => s.UserId == supplierId);
                if (supplier == null)
                    return "Supplier not found.";

                var orderItem = await _context.OrderItems
                    .Include(oi => oi.Drug)
                    .FirstOrDefaultAsync(oi => oi.OrderItemId == orderItemId);

                if (orderItem == null)
                    return "Order item not found.";

                if (orderItem.Drug.SupplierId != supplier.SupplierId)
                    return "Unauthorized access.";

                if (orderItem.IsSupplierConfirmed)
                    return "Already confirmed.";

                // Confirm supply
                orderItem.IsSupplierConfirmed = true;
                orderItem.IsSentBySupplier = true;

                // Update inventory
                orderItem.Drug.QuantityAvailable += orderItem.Quantity;

                await _context.SaveChangesAsync();
                return "Order confirmed and drug quantity updated.";
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }


    }
}