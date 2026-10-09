using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PharmacyBackend.Models
{
    public class OrderItem
{
    [Key]
    public int OrderItemId { get; set; }

    [Required]
    public int OrderId { get; set; }

    [ForeignKey("OrderId")]
    public Order Order { get; set; }

    [Required]
    public int DrugId { get; set; }

    [ForeignKey("DrugId")]
    public Drug Drug { get; set; }

    [Required]
    public int Quantity { get; set; }

    public bool IsRequestedFromSupplier { get; set; } = false;

    public bool IsSupplierConfirmed { get; set; } = false;

    public bool IsSentBySupplier { get; set; } = false;
}

}