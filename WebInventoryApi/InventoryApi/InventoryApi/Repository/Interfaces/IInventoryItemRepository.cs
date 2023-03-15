using InventoryApi.DBModels;
using InventoryApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryApi.Repository.Interfaces
{
    public interface IInventoryItemRepository
    {
        Task<List<InventoryBuildingsModel>> GetBuilding(int client_id);
        Task<List<InventoryBuildingsModel>> GetCartBuilding(int client_id);
        Task<List<InventoryFloorsModel>> GetFloor(int client_id);

        Task<List<string>> GetRoom(int client_id);
        Task<List<InventoryItemConditionModel>> GetCondition();

        List<string> GetDepCostCenter();
        Task<InventoryBuildingsModel> InsertBuilding(InventoryBuildingsModel inventoryBuildingsModel);
        Task<InventoryFloorsModel> InsertFloor(InventoryFloorsModel inventoryFloorsModel);

        Task<List<InventoryItemWarrantyDisplayModel>> GetWarranytDisplayItems(int inventoryId,int warrantyyear);

        Task<string> UpdateInventoryItems(int inventoryitemid, int totalrow);
        Task<string> UpdateInventoryItems(int inventoryitemid, int totalrow,int conditionid);
        Task<List<InventoryItemStatusModel>> GetStatus();

        Task<List<ChildInventoryItemModel>> GetChildInventoryItems(InventoryCartOrderItemModel itemModel);
    }
}
