using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PharmacyBackend.DTOs;
using PharmacyBackend.Interface;
using PharmacyBackend.Models;
using PharmacyBackend.Service;

namespace PharmacyBackend.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO dto)
        {
            try
            {
                var result = await _authService.RegisterUserAsync(dto);
                if (result.ToLower().Contains("error") || result.ToLower().Contains("exists"))
                    return BadRequest(new ApiResponse<string> { Success = false, Data = null, Message = result });

                return Ok(new ApiResponse<string> { Success = true, Data = result, Message = "Registration successful." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<string> { Success = false, Data = null, Message = $"Internal error: {ex.Message}" });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO dto)
        {
            try
            {
                var (token, role) = await _authService.LoginUserAsync(dto);
                if (token == null)
                    return Unauthorized(new ApiResponse<string> { Success = false, Data = null, Message = "Invalid credentials." });

                return Ok(new ApiResponse<object> { Success = true, Data = new { token, role, dto.Email }, Message = "Login successful." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<string> { Success = false, Data = null, Message = $"Internal error: {ex.Message}" });
            }
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDTO dto)
        {
            var success = await _authService.ForgotPasswordAsync(dto.Email);
            return success ? Ok(new { message = "Password reset link sent." }) : NotFound(new { message = "User not found." });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromQuery] string email, [FromBody] ResetPasswordDTO dto)
        {
            if (dto.NewPassword != dto.ConfirmPassword)
                return BadRequest(new { message = "Passwords do not match!" });
                

            var success = await _authService.ResetPasswordAsync(email, dto);
            return success ? Ok(new { message = "Password reset successful." }) : BadRequest(new { message = "Invalid token or user." });
        }

    }
}