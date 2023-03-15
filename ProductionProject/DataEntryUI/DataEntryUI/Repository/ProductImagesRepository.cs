using DataEntryUI.DBContext;
using DataEntryUI.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataEntryUI.Repository
{
    public class ProductImagesRepository : IProductImagesRepository
    {
        ProductContext _dbContext;
        public ProductImagesRepository(ProductContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<string> InsertProductImages(ProductImages productImages)
        {
            string resultStr = "";
            try
            {
                var entity = _dbContext.ProductImages.Where(item => item.Microsoft_product_ID == productImages.Microsoft_product_ID).SingleOrDefault();

                if (entity != null)
                {
                    resultStr = $"File  {productImages.Filename} already exists";
                }
                else
                {
                    await _dbContext.AddAsync(productImages);
                    await _dbContext.SaveChangesAsync();
                    resultStr = $"Data are inserted successfully for the image  {productImages.Filename} ";
                }

            }
            catch (Exception ex)
            {
                throw new Exception($"Exception Occured while processing data for the image {productImages.Filename} \n Error Message: {ex.Message} \n {ex.StackTrace}");
                //throw;
            }
            return resultStr;

        }

    }
}
