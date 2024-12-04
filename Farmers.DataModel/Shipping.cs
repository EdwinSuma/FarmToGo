using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Farmers.DataModel;

namespace Farmers.DataModel
{
    public class Shipping
    {
        [Key]
        public int ShippingId { get; set; }

        [Required]
        public int OrderId { get; set; }

        [ForeignKey("OrderId")]
        public Order Order { get; set; }

        [Required]
        public int CourierId { get; set; }

        [ForeignKey("CourierId")]
        public Courier Courier { get; set; }

        [Required]
        [StringLength(255)]
        public string ShippingAddress { get; set; }

        [Required]
        public DateTime ShippingDate { get; set; } = DateTime.Now;

        [Required]
        public string ShippingStatus { get; set; } = "Pending"; // Pending, In Transit, Delivered, Cancelled
    }
}
