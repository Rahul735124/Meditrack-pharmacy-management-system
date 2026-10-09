using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PharmacyBackend.DTOs;
using PharmacyBackend.Interface;

namespace PharmacyBackend.Service
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;

        public AuthService(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        public async Task<string> RegisterUserAsync(RegisterDTO dto)
        {
            try
            {
                return await _authRepository.RegisterAsync(dto);
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }


        public async Task<(string Token, string Role)> LoginUserAsync(LoginDTO dto)
        {
            try
            {
                return await _authRepository.LoginAsync(dto);
            }
            catch
            {
                return (null, null);
            }
        }
        public async Task<bool> ForgotPasswordAsync(string email)
        {
            return await _authRepository.SendResetPasswordLinkAsync(email);
        }


        public async Task<bool> ResetPasswordAsync(string email,ResetPasswordDTO dto)
        {
            return await _authRepository.ResetPasswordAsync( email, dto);
        }


    }
}