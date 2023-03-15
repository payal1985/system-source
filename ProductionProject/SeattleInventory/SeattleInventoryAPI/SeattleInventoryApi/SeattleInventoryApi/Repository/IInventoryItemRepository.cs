using SeattleInventoryApi.DBModels;
using SeattleInventoryApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeattleInventoryApi.Repository
{
    public interface IInventoryItemRepository
    {
        List<string> GetBuilding(int client_id);
        List<string> GetFloor(int client_id);

        List<string> GetRoom(int client_id);
        List<string> GetCondition();

        List<string> GetDepCostCenter();
        Task<bool> InsertBuilding(InventoryBuildingsModel inventoryBuildingsModel);
        Task<bool> InsertFloor(InventoryFloorsModel inventoryFloorsModel);
    }
}
