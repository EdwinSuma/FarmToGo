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
            return await _context.Products
                .Where(p => p.FarmerId == farmerId)
                .ToListAsync();
        }

        // Implement the method to get a product by its ID
        public async Task<Product> GetProductByIdAsync(int productId)
        {
            return await _context.Products
                .Where(p => p.ProductId == productId)
                .FirstOrDefaultAsync();
        }

        // Implement the method to get available products
        public async Task<List<Product>> GetAvailableProductsAsync()
        {
            return await _context.Products
                .Where(p => p.Stock > 0)
                .ToListAsync();
        }
    }
}
