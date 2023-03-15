using InventoryApi.DBModels;
using InventoryApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryApi.Repository.Interfaces
{
    public interface IInventoryItemImageRepository
    {

        Task<List<InventoryItemImagesModel>> GetInventoryItemImages(int inv_id);
        Task<List<InventoryItemImagesModel>> GetInventoryItemImagesForCondition(int inv_item_id,int conditionid, int inventoryid);
        //List<string> GetInventoryCategory();
    }
}
