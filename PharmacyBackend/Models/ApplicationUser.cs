using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace PharmacyBackend.Models
{
    public class ApplicationUser : IdentityUser
{
    [Required]
    public string FullName { get; set; }

    [Required]
    public string Contact { get; set; }

    public ICollection<Order> Orders { get; set; } // For doctors
}

}