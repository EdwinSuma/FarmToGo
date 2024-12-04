using Farmers.DataModel;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FTG.Repository.Repository
{
    public class OrderRepo : IOrderRepo
    {
        private readonly AppDbContext _context;

        public OrderRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Order> GetOrderByIdAsync(int orderId)
        {
            return await _context.Orders.Include(o => o.OrderDetails)
                                              .ThenInclude(od => od.Product)
                                              .FirstOrDefaultAsync(o => o.OrderId == orderId);
        }

        public async Task<List<Order>> GetOrdersByCustomerIdAsync(int customerId)
        {

            return await _context.Orders.Include(o => o.OrderDetails)
                                        .ThenInclude(od => od.Product)
                                        .Where(o => o.CustomerId == customerId)
                                        .ToListAsync();
        }


        public async Task AddOrderAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateOrderAsync(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteOrderAsync(int orderId)
        {
            var order = await GetOrderByIdAsync(orderId);
            if (order != null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<OrderResult> CreateOrderAsync(ApplicationUser user, Product product, int quantity)
        {
            // Try to convert the string user.Id to an int
            if (int.TryParse(user.Id, out int customerId))
            {
                var order = new Order
                {
                    CustomerId = customerId,  // Now using the converted int value
                    OrderDate = DateTime.UtcNow,
                    OrderDetails = new List<OrderDetail>
            {
                new OrderDetail
                {
                    ProductId = product.ProductId,
                    Quantity = quantity,
                    Price = product.Price
                }
            }
                };

                // You might want to reduce stock here if necessary
                if (product.Stock < quantity)
                {
                    return new OrderResult { IsSuccess = false, ErrorMessage = "Not enough stock" };
                }

                product.Stock -= quantity;  // Update product stock

                await _context.Orders.AddAsync(order);
                await _context.SaveChangesAsync();

                return new OrderResult { IsSuccess = true, OrderId = order.OrderId };
            }
            else
            {
                return new OrderResult { IsSuccess = false, ErrorMessage = "Invalid customer ID" };
            }
        }

    }
}
