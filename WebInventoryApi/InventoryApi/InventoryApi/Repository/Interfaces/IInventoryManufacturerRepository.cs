using InventoryApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryApi.Repository.Interfaces
{
    public interface IInventoryManufacturerRepository
    {
        Task<List<ManufacturersModel>> GetManufacturers(string search);
    }
}
