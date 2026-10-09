using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PharmacyBackend.DTOs;
using PharmacyBackend.Interface;
using PharmacyBackend.Models;

namespace PharmacyBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpPost("add-supplier")]
        public async Task<IActionResult> AddSupplier([FromBody] SupplierDTO supplierDto)
        {
            var response = await _adminService.AddSupplierAsync(supplierDto);

            if (!response.Success)
            {
                if (response.Message.Contains("email already exists"))
                    return Conflict(response); // 409 Conflict

                if (response.Message.Contains("Failed to create"))
                    return BadRequest(response); // 400 Bad Request

                return StatusCode(500, response); // 500 Internal Server Error
            }

            return Ok(response); // 200 OK for success
        }

        [HttpGet("drugs")]
        public async Task<IActionResult> GetAllDrugs()
        {
            var data = await _adminService.GetAllDrugsAsync();
            return Ok(new ApiResponse<List<DrugResponseDTO>>
            {
                Success = true,
                Data = data,
                Message = "Drugs retrieved successfully."
            });
        }

        [HttpPost("add-drug")]
        public async Task<IActionResult> AddDrug([FromBody] DrugDTO drugDto)
        {
            var result = await _adminService.AddDrugAsync(drugDto);
            if (result == "Supplier not found!")
            {
                return NotFound(new { message = result });
            }
            else if (result == "Drug added successfully!")
            {
                return Created("", new { message = result });
            }

            return BadRequest(new { message = "Unexpected error occurred!" });

        }

        [HttpGet("suppliers")]
        public async Task<IActionResult> GetAllSuppliers()
        {
            var data = await _adminService.GetAllSuppliersAsync();
            return Ok(new ApiResponse<List<SupplierResponseDTO>>
            {
                Success = true,
                Data = data,
                Message = "Suppliers retrieved successfully."
            });
        }

        [HttpPut("edit-drug/{id}")]
        public async Task<IActionResult> EditDrug(int id, [FromBody] DrugDTO dto)
        {
            var message = await _adminService.EditDrugAsync(id, dto);
            return Ok(new ApiResponse<string>
            {
                Success = true,
                Data = message,
                Message = "Drug updated successfully."
            });
        }

        [HttpDelete("delete-drug/{id}")]
        public async Task<IActionResult> DeleteDrug(int id)
        {
            var message = await _adminService.DeleteDrugAsync(id);
            return Ok(new ApiResponse<string>
            {
                Success = true,
                Data = message,
                Message = "Drug deleted successfully."
            });
        }

        [HttpPut("edit-supplier/{id}")]
        public async Task<IActionResult> EditSupplier(int id, [FromBody] SupplierDTO dto)
        {
            var message = await _adminService.EditSupplierAsync(id, dto);
            return Ok(new ApiResponse<string>
            {
                Success = true,
                Data = message,
                Message = "Supplier updated successfully."
            });
        }

        [HttpDelete("delete-supplier/{id}")]
        public async Task<IActionResult> DeleteSupplier(int id)
        {
            var message = await _adminService.DeleteSupplierAsync(id);
            return Ok(new ApiResponse<string>
            {
                Success = true,
                Data = message,
                Message = "Supplier deleted successfully."
            });
        }

        [HttpGet("orders/new")]
        public async Task<IActionResult> GetNewOrders()
        {
            var data = await _adminService.GetNewOrdersAsync();
            return Ok(new ApiResponse<List<OrderDTO>>
            {
                Success = true,
                Data = data,
                Message = "New orders retrieved."
            });
        }



        [HttpPost("orders/verify/{orderId}")]
        public async Task<IActionResult> VerifyOrder(int orderId)
        {
            var message = await _adminService.VerifyOrderAsync(orderId);
            return Ok(new ApiResponse<string>
            {
                Success = !message.Contains("Insufficient stock"),
                Data = message,
                Message = message.Contains("Insufficient stock") ? $"{message}" : "Order verified."
            });
        }

        [HttpPost("orders/classify/{orderItemId}")]
        public async Task<IActionResult> ClassifyOrderItem(int orderItemId)
        {
            try
            {
                var result = await _adminService.ClassifyOrderItemAsync(orderItemId);

                if (result == "Order Item not found.")
                    return NotFound(new{message=result}); // Returns 404 if item doesn't exist

                return Ok(new{message=result}); // Returns 200 if classification succeeds
            }
            catch (ArgumentException ex)
            {
                return BadRequest($"Invalid request: {ex.Message}"); // Returns 400 for bad input
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}"); // Returns 500 for unexpected errors
            }
        }

        [HttpPost("orders/pick/{orderId}")]
        public async Task<IActionResult> PickOrder(int orderId)
        {
            var message = await _adminService.PickOrderAsync(orderId);
            return Ok(new ApiResponse<string>
            {
                Success = true,
                Data = message,
                Message = "Order picked."
            });
        }

        [HttpGet("sales-report")]
        public async Task<IActionResult> GetSalesReport([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var data = await _adminService.GetSalesReportAsync(startDate, endDate);
            return Ok(new ApiResponse<SalesReportResponseDTO>
            {
                Success = true,
                Data = data,
                Message = "Sales report retrieved."
            });
        }

        [HttpGet("supplier-order-status")]
        public async Task<IActionResult> GetSupplierOrderStatuses()
        {
            var data = await _adminService.GetSupplierOrderItemStatusesAsync();
            return Ok(new ApiResponse<List<SupplierOrderStatusDTO>>
            {
                Success = true,
                Data = data,
                Message = "Supplier order statuses retrieved."
            });
        }

        [HttpGet("orders/verified")]
        public async Task<IActionResult> GetVerifiedOrders()
        {
            var data = await _adminService.GetVerifiedOrdersAsync();
            return Ok(new ApiResponse<List<OrderDTO>>
            {
                Success = true,
                Data = data,
                Message = "Verified orders retrieved."
            });
        }

        [HttpGet("expired-drugs")]
        public async Task<IActionResult> GetExpiredDrugs()
        {
            try
            {
                var expiredDrugs = await _adminService.GetExpiredDrugsAsync();

                if (expiredDrugs == null || !expiredDrugs.Any())
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "No expired drugs found.",
                        data = new List<object>()
                    });
                }

                return Ok(new
                {
                    success = true,
                    message = "Expired drugs retrieved successfully.",
                    data = expiredDrugs
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = $"An error occurred while fetching expired drugs: {ex.Message}",
                    data = new List<object>()
                });
            }
        }



    }
}
