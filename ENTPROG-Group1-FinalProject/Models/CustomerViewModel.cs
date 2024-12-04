using Farmers.DataModel;
using Microsoft.AspNetCore.Mvc;

namespace Farmers.App.Models
{
    public class CustomerViewModel
    {
        // For the Index View (List of Products)
        public IEnumerable<Product> Products { get; set; }

        // For the BuyProduct View (Product details and Quantity for purchase)
        public BuyProductViewModel BuyProductViewModel { get; set; }

        // For the OrderConfirmation View (Order Details)
        public Order Order { get; set; }
    }
}