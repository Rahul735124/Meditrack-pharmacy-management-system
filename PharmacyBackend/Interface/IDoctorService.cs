using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PharmacyBackend.DTOs;
using PharmacyBackend.Models;

namespace PharmacyBackend.Interface
{
    public interface IDoctorService
    {
        Task<List<DoctorDrugDTO>> GetAllDrugsAsync();
        Task<OrderSummaryDTO> PlaceOrderAsync(string userId, PlaceOrderDTO placeOrderDTO);

        Task<List<DoctorOrderStatusDTO>> GetMyOrdersAsync(string userId);

        Task<ApplicationUser> GetProfileAsync(string userId);
        Task<bool> UpdateProfileAsync(string userId, UpdateDoctorProfileDTO dto);
    }
}