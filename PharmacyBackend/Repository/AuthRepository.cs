using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using PharmacyBackend.DTOs;
using PharmacyBackend.Helpers;
using PharmacyBackend.Interface;
using PharmacyBackend.Models;
using PharmacyBackend.Service;

namespace PharmacyBackend.Repository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IConfiguration _config;
        private readonly IEmailService _emailService;

        public AuthRepository(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IConfiguration config, IEmailService emailService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _config = config;
            _emailService = emailService;

        }

        public async Task<string> RegisterAsync(RegisterDTO dto)
        {
            try
            {
                var role = "Doctor";

                if (!await _roleManager.RoleExistsAsync(role))
                    return "Role Doctor does not exist.";

                var userExists = await _userManager.FindByEmailAsync(dto.Email);
                if (userExists != null)
                    return "User already exists.";

                var user = new ApplicationUser
                {
                    FullName = dto.Name,
                    Contact = dto.Contact,
                    Email = dto.Email,
                    UserName = dto.Email,
                    NormalizedEmail = dto.Email.ToUpper(),
                    NormalizedUserName = dto.Email.ToUpper()
                };

                var result = await _userManager.CreateAsync(user, dto.Password);
                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    return $"User creation failed: {errors}";
                }

                await _userManager.AddToRoleAsync(user, role);

                return "User registered successfully.";
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }



        public async Task<(string Token, string Role)> LoginAsync(LoginDTO dto)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(dto.Email);
                if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password))
                    return (null, null);

                var roles = await _userManager.GetRolesAsync(user);
                var token = JwtHelper.GenerateJwtToken(user, roles, _config);
                return (token, roles.FirstOrDefault());
            }
            catch
            {
                return (null, null);
            }
        }
        public async Task<bool> SendResetPasswordLinkAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return false;

            var token = JwtHelper.GeneratePasswordResetToken(user, _config);

            Console.WriteLine(token);
            var resetLink = $"http://localhost:4200/auth/reset-password?email={email}&token={token}";

            await _emailService.SendEmailAsync(email, "Password Reset Request", $"Click to reset: {resetLink}");
            return true;
        }

        public async Task<bool> ResetPasswordAsync(string email, ResetPasswordDTO dto)
        {
            var principal = JwtHelper.ValidateResetToken(dto.Token, _config);
            if (principal == null) return false; // Token is invalid or expired

            var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return false;

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            // Manually reset password since Identity’s `ResetPasswordAsync()` doesn’t accept JWT
            user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, dto.NewPassword);
            await _userManager.UpdateAsync(user);

            return true;
        }



    }
}