using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PharmacyBackend.DTOs;
using PharmacyBackend.Interface;
using PharmacyBackend.Models;

namespace PharmacyBackend.Service
{
    public class DoctorService : IDoctorService
    {
        private readonly IDoctorRepository _doctorRepository;

        public DoctorService(IDoctorRepository doctorRepository)
        {
            _doctorRepository = doctorRepository;
        }

        public async Task<List<DoctorDrugDTO>> GetAllDrugsAsync()
        {
            return await _doctorRepository.GetAllDrugsAsync();
        }

        public async Task<OrderSummaryDTO> PlaceOrderAsync(string userId, PlaceOrderDTO placeOrderDTO)
        {
            return await _doctorRepository.PlaceOrderAsync(userId, placeOrderDTO);
        }

        public async Task<List<DoctorOrderStatusDTO>> GetMyOrdersAsync(string userId)
        {
            return await _doctorRepository.GetMyOrdersAsync(userId);
        }

        public async Task<ApplicationUser> GetProfileAsync(string userId)
        {
            return await _doctorRepository.GetDoctorByIdAsync(userId);
        }

        public async Task<bool> UpdateProfileAsync(string userId, UpdateDoctorProfileDTO dto)
        {
            var user = await _doctorRepository.GetDoctorByIdAsync(userId);
            if (user == null) return false;

            user.FullName = dto.FullName;
            user.Contact = dto.Contact;

            return await _doctorRepository.UpdateDoctorProfileAsync(user, dto.Email);
        }

    }
}