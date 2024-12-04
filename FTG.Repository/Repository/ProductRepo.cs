using Farmers.DataModel;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FTG.Repository.Repository
{
    public class ProductRepo : GenericRepo<Product>, IProductRepo
    {
        public ProductRepo(AppDbContext context) : base(context) { }

        public async Task<List<Product>> GetByFarmerIdAsync(int farmerId)
        {
            return await _context.Products.Where(p => p.FarmerId == farmerId).ToListAsync();
        }
    }
}
