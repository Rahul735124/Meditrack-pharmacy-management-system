using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PharmacyBackend.DTOs;

namespace PharmacyBackend.Interface
{
    public interface IAuthService
    {
        Task<string> RegisterUserAsync(RegisterDTO dto);
        Task<(string Token, string Role)> LoginUserAsync(LoginDTO dto);

        Task<bool> ForgotPasswordAsync(string email);
        Task<bool> ResetPasswordAsync(string email, ResetPasswordDTO dto);



    }
}