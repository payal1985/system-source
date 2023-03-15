using CoreWebApiWithEFCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApiWithEFCore.Repositories
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetAllCategories();

        Task<Category> GetCategoryById(int? categoryId);

        Task<int> AddCategoryAsync(Category category);

        Task<int> DeleteCategory(int? categoryId);

        Task UpdateCategory(Category category);
    }
}
