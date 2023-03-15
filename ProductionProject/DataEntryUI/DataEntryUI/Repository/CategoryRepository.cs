using DataEntryUI.DBContext;
using DataEntryUI.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataEntryUI.Repository
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
