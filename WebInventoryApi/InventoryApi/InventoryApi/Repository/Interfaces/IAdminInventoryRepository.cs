using InventoryApi.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryApi.Repository.Interfaces
{
    public interface IAdminInventoryRepository
    {
        Task<AdminInventoryModel> GetAdminInventory(int clientId, int currentPage, int perPageRows, int startIndex);
        Task<List<InventoryItemModel>> GetAdminInventoryItem(int clientId, int inventoryId, int buildingid, int floorid, string room);
        Task<AdminInventoryModel> SearchAdminInventory(int clientId, int currentPage, int perPageRows, int startIndex,string search);

        Task<bool> UpdateAdminInventory(int inventoryid, string column, string value, int userId);
        Task<bool> UpdateAdminInventoryItem(InventoryItemModel inventoryitemmodel, string column, int value, int userId);

        Task<bool> UploadFileToS3Bucket(IFormFile files,int clientId);
        Task<string> UploadImageToS3Bucket(IFormFile file, int clientId,int inventoryId, int userId);

        Task<List<AdminInventoryItemTypesOptionSetModel>> GetInventoryItemTypes(int clientid, int itemtypeid);

        Task<bool> EditInventory(int userid, string apicallurl, AdminInventory adminInventory);
        Task<AdminInventory> CreateInventory(int userid, int clientid, CreateInventoryModel createInventory);
        Task<List<StatusModel>> GetStatus();

        Task<bool> EditInventoryItem(int userid, int prevQty, string apicallurl, InventoryItemModel inventoryItemModel);

        Task<List<InventoryItemModel>> GetAdminChildInventoryItem(int inventoryid, int buildingid, int floorid, string room, int conditionid);
        Task<AdminInventoryModel> GetAdminInventoryByLocation(int clientId, int currentPage, int perPageRows, int startIndex, int buildingid, int floorid, string room);

        Task<List<ItemTypesModel>> GetItemTypes(int clientid);
    }
}
