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
    [Route("api/supplier")]
    [Authorize(Roles = "Supplier")]
    public class SupplierController : ControllerBase
    {
        private readonly ISupplierService _supplierService;

        public SupplierController(ISupplierService supplierService)
        {
            _supplierService = supplierService;
        }

        [HttpGet("requests")]
        public async Task<IActionResult> GetPendingRequests()
        {
            try
            {
                var supplierId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var requests = await _supplierService.GetPendingRequestsAsync(supplierId);
                return Ok(new ApiResponse<IEnumerable<SupplierRequestDTO>> { Success = true, Data = requests, Message = "Pending requests retrieved successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<string> { Success = false, Data = null, Message = $"Error fetching requests: {ex.Message}" });
            }
        }

        [HttpPost("confirm-and-send/{orderItemId}")]
        public async Task<IActionResult> ConfirmAndSendDrug(int orderItemId)
        {
            try
            {
                var supplierId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var result = await _supplierService.ConfirmAndSendDrugAsync(orderItemId, supplierId);
                return Ok(new ApiResponse<object> { Success = true, Data = result, Message = "Drug confirmed and sent successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<string> { Success = false, Data = null, Message = $"Error confirming and sending drug: {ex.Message}" });
            }
        }

    }
}