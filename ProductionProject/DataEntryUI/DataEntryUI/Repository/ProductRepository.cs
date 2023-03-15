using DataEntryUI.DBContext;
using DataEntryUI.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataEntryUI.Repository
{
    public class ProductRepository : IProductRepository
    {
        ProductContext _dbContext;
        public ProductRepository(ProductContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<string> InsertProduct(Product product)
        {
            string resultStr = "";
            try
            {
                var entity = _dbContext.Products.Where(item => item.Item_Number == product.Item_Number).SingleOrDefault();

                if(entity != null)
                {
                    resultStr = $"Part Number {product.Item_Number} already exists";
                }
                else
                {
                    await _dbContext.AddAsync(product);
                    await _dbContext.SaveChangesAsync();
                    resultStr = $"Data are inserted successfully for the part number {product.Item_Number} ";
                }

            }
            catch (Exception ex)
            {
                throw new Exception($"Exception Occured while processing data for the part number {product.Item_Number} \n Error Message: {ex.Message} \n {ex.StackTrace}");
                //throw;
            }
            return resultStr;

        }

        public int GetProductId(string ItemCode)
        {
            try
            {
                //var entity = _dbContext.Products.Where(item => item.Item_Number == ItemCode).LastOrDefault();
                //return entity.Microsoft_product_ID;
                return _dbContext.Products.Where(p => p.Item_Number == ItemCode).SingleOrDefault().Microsoft_product_ID;


            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public string GetProductManufacturer(string ItemCode)
        {
            try
            {
                return _dbContext.Products.Where(p => p.Item_Number == ItemCode).SingleOrDefault().Manufacturer;

            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
