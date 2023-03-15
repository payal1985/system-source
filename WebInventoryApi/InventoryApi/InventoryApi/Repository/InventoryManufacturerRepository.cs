using InventoryApi.DBContext;
using InventoryApi.Models;
using InventoryApi.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryApi.Repository
{
    public class InventoryManufacturerRepository : IInventoryManufacturerRepository
    {
        InventoryContext _dbContext;

        public InventoryManufacturerRepository(InventoryContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<ManufacturersModel>> GetManufacturers(string search)
        {
            List<ManufacturersModel> manufacturersModelsList = new List<ManufacturersModel>();

            List<string> pmTypes = new List<string> { "P", "F", "X" };
           
            if(!string.IsNullOrEmpty(search))
            {
                manufacturersModelsList = await _dbContext.Manufacturers
                                        .Where(x => (x.ManufacturerName.Contains(search) || x.VendorNum.Contains(search)) 
                                                    && pmTypes.Contains(x.PMType)
                                              )
                                        .Select(s=> new ManufacturersModel()
                                        {
                                            ManufacturerId = s.ManufacturerID,
                                            ManufacturerName = s.DisplayName
                                        })
                                        .ToListAsync();
                //manufacturersModelsList = await manufacturersList.Where(x => pmTypes.Contains(x.PMType)).Select(x => new ManufacturersModel()
                //{
                //    ManufacturerId = x.ManufacturerID,
                //    ManufacturerName = x.ManufacturerName
                //}).ToListAsync();
            }
            else
            {
                manufacturersModelsList = await _dbContext.Manufacturers.Where(x => pmTypes.Contains(x.PMType)).Select(x=> new ManufacturersModel()
                                            {
                                                ManufacturerId = x.ManufacturerID,
                                                ManufacturerName = x.DisplayName
                                            }).ToListAsync();

            }

            return manufacturersModelsList.OrderBy(x => x.ManufacturerName).ToList();
      
        }
    }
}
