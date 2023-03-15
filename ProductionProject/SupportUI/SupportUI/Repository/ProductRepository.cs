using SupportUI.DBContext;
using SupportUI.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SupportUI.Repository
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
                var entity = _dbContext.Products.Where(item => item.Item_Number == product.Item_Number).LastOrDefault();

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


                //// Validate entity is not null
                //if (entity != null)
                //{
                //    entity.Category_ID = product.Category_ID;
                //    entity.Subcategory_ID = product.Subcategory_ID;
                //    entity.Lead_Time_ID = product.Lead_Time_ID;
                //    entity.Manufacturer = product.Manufacturer;
                //    entity.Mfg_Rep = product.Mfg_Rep;
                //    entity.Description = product.Description;
                //    entity.ManFac = product.ManFac;
                //    entity.Cat = product.Cat;
                //    entity.Series = product.Series;
                //    entity.Base_Attribute = product.Base_Attribute;
                //    entity.Height_attr = product.Height_attr;
                //    entity.Misc = product.Misc;
                //    entity.Seat_Actions = product.Seat_Actions;
                //    entity.Fabric_Detail = product.Fabric_Detail;
                //    entity.Modular = product.Modular;
                //    entity.Shape = product.Shape;
                //    entity.Seats = product.Seats;
                //    entity.COM_Price = product.COM_Price;
                //    entity.Grade_3_or_C_Price = product.Grade_3_or_C_Price;
                //    entity.Height = product.Height;
                //    entity.Depth = product.Depth;
                //    entity.Width = product.Width;
                //    entity.Diameter = product.Diameter;
                //    entity.Designer = product.Designer;
                //    entity.Yardage = product.Yardage;
                //    entity.Leather_SqFt = product.Leather_SqFt;
                //    entity.Leather_SqM = product.Leather_SqM;
                //    entity.CF = product.CF;
                //    entity.Environmental_1 = product.Environmental_1;
                //    entity.Environmental_2 = product.Environmental_2;
                //    entity.Environmental_3 = product.Environmental_3;
                //    entity.Environmental_4 = product.Environmental_4;
                //    entity.Seat_Height = product.Seat_Height;
                //    entity.Arm_Height = product.Arm_Height;
                //    entity.active_fl = product.active_fl;
                //    entity.Load_DateTime = product.Load_DateTime;



                //    _dbContext.Update(entity);
                //}
                //else
                //{
                //    await _dbContext.AddAsync(product);
                //}
            }
            catch (Exception ex)
            {
                resultStr = $"Exception Occured while processing data for the part number {product.Item_Number} \n Error Message: {ex.Message} \n {ex.StackTrace}";
                //throw;
            }
            return resultStr;

        }

    }
}
