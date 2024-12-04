using Farmers.DataModel;
using FTG.Repository.Repository;
using Microsoft.EntityFrameworkCore;

namespace Farmers.App.Models.Repositories
{
    public class OrderRepo : ProductRepo<Order>, IOrderRepo
    {
        private readonly AppDbContext dbc;

        public OrderRepo(AppDbContext dbc) : base(dbc)
        {
            this.dbc = dbc;
        }

        public async Task CreateAsync(Order entity)
        {
            entity.OrderStatus = "Pending";

            var transaction = await dbc.Database.BeginTransactionAsync();
            await dbc.Database.ExecuteSqlRawAsync(
                "INSERT INTO PurchaseOrderHeadersINV ");

            int purchaseId = await dbc.Set<Order>().MaxAsync(p => p.OrderId);

            foreach (OrderDetail item in entity.OrderDetails)
            {
                item.OrderId = purchaseId;

                await dbc.Database.ExecuteSqlRawAsync(
                    "INSERT INTO PurchaseOrderHeadersINV ");
            }

            await transaction.CommitAsync();
        }

    }
}
