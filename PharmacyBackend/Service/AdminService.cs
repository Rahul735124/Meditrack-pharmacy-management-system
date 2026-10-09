using PharmacyBackend.DTOs;
using PharmacyBackend.Interface;
using PharmacyBackend.Models;



namespace PharmacyBackend.Service
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _adminRepository;
        private readonly IEmailService _emailService;

        public AdminService(IAdminRepository adminRepository, IEmailService emailService)
        {
            _adminRepository = adminRepository;
            _emailService = emailService;
        }

        public async Task<ApiResponse<string>> AddSupplierAsync(SupplierDTO supplierDto)
        {
            try
            {
                var result = await _adminRepository.AddSupplierAsync(supplierDto);

                // Check failure cases from repository response
                if (result.Contains("Supplier email already exists"))
                    return new ApiResponse<string> { Success = false, Message = result };

                if (result.Contains("Failed to create"))
                    return new ApiResponse<string> { Success = false, Message = result };

                if (result.StartsWith("Error:"))
                    return new ApiResponse<string> { Success = false, Message = "An unexpected error occurred." };

                // Secure Email: Removing plaintext password
                var resetPasswordLink = $"http://localhost:4200/auth/forgot-password";
                var emailBody = $@"
            <h3>Welcome to the Pharmacy System</h3>
            <p>Hello {supplierDto.Name},</p>
            <p>Your supplier account has been created successfully.</p>
            <p>Please reset your password before logging in:</p>
            <p><b><a href='{resetPasswordLink}'>Reset Password</a></b></p>
            <p>Once reset, log in here:</p>
            <p><b><a href='http://localhost:4200/auth/login'>Login</a></b></p>";

                await _emailService.SendEmailAsync(supplierDto.Email, "Supplier Account Created", emailBody);

                return new ApiResponse<string> { Success = true, Message = "Supplier added successfully. Please reset password via email." };
            }
            catch (Exception ex)
            {
                // _logger.LogError($"Error adding supplier: {ex.Message}");
                return new ApiResponse<string> { Success = false, Message = "An unexpected error occurred while adding the supplier." };
            }


        }



        public async Task<List<DrugResponseDTO>> GetAllDrugsAsync()
        {
            try
            {
                return await _adminRepository.GetAllDrugsAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in AdminService.GetAllDrugsAsync: {ex.Message}");
            }
        }

        public async Task<string> AddDrugAsync(DrugDTO drugDto)
        {
            try
            {
                return await _adminRepository.AddDrugAsync(drugDto);
            }
            catch (Exception ex)
            {
                return $"Error in AdminService.AddDrugAsync: {ex.Message}";
            }
        }

        public async Task<List<OrderDTO>> GetNewOrdersAsync()
        {
            try
            {
                return await _adminRepository.GetNewOrdersAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in AdminService.GetNewOrdersAsync: {ex.Message}");
            }
        }

        public async Task<string> ClassifyOrderItemAsync(int orderItemId)
        {
            try
            {
                return await _adminRepository.ClassifyOrderItemAsync(orderItemId);
            }
            catch (Exception ex)
            {
                return $"Error in AdminService.ClassifyOrderItemAsync: {ex.Message}";
            }
        }

        public async Task<string> VerifyOrderAsync(int orderId)
        {
            try
            {
                return await _adminRepository.VerifyOrderAsync(orderId);
            }
            catch (Exception ex)
            {
                return $"Error in AdminService.VerifyOrderAsync: {ex.Message}";
            }
        }

        public async Task<string> PickOrderAsync(int orderId)
        {
            try
            {
                return await _adminRepository.PickOrderAsync(orderId);
            }
            catch (Exception ex)
            {
                return $"Error in AdminService.PickOrderAsync: {ex.Message}";
            }
        }

        public async Task<List<SupplierResponseDTO>> GetAllSuppliersAsync()
        {
            return await _adminRepository.GetAllSuppliersAsync();
        }

        public async Task<string> EditDrugAsync(int drugId, DrugDTO dto)
        {
            return await _adminRepository.EditDrugAsync(drugId, dto);
        }

        public async Task<string> DeleteDrugAsync(int drugId)
        {
            return await _adminRepository.DeleteDrugAsync(drugId);
        }

        public async Task<string> EditSupplierAsync(int supplierId, SupplierDTO dto)
        {
            return await _adminRepository.EditSupplierAsync(supplierId, dto);
        }

        public async Task<string> DeleteSupplierAsync(int supplierId)
        {
            return await _adminRepository.DeleteSupplierAsync(supplierId);
        }

        public async Task<SalesReportResponseDTO> GetSalesReportAsync(DateTime startDate, DateTime endDate)
        {
            return await _adminRepository.GetSalesReportAsync(startDate, endDate);
        }

        public async Task<List<SupplierOrderStatusDTO>> GetSupplierOrderItemStatusesAsync()
        {
            return await _adminRepository.GetSupplierOrderItemStatusesAsync();
        }

        public async Task<List<OrderDTO>> GetVerifiedOrdersAsync()
        {
            return await _adminRepository.GetVerifiedOrdersAsync();
        }

        public async Task<IEnumerable<DrugResponseDTO>> GetExpiredDrugsAsync()
        {
            return await _adminRepository.GetExpiredDrugsAsync();
        }




    }
}