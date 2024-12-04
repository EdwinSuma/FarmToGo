using Farmers.DataModel;
using System.ComponentModel.DataAnnotations;

namespace Farmers.App.Models
{
    public class OrderDetailsVM
    {
        public int OrderId { get; set; }
        public int BuyerId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
        public string TotalPrice { get; set; }

        [Required]
        [Display(Name = "Order Status")]
        public string Status { get; set; }

        //Additional fields for displaying Buyer Details
        public string CustomerName { get; set; }

        //Product details
        public string ProductName { get; set; }
        public decimal ProductPrice { get; set; }
        public Product Product { get; set; }

    }
}
