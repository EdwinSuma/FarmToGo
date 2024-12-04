using System.Threading.Tasks;
using Farmers.DataModel;

namespace FTG.Repository.Repository
{
    public interface IInvoiceRepo
    {
        Task<Invoice> CreateInvoiceAsync(Order order);
        Task<Invoice> GetInvoiceByOrderIdAsync(int orderId);
        Task UpdateInvoiceAsync(Invoice invoice);
    }
}
