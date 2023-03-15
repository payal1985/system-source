using SeattleInventoryApi.DBModels;
using SeattleInventoryApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeattleInventoryApi.Repository
{
    public interface IInventoryRepository
    {
        Task<List<InventoryModel>> GetInventory(string category, int client_id, string bulding, string floor,string room,string cond);
        List<InventoryModel> GetInventory1(string category, string bulding, string floor);
        Task<List<string>> GetInventoryCategory(int client_id);

        Task<List<InventoryModel>> SearchInventory(string strSearch, int client_id, string bulding, string floor, string room, string cond);

    }
}
