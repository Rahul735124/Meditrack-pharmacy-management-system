using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PharmacyBackend.Models
{
    public class Order
{
    [Key]
    public int OrderId { get; set; }

    [Required]
    public string UserId { get; set; }

    [ForeignKey("UserId")]
    public ApplicationUser User { get; set; }

    public DateTime OrderDate { get; set; } = DateTime.UtcNow;

    public bool IsVerified { get; set; } = false;

    public bool IsPickedUp { get; set; } = false;

    public ICollection<OrderItem> OrderItems { get; set; }
}

}