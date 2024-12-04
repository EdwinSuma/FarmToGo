using System.Collections.Generic;
using System.Threading.Tasks;
using Farmers.DataModel;

namespace FTG.Repository.Repository
{
    public interface IOrderRepo
    {
        Task<Order> GetOrderByIdAsync(int orderId);
        Task<List<Order>> GetOrdersByCustomerIdAsync(int customerId);
        Task AddOrderAsync(Order order);
        Task UpdateOrderAsync(Order order);
        Task DeleteOrderAsync(int orderId);
        Task<OrderResult> CreateOrderAsync(ApplicationUser user, Product product, int quantity);
    }
}
