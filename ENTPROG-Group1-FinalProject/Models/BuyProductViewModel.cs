using Microsoft.AspNetCore.Mvc;

namespace Farmers.App.Models
{
    public class BuyProductViewModel
    {
        public int ProductId { get; set; } // The ID of the product being purchased
        public string Name { get; set; } // The name of the product
        public string Description { get; set; }
        public decimal Price { get; set; } // The price of the product
        public int Quantity { get; set; } // The quantity the customer wants to buy
        public int Stock { get; set; }
    }
}

