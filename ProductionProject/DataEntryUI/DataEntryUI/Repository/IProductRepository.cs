using DataEntryUI.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataEntryUI.Repository
{
    public interface IProductRepository
    {
       Task<string> InsertProduct(Product product);
        int GetProductId(string ItemCode);
        string GetProductManufacturer(string ItemCode);
    }
}
