using SupportUI.DBContext;
using SupportUI.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SupportUI.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        ProductContext _dbContext;
        public CategoryRepository(ProductContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Category> GetCategory()
        {
            try
            {
                return _dbContext.Categories.ToList();

            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
