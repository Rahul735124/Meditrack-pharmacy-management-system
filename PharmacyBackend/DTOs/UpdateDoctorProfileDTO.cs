using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmacyBackend.DTOs
{
    public class UpdateDoctorProfileDTO
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Contact { get; set; }
    }
}