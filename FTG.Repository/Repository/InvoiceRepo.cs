using Farmers.DataModel;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace FTG.Repository.Repository
{
    public class InvoiceRepo : IInvoiceRepo
    {
        private readonly AppDbContext _context;

        public InvoiceRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Invoice> CreateInvoiceAsync(Order order)
        {
            var invoice = new Invoice
            {
                OrderId = order.OrderId,
                TotalAmount = order.TotalAmount,
                InvoiceDate = DateTime.Now
            };

            await _context.Invoices.AddAsync(invoice);
            await _context.SaveChangesAsync();
            return invoice;
        }

        public async Task<Invoice> GetInvoiceByOrderIdAsync(int orderId)
        {
            return await _context.Invoices.FirstOrDefaultAsync(i => i.OrderId == orderId);
        }

        public async Task UpdateInvoiceAsync(Invoice invoice)
        {
            _context.Invoices.Update(invoice);
            await _context.SaveChangesAsync();
        }
    }
}
