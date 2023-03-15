using SupportUI.DBContext;
using SupportUI.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SupportUI.Repository
{
    public class SubCategoryRepository : ISubCategoryRepository
    {
        ProductContext _dbContext;
        public SubCategoryRepository(ProductContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<SubCategory> GetSubCategory()
        {
            try
            {
                return _dbContext.SubCategories.ToList();

            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
