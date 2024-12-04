using Farmers.DataModel;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FTG.Repository.Repository
{
    public class CategoryRepo : GenericRepo<ProductCategory>, ICategoryRepo
    {
        public CategoryRepo(AppDbContext context) : base(context) { }

        public async Task<List<ProductCategory>> GetAllForDropdownAsync()
        {
            return await _context.ProductCategories.ToListAsync();
        }
    }
}
