using InventoryApi.DBContext;
using InventoryApi.DBModels.InventoryDBModels;
using InventoryApi.Models;
using InventoryApi.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryApi.Repository
{
    public class DonationInventoryRepository : IDonationInventoryRepository
    {
        InventoryContext _dbContext;
        SSIRequestContext _requestContext;

        IConfiguration _configuration { get; }

        public DonationInventoryRepository(InventoryContext dbContext,IConfiguration configuration, SSIRequestContext requestContext)
        {
            _dbContext = dbContext;
            _requestContext = requestContext;
        }

        public async Task<List<DonationInventoryModel>> GetInventory(int categoryid)
        {

            try
            {
                List<Inventory> result = new List<Inventory>();

                if (categoryid > 0)
                {

                    var inventoryModelsQuery = (from i in _dbContext.Inventories
                                           join ii in _dbContext.InventoryItems on i.InventoryID equals ii.InventoryID into invitem
                                           from ii in invitem.DefaultIfEmpty()
                                           join oi in _dbContext.OrderItems on i.InventoryID equals oi.InventoryID into ordinvitem
                                           from oi in ordinvitem.DefaultIfEmpty()
                                           join o in _dbContext.Orders on oi.OrderID equals o.OrderID into ord
                                           from o in ord.DefaultIfEmpty()

                                               //join iii in _dbContext.InventoryItemImages on ii.inv_item_id equals iii.inv_item_id into invitemimg
                                               //from InventoryItemImages in invitemimg.DefaultIfEmpty()
                                           where i.ItemTypeID == categoryid
                                           where oi.Delivered == false
                                           //where o.DestBuilding.Contains("Donate")
                                           where oi.DestBuildingID.Equals(-3)
                                           //where oi.Completed == false
                                           select new DonationInventoryModel()
                                           {
                                               InventoryID = i.InventoryID,
                                               ItemCode = i.ItemCode,                                              
                                               Description = i.Description,
                                               //AdditionalDescription = i.AdditionalDescription,
                                               ManufacturerName = i.ManufacturerName,
                                               Fabric = i.Fabric,
                                               Finish = i.Finish,
                                              // Qty =  i.InventoryItems.Count,                                             

                                               InventoryItemModels = (from ii in _dbContext.InventoryItems
                                                                      where ii.InventoryID == i.InventoryID                                                                     
                                                                      select new InventoryItemModel
                                                                      {
                                                                          InventoryID = ii.InventoryID,
                                                                          InventoryItemID = ii.InventoryItemID,
                                                                          InventoryBuildingID = ii.InventoryBuildingID,
                                                                          InventoryFloorID = ii.InventoryFloorID,
                                                                          Room = ii.Room,
                                                                          ConditionId = ii.ConditionID,
                                                                          Condition = _dbContext.InventoryItemConditions.Where(c=>c.InventoryItemConditionID == ii.ConditionID).FirstOrDefault().ConditionName,
                                                                          Building = (ii.InventoryBuildingID == 0 ? "" : _requestContext.ClientLocations.Where(ib => ib.location_id == ii.InventoryBuildingID).FirstOrDefault().location_name),
                                                                          Floor = _dbContext.InventoryFloors.Where(f => f.InventoryFloorID == ii.InventoryFloorID).FirstOrDefault().InventoryFloorName,
                                                                          InventoryImageName = GetCartImages(ii.InventoryItemID, ii.InventoryID, ii.ConditionID) ?? i.MainImage,
                                                                          InventoryImageUrl = _configuration.GetValue<string>("ImgUrl") + i.ClientID + "/",                                                                       
                                                                      }).ToList(),

                                               ImageName = i.MainImage,
                                               ImageUrl = _configuration.GetValue<string>("ImgUrl") + i.ClientID + "/"
                                           }).AsQueryable();

                    var inventoryModels = await inventoryModelsQuery.ToListAsync();

                    if (inventoryModels.Count > 0)
                    {
                        var invModels = inventoryModels.GroupBy(x => x.InventoryID).Select(s => s.First()).Distinct().ToList();

                        foreach (var item in invModels)
                        {
                            var gropinglocation = item.InventoryItemModels.GroupBy(x => new { x.InventoryBuildingID, x.InventoryFloorID, x.Room, x.Condition }).Select(g => new { g.Key.InventoryBuildingID, g.Key.InventoryFloorID, g.Key.Room, g.Key.Condition, qtyCount = g.Count() }).ToList();

                            //var invItemModelDisplay = gropinglocation.Select(x => new InventoryItemModel
                            //{
                            //    InventoryID = item.InventoryID,
                            //    InventoryItemID = item.InventoryItemModels.FirstOrDefault().InventoryItemID,
                            //    InventoryImageName = item.ImageName,
                            //    InventoryImageUrl = item.ImageUrl.FirstOrDefault(),
                            //    Building = _requestContext.ClientLocations.Where(ib => ib.InventoryBuildingID == x.InventoryBuildingID).FirstOrDefault().InventoryBuildingName,
                            //    Floor = _dbContext.InventoryFloors.Where(f => f.InventoryFloorID == x.InventoryFloorID).FirstOrDefault().InventoryFloorName,
                            //    Room = x.Room,
                            //    Condition = x.Condition,
                            //    Qty = x.qtyCount,
                            //    ItemCode = item.ItemCode,
                            //    Description = item.Description

                            //});

                            //invModels.Where(r => r.InventoryID == item.InventoryID)
                            //  .Select(r => { r.InventoryItemModelsDisplay = invItemModelDisplay.ToList(); return r; }).ToList();

                            StringBuilder stringBuilder = new StringBuilder();
                            int index = 1;
                            foreach (var grp in gropinglocation)
                            {
                                string building = (grp.InventoryBuildingID == 0 ? "" : _requestContext.ClientLocations.Where(ib => ib.location_id == grp.InventoryBuildingID).FirstOrDefault().location_name);
                                string floor = _dbContext.InventoryFloors.Where(f => f.InventoryFloorID == grp.InventoryFloorID).FirstOrDefault().InventoryFloorName;
                                if (gropinglocation.Count == index)
                                    stringBuilder.Append($"{building} : {floor} : Loc {grp.Room} : Cond {grp.Condition} : Qty {grp.qtyCount}");
                                else                                             
                                    stringBuilder.Append($"{building} : {floor} : Loc {grp.Room} : Cond {grp.Condition} : Qty {grp.qtyCount} \n");

                                index++;
                            }

                            invModels.Where(r => r.InventoryID == item.InventoryID)
                               .Select(r => { r.TootipInventoryItem = stringBuilder.ToString(); return r; }).ToList();
                        }

                        return invModels;

                    }
                    else
                        return new List<DonationInventoryModel>();

                }
                else
                    return new List<DonationInventoryModel>();

            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public async Task<List<ItemTypesModel>> GetInventoryCategory()
        {
            try
            {
                //return _dbContext.Inventories.Where(i => !string.IsNullOrEmpty(i.category)).Select(cat => cat.category).Distinct().ToList();

                var donationCategoryQuery = (from o in _dbContext.Orders
                                               join oi in _dbContext.OrderItems on o.OrderID equals oi.OrderID 
                                               join i in _dbContext.Inventories on oi.InventoryID equals i.InventoryID 
                                               join it in _dbContext.ItemTypes on i.ItemTypeID equals it.ItemTypeID
                                               where oi.Delivered == false
                                               where oi.DestBuildingID.Equals(-3)
                                               //where oi.Completed == false
                                               orderby i.ItemTypeID,i.ItemCode                                       
                                               select new ItemTypesModel()
                                               {
                                                   ItemTypeID = it.ItemTypeID,
                                                   ItemTypeName = it.ItemTypeName                                                   
                                               }
                                               ).AsQueryable();

                var donationCategory = await donationCategoryQuery.Distinct().ToListAsync();

                //if (donationCategory.Count > 0)
                //{
                //    var itemTypes = donationCategory.Select(item => new ItemTypesModel 
                //    { 
                //        ItemTypeID = item.ItemTypeID, ItemTypeName = item.ItemTypeName 
                //    });

                //    // donationCategory = donationCategory.GroupBy(x =>x.).Select(s => s.First()).Distinct().ToList();

                //}

                 return donationCategory;

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private string GetCartImages(int inventoryitemid, int inventoryid, int conditionid)
        {
            string imagename;
            var entity = _dbContext.InventoryImages.FirstOrDefault(invimg => invimg.InventoryItemID == inventoryitemid && invimg.ConditionID == conditionid);

            if (entity != null)
            {
                imagename = entity.ImageName;
            }
            else
            {
                var inventoryImagesEntity = _dbContext.InventoryImages.FirstOrDefault(invimg => invimg.InventoryID == inventoryid && invimg.ConditionID == conditionid);

                if (inventoryImagesEntity == null)
                {
                    inventoryImagesEntity = _dbContext.InventoryImages.FirstOrDefault(invimg => invimg.InventoryID == inventoryid && invimg.InventoryItemID == null);
                }
                imagename = (inventoryImagesEntity != null ? inventoryImagesEntity.ImageName : "");
            }
            return imagename;
            //return await Task.Run(() => imagename);
        }

    }
}
