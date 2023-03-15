using SeattleInventoryApi.DBContext;
using SeattleInventoryApi.DBModels;
using SeattleInventoryApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeattleInventoryApi.Repository
{
    public class InventoryItemImageRepository : IInventoryItemImageRepository
    {
        InventoryContext _dbContext;
        public InventoryItemImageRepository(InventoryContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<InventoryItemImagesModel> GetInventoryItemImages(int inv_id)
        {
            try
            {
                // return _dbContext.InventoryItemImages.Where(iii=>iii.inv_item_id == inv_item_id).ToList();
                //var entity = _dbContext.ImageDataModels.Where(item => _object.Select(x => x.SubmissionId).Contains(item.SubmissionId)).ToList();

               // return _dbContext.InventoryItemImages.Where(iii => _dbContext.InventoryItems.Where(x=>x.inventory_id == inv_id).Select(y=>y.inv_item_id).Contains(iii.inv_item_id)).ToList();
                var invitemimg =  _dbContext.InventoryItemImages.Where(iii => _dbContext.InventoryItems.Where(x=>x.inventory_id == inv_id).Select(y=>y.inv_item_id).Contains(iii.inv_item_id)).ToList();

                var invitem = _dbContext.InventoryItems.Where(ii => ii.inventory_id == inv_id).ToList();

                var grpInvItem = invitem.GroupBy(x => new { x.building, x.floor, x.mploc, x.cond }).Select(g => new { g.Key.building, g.Key.floor, g.Key.mploc, g.Key.cond, qtyCount = g.Count() }).ToList();

                List<InventoryItemImagesModel> inventoryItemImagesModel = new List<InventoryItemImagesModel>();

                inventoryItemImagesModel = invitemimg.Select(x => new InventoryItemImagesModel()
                {
                    inv_item_img_id = x.inv_item_img_id,
                    inv_item_id = x.inv_item_id,
                    inv_item_img_guid = x.inv_item_img_guid,
                    image_name = x.image_name,
                    image_url = x.image_url,
                    client_id = x.client_id
                }).ToList();

                
                    
                foreach (var item in grpInvItem)
                {
                    int extInvItemId = invitem.Where(x => x.building == item.building && x.floor == item.floor && x.mploc == item.mploc && x.cond == item.cond).FirstOrDefault().inv_item_id;
                    string strLocation = $"{item.building} : {item.floor} : Loc {item.mploc} : Cond {item.cond} : Qty {item.qtyCount}";

                    inventoryItemImagesModel.Where(x => x.inv_item_id == extInvItemId).Select(s => { s.location = strLocation; return s; }).ToList();
                }


                return inventoryItemImagesModel;
                //var inv_item_images = (from ii in _dbContext.InventoryItems
                //                       join iii in _dbContext.InventoryItemImages on ii.inv_item_id equals iii.inv_item_id into invitemimg1
                //                       from iii_new in invitemimg1.DefaultIfEmpty()                                          
                //                       where ii.inventory_id == inv_id
                //                       //where ii.client_id == client_id
                //                       select new InventoryItemImagesModel
                //                       {
                //                           inv_item_img_id = iii_new.inv_item_img_id,
                //                           inv_item_id = iii_new.inv_item_id,
                //                           inv_item_img_guid = iii_new.inv_item_img_guid,
                //                           image_name = iii_new.image_name == null ? "" : iii_new.image_name,
                //                           image_url = iii_new.image_url == null ? "" : iii_new.image_url,
                //                           client_id = iii_new.client_id == null ? 0 : iii_new.client_id
                //                           //inventoryItemModel = new InventoryItemModel()
                //                           //{
                //                           //    inv_item_id = ii.inv_item_id,
                //                           //    inventory_id = ii.inventory_id,
                //                           //    building = ii.building,
                //                           //    floor = ii.floor,
                //                           //    mploc = ii.mploc,
                //                           //    cond = ii.cond,
                //                           //    client_id = ii.client_id
                //                           //}

                //                       }
                //                       ).ToList();


                // return _dbContext.InventoryItemImages.Where(iii => _dbContext.InventoryItems.Where(x=>x.inventory_id == inv_id).Select(y=>y.inv_item_id).Contains(iii.inv_item_id)).ToList();

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<InventoryItemImagesModel> GetInventoryItemImagesForCondition(int inv_item_id, string condition)
        {
            try
            {
                var invitemimg = _dbContext.InventoryItemImages.Where(iii => iii.inv_item_id == inv_item_id).ToList();


                List<InventoryItemImagesModel> inventoryItemImagesModel = new List<InventoryItemImagesModel>();

                inventoryItemImagesModel = invitemimg.Select(x => new InventoryItemImagesModel()
                {
                    inv_item_img_id = x.inv_item_img_id,
                    inv_item_id = x.inv_item_id,
                    inv_item_img_guid = x.inv_item_img_guid,
                    image_name = x.image_name,
                    image_url = x.image_url,
                    client_id = x.client_id
                }).ToList();

                return inventoryItemImagesModel;

            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
