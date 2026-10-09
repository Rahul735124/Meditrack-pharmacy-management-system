using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PharmacyBackend.DTOs;
using PharmacyBackend.Interface;
using PharmacyBackend.Models;

namespace PharmacyBackend.Controllers
{
    [ApiController]
    [Route("api/doctor")]
    [Authorize(Roles = "Doctor")]
    public class DoctorController : ControllerBase
    {

        private readonly IDoctorService _doctorService;

        public DoctorController(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        [HttpGet("drugs")]
        public async Task<IActionResult> GetAllDrugs()
        {
            try
            {
                var drugs = await _doctorService.GetAllDrugsAsync();
                return Ok(new ApiResponse<IEnumerable<DoctorDrugDTO>> { Success = true, Data = drugs, Message = "Drugs retrieved successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<string> { Success = false, Data = null, Message = $"Error getting drugs: {ex.Message}" });
            }
        }

        [HttpPost("place-order")]
        public async Task<IActionResult> PlaceOrder([FromBody] PlaceOrderDTO placeOrderDTO)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new ApiResponse<string> { Success = false, Data = null, Message = "User ID not found." });
                }

                var result = await _doctorService.PlaceOrderAsync(userId, placeOrderDTO);
                return Ok(new ApiResponse<object> { Success = true, Data = result, Message = "Order placed successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<string> { Success = false, Data = null, Message = $"Error placing order: {ex.Message}" });
            }
        }

        [HttpGet("my-orders")]
        public async Task<IActionResult> GetMyOrders()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new ApiResponse<string> { Success = false, Data = null, Message = "User ID not found." });
                }

                var result = await _doctorService.GetMyOrdersAsync(userId);
                return Ok(new ApiResponse<IEnumerable<DoctorOrderStatusDTO>> { Success = true, Data = result, Message = "Orders retrieved successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<string> { Success = false, Data = null, Message = $"Error fetching orders: {ex.Message}" });
            }
        }

        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _doctorService.GetProfileAsync(userId);

            if (user == null)
                return NotFound(new { success = false, message = "Doctor not found" });

            return Ok(new
            {
                success = true,
                data = new { user.FullName, user.Contact, user.Email }
            });
        }

        [HttpPut("update-profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateDoctorProfileDTO dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _doctorService.UpdateProfileAsync(userId, dto);

            if (!result)
                return BadRequest(new { success = false, message = "Update failed" });

            return Ok(new { success = true, message = "Profile updated successfully" });
        }

    }
}