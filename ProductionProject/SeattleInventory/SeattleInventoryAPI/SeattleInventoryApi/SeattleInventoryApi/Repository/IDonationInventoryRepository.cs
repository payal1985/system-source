using SeattleInventoryApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeattleInventoryApi.Repository
{
    public interface IDonationInventoryRepository
    {
        List<DonationInventoryModel> GetInventory(string category);
        List<string> GetInventoryCategory();
    }
}
