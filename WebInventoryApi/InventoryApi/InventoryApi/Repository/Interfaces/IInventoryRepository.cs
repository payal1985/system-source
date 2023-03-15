using InventoryApi.DBModels;
using InventoryApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryApi.Repository.Interfaces
{
    public interface IInventoryRepository
    {
        Task<List<InventoryModel>> GetInventory(int itemTypeId, int client_id, int bulding, int floor,string room,int cond,int startindex);
        //List<InventoryModel> GetInventory1(string category, string bulding, string floor);
        Task<List<ItemTypesModel>> GetInventoryCategory(int client_id);

        Task<List<InventoryModel>> SearchInventory(string strSearch, int client_id, int bulding, int floor, string room, int cond);

        Task<InventoryItemsTypesModel> GetSpecificInventoryItemsTypes(int clientid);

        Task<List<InventoryModel>> GetItemTypesInventoy(int spacetypeid, int ownershipid, int clientid, int buildingid, int floorid, string room, int condition);
        Task<string> GetAvailTooltip(List<InventoryItemModel> model);
        Task<string> GetResTooltip(List<InventoryItemModel> model);
        Task<List<InventoryItemModel>> GetCartList(InventoryModel model);
    }
}
