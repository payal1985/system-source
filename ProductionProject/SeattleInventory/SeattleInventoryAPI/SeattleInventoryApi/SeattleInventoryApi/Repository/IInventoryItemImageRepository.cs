using SeattleInventoryApi.DBModels;
using SeattleInventoryApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeattleInventoryApi.Repository
{
    public interface IInventoryItemImageRepository
    {

        List<InventoryItemImagesModel> GetInventoryItemImages(int inv_id);
        List<InventoryItemImagesModel> GetInventoryItemImagesForCondition(int inv__item_id,string condition);
        //List<string> GetInventoryCategory();
    }
}
