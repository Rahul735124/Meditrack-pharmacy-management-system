using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PharmacyBackend.DTOs;

namespace PharmacyBackend.Interface
{
    public interface IAuthRepository
    {
        Task<string> RegisterAsync(RegisterDTO dto);

        Task<(string Token, string Role)> LoginAsync(LoginDTO dto);
        Task<bool> SendResetPasswordLinkAsync(string email);

        Task<bool> ResetPasswordAsync(string email, ResetPasswordDTO dto);


    }


}