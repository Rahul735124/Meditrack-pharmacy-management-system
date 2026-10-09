using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PharmacyBackend.Models
{
    public class Drug
    {
        [Key]
        public int DrugId { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public int QuantityAvailable { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public DateTime ExpiryDate { get; set; }

        [ForeignKey("Supplier")]
        public int SupplierId { get; set; }

        public Supplier Supplier { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; }
    }

}