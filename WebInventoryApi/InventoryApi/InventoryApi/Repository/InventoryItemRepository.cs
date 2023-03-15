using Microsoft.EntityFrameworkCore.Storage;
using InventoryApi.DBContext;
using InventoryApi.DBModels.InventoryDBModels;
using InventoryApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryApi.Helpers;
using Microsoft.EntityFrameworkCore;
using InventoryApi.Repository.Interfaces;
using Microsoft.Extensions.Configuration;

namespace InventoryApi.Repository
{
    public class InventoryItemRepository : IInventoryItemRepository
    {
        IConfiguration _configuration { get; }
        InventoryContext _dbContext;
        SSIRequestContext _requestContext;
        public InventoryItemRepository(InventoryContext dbContext, IConfiguration configuration, SSIRequestContext requestContext)
        {
            _dbContext = dbContext;
            _requestContext = requestContext;
            _configuration = configuration;
            Common._dbContext = dbContext;
        }
        public async Task<List<InventoryBuildingsModel>> GetBuilding(int client_id)
        {
            try
            {
                return await _requestContext.ClientLocations.Where(x=>x.client_id == client_id)
                            .Select(bldg => new InventoryBuildingsModel() 
                            { 
                                InventoryBuildingId = bldg.location_id
                            , InventoryBuildingName = bldg.location_name 
                            }
                            )
                            .Distinct()
                            .ToListAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<InventoryBuildingsModel>> GetCartBuilding(int client_id)
        {
            try
            {
                //var cartBuildings =  _requestContext.ClientLocations.Where(x => x.ClientID == client_id || (x.ClientID == 1 && x.OrderSequence != 0))
                //                    .Select(bldg => new InventoryBuildingsModel()
                //                    {
                //                        InventoryBuildingId = bldg.InventoryBuildingID,
                //                        InventoryBuildingName = bldg.InventoryBuildingName,
                //                        OrderSequence = bldg.OrderSequence
                //                    })                                    
                //                    .Distinct()
                //                    .OrderBy(ord => ord.OrderSequence)
                //                    .ThenBy(ord => ord.InventoryBuildingName)
                //                    .AsQueryable();

                var cartBuildings = _requestContext.ClientLocations.Where(x => x.client_id == client_id 
                                                                    //|| (x.ClientID == 1 && x.OrderSequence != 0)
                                                                    )   
                             .Select(bldg => new InventoryBuildingsModel()
                             {
                                 InventoryBuildingId = bldg.location_id,
                                 InventoryBuildingName = bldg.location_name
                                 //OrderSequence = bldg.OrderSequence
                             })
                             .Distinct()
                            // .OrderBy(ord => ord.OrderSequence)
                             //.ThenBy(ord => ord.InventoryBuildingName)
                             .OrderBy(ord=>ord.InventoryBuildingName)
                             .AsQueryable();

                return await cartBuildings.ToListAsync();

                //var cartBuildings = _requestContext.ClientLocations.Where(x => x.ClientID == client_id || (x.ClientID == 1 && x.OrderSequence != 0))
                //                    .OrderBy(x => x.OrderSequence).ThenBy(x => x.InventoryBuildingName);

                //return cartBuildings.Select(bldg => new InventoryBuildingsModel()
                //{
                //    InventoryBuildingId = bldg.InventoryBuildingID,
                //    InventoryBuildingName = bldg.InventoryBuildingName,
                //    OrderSequence = bldg.OrderSequence
                //}).Distinct().OrderBy(x => x.OrderSequence).ThenBy(x => x.InventoryBuildingName).ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<InventoryFloorsModel>> GetFloor(int client_id)
        {
            try
            {
                return await _dbContext.InventoryFloors.Where(flr=>flr.InventoryFloorID != 0 && flr.InventoryFloorID != -1)
                                .Select(flr => new InventoryFloorsModel()
                                { InventoryFloorId = flr.InventoryFloorID, InventoryFloorName = flr.InventoryFloorName })
                                .Distinct()
                                .ToListAsync();

                //return await _dbContext.InventoryFloors.Select(flr => new InventoryFloorsModel() 
                //{ InventoryFloorId = flr.InventoryFloorID, InventoryFloorName = flr.InventoryFloorName }).Distinct().ToListAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<string>> GetRoom(int client_id)
        {
            try
            {
                return await _dbContext.InventoryItems.Where(x => x.Room != "" && x.ClientID == client_id).Select(f => f.Room).Distinct().ToListAsync();

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<InventoryItemConditionModel>> GetCondition()
        {
            try
            {
                List<InventoryItemConditionModel> listCond = new List<InventoryItemConditionModel>();
                listCond = await _dbContext.InventoryItemConditions
                                .Select(cond => new InventoryItemConditionModel()
                                {
                                    InventoryItemConditionID = cond.InventoryItemConditionID,
                                    ConditionName = cond.ConditionName,
                                    OrderSequence = cond.OrderSequence
                                })
                                .ToListAsync();
                                

                //listCond.Add("Good");
                //listCond.Add("Fair");
                //listCond.Add("Poor");
                //listCond.Add("Damage");
                //listCond.Add("Missing Parts");
                return listCond;

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<string> GetDepCostCenter()
        {
            try
            {
                List<string> listDepCost = new List<string>();
                listDepCost.Add("HR");
                listDepCost.Add("Management");
                listDepCost.Add("IT");
                listDepCost.Add("Network");
                listDepCost.Add("Marketing");
                return listDepCost;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<InventoryBuildingsModel> InsertBuilding(InventoryBuildingsModel inventoryBuildingsModel)
        {
            //bool result = false;
            InventoryBuildingsModel model = new InventoryBuildingsModel();

            var entity = await _requestContext.ClientLocations.Where(x => x.location_name.ToLower() == inventoryBuildingsModel.InventoryBuildingName.ToLower() && x.client_id == inventoryBuildingsModel.ClientID).FirstOrDefaultAsync();

            if (entity != null)
            {
                //model.InventoryBuildingId = entity.InventoryBuildingID;
                //model.ClientID = entity.ClientID;
                //model.InventoryBuildingName = entity.InventoryBuildingName;
                //model.UserId = entity.CreateID;
            }
            else
            {
                using (IDbContextTransaction transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        InventoryBuildings buildingModel = new InventoryBuildings();
                        buildingModel.ClientID = inventoryBuildingsModel.ClientID;
                        buildingModel.InventoryBuildingCode = inventoryBuildingsModel.InventoryBuildingName.Replace(" ", "");
                        buildingModel.InventoryBuildingName = inventoryBuildingsModel.InventoryBuildingName;
                        buildingModel.InventoryBuildingDesc = inventoryBuildingsModel.InventoryBuildingName;
                        buildingModel.StatusID = (int)Enums.Status.Active;
                        buildingModel.OrderSequence = 1;
                        buildingModel.CreateID = inventoryBuildingsModel.UserId;
                        buildingModel.CreateDateTime = System.DateTime.Now;
                        buildingModel.UpdateID = null;
                        buildingModel.UpdateDateTime = null;

                        await _dbContext.AddAsync(buildingModel);
                        await _dbContext.SaveChangesAsync();


                        model.InventoryBuildingId = buildingModel.InventoryBuildingID;
                        model.ClientID = buildingModel.ClientID;
                        model.InventoryBuildingName = buildingModel.InventoryBuildingName;
                        model.UserId = buildingModel.CreateID;

                        await transaction.CommitAsync();

                        //return model;
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        throw;
                    }
                }
            }

            return model;
        }

        public async Task<InventoryFloorsModel> InsertFloor(InventoryFloorsModel inventoryFloorsModel)
        {
            //bool result = false;
            InventoryFloorsModel model = new InventoryFloorsModel();

            using (IDbContextTransaction transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    InventoryFloors floorModel = new InventoryFloors();
                    floorModel.ClientID = inventoryFloorsModel.ClientID;
                    floorModel.InventoryFloorCode = inventoryFloorsModel.InventoryFloorName.Replace(" ","");
                    floorModel.InventoryFloorName = inventoryFloorsModel.InventoryFloorName;
                    floorModel.InventoryFloorDesc = inventoryFloorsModel.InventoryFloorName;
                    floorModel.StatusID = (int)Enums.Status.Active;
                    floorModel.OrderSequence = 1;
                    floorModel.CreateID = inventoryFloorsModel.UserId;
                    floorModel.CreateDateTime = System.DateTime.Now;
                    floorModel.UpdateID = null;
                    floorModel.UpdateDateTime = null;


                    await _dbContext.AddAsync(floorModel);
                    await _dbContext.SaveChangesAsync();

                    model.InventoryFloorId = floorModel.InventoryFloorID;
                    model.ClientID = floorModel.ClientID;
                    model.InventoryFloorName = floorModel.InventoryFloorName;
                    model.UserId = floorModel.CreateID;

                    await transaction.CommitAsync();

                    //result = true;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }

           // return result;
            return model;
        }


        public async Task<List<InventoryItemWarrantyDisplayModel>> GetWarranytDisplayItems(int inventoryId,int warrantyyear)
        {
            List<InventoryItemWarrantyDisplayModel> inventoryItemWarrantyDisplayModels = new List<InventoryItemWarrantyDisplayModel>();

           // var entity = await _dbContext.InventoryItems.Where(ii => ii.InventoryID == inventoryId && ii.StatusID != (int)Enums.Status.Reserved).ToListAsync();
            var entity = await _dbContext.InventoryItems.Where(ii => ii.InventoryID == inventoryId).ToListAsync();

            if(entity != null && entity.Count > 0)
            {
                //var gropinglocation = entity.GroupBy(e=> new { e.InventoryBuildingID, e.InventoryFloorID, e.Room, e.ConditionID } )
                //    .Select(g => new { g.Key.InventoryBuildingID, g.Key.InventoryFloorID, g.Key.Room, g.Key.ConditionID })
                //    .ToList();

                //inventoryItemWarrantyDisplayModels = gropinglocation.Select(x => new InventoryItemWarrantyDisplayModel
                //{
                //    Building = _requestContext.ClientLocations.Where(ib => ib.InventoryBuildingID == x.InventoryBuildingID).FirstOrDefault().InventoryBuildingName,
                //    Floor = _dbContext.InventoryFloors.Where(f => f.InventoryFloorID == x.InventoryFloorID).FirstOrDefault().InventoryFloorName,
                //    Room = x.Room,
                //    Condition = _dbContext.InventoryItemConditions.Where(c=>c.InventoryItemConditionID == x.ConditionID).FirstOrDefault().ConditionName,
                //    PurchaseDate = entity.Where(i => i.InventoryBuildingID == x.InventoryBuildingID && i.InventoryFloorID == x.InventoryFloorID && i.Room == x.Room && i.ConditionID == x.ConditionID).FirstOrDefault().NonSSIPurchaseDate,
                //    //WarrantyYear = System.DateTime.Now.Year - entity.Where(i => i.Building == x.Building && i.Floor == x.Floor && i.Room == x.Room && i.Condition == x.Condition).FirstOrDefault().NonSSIPurchaseDate.Value.Year
                //    WarrantyYear = WarrantyYearCalc(entity.Where(i => i.InventoryBuildingID == x.InventoryBuildingID && i.InventoryFloorID == x.InventoryFloorID && i.Room == x.Room && i.ConditionID == x.ConditionID).FirstOrDefault().NonSSIPurchaseDate.Value.Date)
                //}).ToList();

                var gropinglocation = entity.GroupBy(e => new { e.InventoryBuildingID, e.InventoryFloorID, e.Room, e.ConditionID, e.NonSSIPurchaseDate, e.PoOrderDate })
                   .Select(g => new { g.Key.InventoryBuildingID, g.Key.InventoryFloorID, g.Key.Room, g.Key.ConditionID, g.Key.NonSSIPurchaseDate,g.Key.PoOrderDate })
                   .ToList();

                inventoryItemWarrantyDisplayModels = gropinglocation.Select(x => new InventoryItemWarrantyDisplayModel
                {
                    Building = (x.InventoryBuildingID == 0 ? "" : _requestContext.ClientLocations.Where(ib => ib.location_id == x.InventoryBuildingID).FirstOrDefault().location_name),
                    Floor = _dbContext.InventoryFloors.Where(f => f.InventoryFloorID == x.InventoryFloorID).FirstOrDefault().InventoryFloorName,
                    Room = x.Room,
                    Condition = _dbContext.InventoryItemConditions.Where(c => c.InventoryItemConditionID == x.ConditionID).FirstOrDefault().ConditionName,
                    PurchaseDate = (!string.IsNullOrEmpty(x.NonSSIPurchaseDate.ToString()) ? x.NonSSIPurchaseDate.Value.Date : !string.IsNullOrEmpty(x.PoOrderDate.ToString()) ? x.PoOrderDate.Value.Date : null),
                    ////PurchaseDate = entity.Where(i => i.InventoryBuildingID == x.InventoryBuildingID && i.InventoryFloorID == x.InventoryFloorID && i.Room == x.Room && i.ConditionID == x.ConditionID).FirstOrDefault().NonSSIPurchaseDate,
                   // WarrantyYear = WarrantyYearCalc((!string.IsNullOrEmpty(x.NonSSIPurchaseDate.ToString()) ? x.NonSSIPurchaseDate.Value.Date : !string.IsNullOrEmpty(x.PoOrderDate.ToString()) ? x.PoOrderDate.Value.Date ))
                }).ToList();

                inventoryItemWarrantyDisplayModels.ForEach(wm =>
                {
                    //wm.WarrantyYear = wm.PurchaseDate != null ? WarrantyYearCalc(wm.PurchaseDate.Value.Date) : null;
                    wm.WarrantyYear = wm.PurchaseDate != null ? WarrantyYearPassedCalc(wm.PurchaseDate.Value.Date, warrantyyear) : null;
                    
                });
            }

            return inventoryItemWarrantyDisplayModels;
        }

        private string WarrantyYearCalc(DateTime purchaseDate)
        {
            DateTime zeroTime = new DateTime(1, 1, 1);           
            DateTime curdate = DateTime.Now.ToLocalTime();
          
            TimeSpan span = curdate - purchaseDate;

            // because we start at year 1 for the Gregorian 
            // calendar, we must subtract a year here.

            int years = (zeroTime + span).Year - 1;
            int months = (zeroTime + span).Month - 1;
            //int days = (zeroTime + span).Day;

            //return $"Y - {years}, M - {months}, D - {days}";
            //return $"{years} : {months} : {days}";
            return $"{years} Yr \n {months} Mon";

        }

        private string WarrantyYearPassedCalc(DateTime purchaseDate,int warrantyyear)
        {
            DateTime zeroTime = new DateTime(1, 1, 1);
            DateTime warrantyEndDate = purchaseDate.AddYears(warrantyyear).Date;

            DateTime curdate = DateTime.Now.ToLocalTime();

            TimeSpan span = warrantyEndDate - curdate;
            int years = (zeroTime + span).Year - 1;
            int months = (zeroTime + span).Month - 1;

            return $"{years} Yr \n {months} Mon";

        }

        public async Task<string> UpdateInventoryItems(int inventoryitemid, int totalrow)
        {
            string result = "";
            using (IDbContextTransaction transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    var entity = await _dbContext.InventoryItems.Where(ii => ii.InventoryItemID == inventoryitemid && !ii.AddedToCartItem).FirstOrDefaultAsync();

                    if (entity != null)
                    {
                        entity.AddedToCartItem = true;

                        _dbContext.Update(entity);
                        await _dbContext.SaveChangesAsync();

                        result = "success";

                        await transaction.CommitAsync();
                    }
                    else
                    {
                        result = "failed";
                        if (entity == null)
                            result = $"{result} due to records not exists as matching criteria for inventory item id is {inventoryitemid}";
                        //else if (entity.Count != totalrow)
                        //    result = $"{result} due to no of record not found as try to add in cart";

                        await transaction.RollbackAsync();
                    }


                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }

            return result;

        }

        public async Task<string> UpdateInventoryItems(int inventoryitemid, int totalrow,int conditionid)
        {
            string result = "";
            using (IDbContextTransaction transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    List<InventoryItem> entity = new List<InventoryItem>();
                    if (totalrow > 1)
                        entity = _dbContext.InventoryItems.Where(ii => ii.InventoryItemID >= inventoryitemid && !ii.AddedToCartItem && ii.ConditionID == conditionid).Take(totalrow).ToList();
                    else
                        entity = _dbContext.InventoryItems.Where(ii => ii.InventoryItemID == inventoryitemid && !ii.AddedToCartItem && ii.ConditionID == conditionid).Take(totalrow).ToList();


                    if (entity != null && entity.Count > 0 && entity.Count.Equals(totalrow))
                    {
                        entity.ForEach(x =>
                        {
                            x.AddedToCartItem = true;
                        });

                        _dbContext.UpdateRange(entity);
                        await _dbContext.SaveChangesAsync();

                        result = "success";

                        await transaction.CommitAsync();

                    }
                    else
                    {
                        result = "failed";
                        if (entity == null && entity.Count <= 0)
                            result = $"{result} due to records not exists as matching criteria for inventory item id is {inventoryitemid}";
                        else if (entity.Count != totalrow)
                            result = $"{result} due to no of record not found as try to add in cart for inventory item id is {inventoryitemid}";
                        
                        await transaction.RollbackAsync();
                        // break;
                    }

                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }

            return result;

        }

        public async Task<List<InventoryItemStatusModel>> GetStatus()
        {
            try
            {
                List<InventoryItemStatusModel> inventoryItemStatusModelsList = new List<InventoryItemStatusModel>();

                inventoryItemStatusModelsList.Add(new InventoryItemStatusModel()
                {
                    StatusId = (int)Enums.Status.Active,
                    StatusName = Enums.Status.Active.ToString()
                });
                inventoryItemStatusModelsList.Add(new InventoryItemStatusModel()
                {
                    StatusId = (int)Enums.Status.Inactive,
                    StatusName = Enums.Status.Inactive.ToString()
                });
                inventoryItemStatusModelsList.Add(new InventoryItemStatusModel()
                {
                    StatusId = (int)Enums.Status.Reserved,
                    StatusName = Enums.Status.Reserved.ToString()
                });

                return await Task.Run(()=> inventoryItemStatusModelsList);


            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public async Task<List<ChildInventoryItemModel>> GetChildInventoryItems(InventoryCartOrderItemModel itemModel)
        {
            try
            {
                List<ChildInventoryItemModel> childInventoryItems = new List<ChildInventoryItemModel>();

                var entity = await _dbContext.InventoryItems
                                    .Where(ii => ii.InventoryID == itemModel.InventoryID
                                           && ii.InventoryBuildingID == itemModel.InventoryBuildingID
                                           && ii.InventoryFloorID == itemModel.InventoryFloorID
                                           && ii.Room.Contains(itemModel.Room)
                                           && ii.ConditionID == itemModel.ConditionId
                                           && ii.AddedToCartItem == false
                                           && ii.StatusID == (int)Enums.Status.Active
                                           ).ToListAsync();

                childInventoryItems = entity.Select(x => new ChildInventoryItemModel()
                    {
                        InventoryItemID = x.InventoryItemID,
                        InventoryID = x.InventoryID,
                        Building = (x.InventoryBuildingID == 0 ? "" : _requestContext.ClientLocations.Where(bldg=>bldg.location_id == x.InventoryBuildingID).FirstOrDefault().location_name),
                        Floor = _dbContext.InventoryFloors.Where(bldg=>bldg.InventoryFloorID == x.InventoryFloorID).FirstOrDefault().InventoryFloorName,
                        Room = x.Room,
                        Condition = _dbContext.InventoryItemConditions.Where(cond=>cond.InventoryItemConditionID == x.ConditionID).FirstOrDefault().ConditionName,
                        ConditionId = x.ConditionID,
                        Qty = 1,
                        InventoryImageName = Common.GetCartImages(x.InventoryItemID, x.InventoryID, x.ConditionID) ??  itemModel.InventoryImageName,
                        InventoryImageUrl = itemModel.InventoryImageUrl,
                        BucketName = _configuration.GetValue<string>("AwsConfig:BuketName"),
                        DamageNotes = x.DamageNotes
                        //ImagePath = $"inventory/{x.ClientID}/{item.InventoryItemModels.Where(i => i.Building == x.Building && i.Floor == x.Floor && i.Room == x.Room && i.Condition == x.Condition).FirstOrDefault().InventoryImageName}"

                }
                ).ToList();

                childInventoryItems.ForEach(ch =>
                {
                    var inventoryImageName = Common.GetCartImages(ch.InventoryItemID, ch.InventoryID, ch.ConditionId) ?? itemModel.InventoryImageName;
                    ch.InventoryImageName = inventoryImageName;
                    ch.ImagePath = $"{_configuration.GetValue<string>("AwsConfig:S3Folder")}/{itemModel.ClientID}/images/{inventoryImageName}";
                });
              
                return await Task.Run(()=> childInventoryItems);
            }
            catch(Exception ex)
            {
                throw;
            }
        }
    }
}
