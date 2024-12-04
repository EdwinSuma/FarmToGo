using FTG.Repository.Repository;
using Farmers.DataModel;

namespace Farmers.App.Models.Repositories
{
    public interface IOrderRepo : IProductRepo<Order>
    {
        Task CreateAsync(Order entity);
    }
}
