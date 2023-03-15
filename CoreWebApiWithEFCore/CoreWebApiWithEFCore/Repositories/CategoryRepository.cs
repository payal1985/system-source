using CoreWebApiWithEFCore.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApiWithEFCore.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private CoreWebApiCrudContext db;

        public CategoryRepository(CoreWebApiCrudContext _db)
        {
            this.db = _db;
        }

        public async Task<int> AddCategoryAsync(Category category)
        {
            if (db != null)
            {
                await db.Categories.AddAsync(category);
                await db.SaveChangesAsync();
                return category.Id;
            }
            return 0;
        }

        public async Task<int> DeleteCategory(int? categoryId)
        {
            int result = 0;

            if (db != null)
            {
                //Find the category for specific category id
                var category = await db.Categories.FirstOrDefaultAsync(x => x.Id == categoryId);
                if (category != null)
                {
                    //Delete that category
                    db.Categories.Remove(category);
                    //Commit the transaction
                    result = await db.SaveChangesAsync();
                }
                return result;
            }
            return result;
        }

        public async Task<List<Category>> GetAllCategories()
        {
            if (db != null)
            {
                return await db.Categories.ToListAsync();
            }
            return null;
        }

        public async Task<Category> GetCategoryById(int? categoryId)
        {
            if (db != null)
            {
                return await db.Categories.FirstOrDefaultAsync(x => x.Id == categoryId);
            }
            return null;
        }

        public async Task UpdateCategory(Category category)
        {
            if (db != null)
            {
                //Delete that category
                db.Categories.Update(category);
                //Commit the transaction
                await db.SaveChangesAsync();
            }
        }
    }
}
