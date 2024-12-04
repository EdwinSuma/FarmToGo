using Farmers.DataModel;
using FTG.Repository.Repository;

namespace Farmers.App.Models.Repositories
{
    public class ProductRepo : ProductRepo<Product>, IProductRepo
    {
        public ProductRepo(AppDbContext dbc) : base(dbc)
        {
        }
    }
}
