using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PharmacyBackend.Models
{
    public class Supplier
    {
        [Key]
    public int SupplierId { get; set; }

    [Required]
    public string UserId { get; set; }

    [ForeignKey("UserId")]
    public ApplicationUser User { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public string Contact { get; set; }

    [Required]
    public string Email { get; set; }

    public Drug Drug { get; set; }
    }
}