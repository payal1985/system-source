using InventoryApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryApi.Repository.Interfaces
{
    public interface IDonationInventoryRepository
    {
        Task<List<DonationInventoryModel>> GetInventory(int categoryid);
        Task<List<ItemTypesModel>> GetInventoryCategory();
    }
}
