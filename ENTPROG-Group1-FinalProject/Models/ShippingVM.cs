using System.ComponentModel.DataAnnotations;

namespace Farmers.App.Models
{
    public class ShippingVM
    {
        [Required]
        public int ShippingId { get; set; }

        [Required]
        public int OrderId { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "Delivery service name must not exceed 20 characters.")]
        public string DeliveryService { get; set; }

        [Required]
        [Display(Name = "Shipped Date")]
        public DateTime ShippedDate { get; set; }

        [Required]
        [Display(Name = "Shipping Status")]
        public string Status { get; set; }
    }
}
