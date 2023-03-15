using Microsoft.EntityFrameworkCore;
using InventoryApi.DBContext;
using InventoryApi.DBModels;
using InventoryApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using InventoryApi.Helpers;
using InventoryApi.Repository.Interfaces;

namespace InventoryApi.Repository
{
    public class InventoryRepository : IInventoryRepository
    {
        InventoryContext _dbContext;
        SSIRequestContext _requestContext;

        IConfiguration _configuration { get; }
        private readonly ILoggerManagerRepository _logger;
        IAws3Repository _aws3Repository { get; }


        public InventoryRepository(InventoryContext dbContext, IConfiguration configuration
            , ILoggerManagerRepository logger
            ,IAws3Repository aws3Repository
            ,SSIRequestContext requestContext
            )
        {
            _logger = logger;
            _dbContext = dbContext;
            _configuration = configuration;
            _aws3Repository = aws3Repository;
            _requestContext = requestContext;
            Common._dbContext = dbContext;
            //Common._requestContext = requestContext;
        }

        public async Task<List<InventoryModel>> GetInventory(int itemTypeId, int client_id, int bulding, int floor,string room,int cond,int startindex)
        {

            try
            {
               // List<Inventory> result = new List<Inventory>();

                if (itemTypeId > 0)
                {

                    #region oldcode
                    //var resultInv = (from i in _dbContext.Inventories
                    //                 join ii in _dbContext.InventoryItems on i.InventoryID equals ii.InventoryID //into invitem
                    //                 //from ii in invitem.DefaultIfEmpty()
                    //                     //join iii in _dbContext.InventoryItemImages on ii.inv_item_id equals iii.inv_item_id into invitemimg
                    //                     //from InventoryItemImages in invitemimg.DefaultIfEmpty()
                    //                 where i.ItemTypeID == itemTypeId
                    //                 where i.ClientID == client_id
                    //                 where ii.Building.Contains((!string.IsNullOrEmpty(bulding) ? bulding : ""))
                    //                 where ii.Floor.Contains((!string.IsNullOrEmpty(floor) ? floor : ""))
                    //                 where ii.Room.Contains((!string.IsNullOrEmpty(room) ? room : ""))
                    //                 where ii.Condition.Contains((!string.IsNullOrEmpty(cond) ? cond : ""))
                    //                 where ii.Building != "Donate"
                    //                 select new InventoryModel()
                    //                 {
                    //                     InventoryID = i.InventoryID,
                    //                     ItemCode = i.ItemCode,
                    //                     //description = i.description.ToString(),
                    //                     Description = (!string.IsNullOrEmpty(i.AdditionalDescription) ? i.Description + "-" + i.AdditionalDescription : i.Description),
                    //                     Manuf = i.Manuf,
                    //                     Height = i.Height.ToString(),
                    //                     Width = i.Width.ToString(),
                    //                     Depth = i.Depth.ToString(),
                    //                     //hwd_str = (i.height.ToString() == "0.000" ? "" : i.height.ToString() + "h X ") + (i.width.ToString() == "0.000" ? "" : i.width.ToString() + "w X ") + (i.depth.ToString() == "0.000" ? "" : i.depth.ToString() + "d"),
                    //                     //hwd_str = (i.height.ToString() == "0.000" ? "" : Math.Round(Convert.ToDecimal(i.height), 1).ToString() + "h X ") + (i.width.ToString() == "0.000" ? "" : Math.Round(Convert.ToDecimal(i.width),1).ToString() + "w X ") + (i.depth.ToString() == "0.000" ? "" : Math.Round(Convert.ToDecimal(i.depth),1).ToString() + "d"),
                    //                     HWDStr = (i.Height.ToString() == "-1.000" ? "" : Decimal.Round(i.Height, 1).ToString() + "h X ") + (i.Width.ToString() == "-1.000" ? "" : Decimal.Round(i.Width, 1).ToString() + "w X ") + (i.Depth.ToString() == "-1.000" ? "" : Decimal.Round(i.Depth, 1).ToString() + "d"),
                    //                     Fabric = i.Fabric,
                    //                     Finish = i.Finish,
                    //                     ClientID = i.ClientID,
                    //                     Category = i.Category,
                    //                     Qty = (!string.IsNullOrEmpty(bulding) && !string.IsNullOrEmpty(floor) && !string.IsNullOrEmpty(room) ? i.InventoryItems.Where(x => x.Building == bulding && x.Floor == floor && x.Room == room && x.Condition == cond).Count()
                    //                                                                                           : (!string.IsNullOrEmpty(bulding) ? i.InventoryItems.Where(x => x.Building == bulding).Count()
                    //                                                                                                                             : !string.IsNullOrEmpty(floor) ? i.InventoryItems.Where(x => x.Floor == floor).Count()
                    //                                                                                                                             : !string.IsNullOrEmpty(room) ? i.InventoryItems.Where(x => x.Room == room).Count()
                    //                                                                                                                             : !string.IsNullOrEmpty(cond) ? i.InventoryItems.Where(x => x.Condition == cond).Count()
                    //                                                                                                                                                            : i.InventoryItems.Count)),

                    //                     InventoryItemModels = (from ii in _dbContext.InventoryItems
                    //                                            where ii.InventoryID == i.InventoryID
                    //                                            where ii.Building.Contains((!string.IsNullOrEmpty(bulding) ? bulding : ""))
                    //                                            where ii.Floor.Contains((!string.IsNullOrEmpty(floor) ? floor : ""))
                    //                                            where ii.Room.Contains((!string.IsNullOrEmpty(room) ? room : ""))
                    //                                            where ii.Condition.Contains((!string.IsNullOrEmpty(cond) ? cond : ""))
                    //                                            where ii.Building != "Donate"
                    //                                            select new InventoryItemModel
                    //                                            {
                    //                                                InventoryID = ii.InventoryID,
                    //                                                InventoryItemID = ii.InventoryItemID,
                    //                                                Building = ii.Building,
                    //                                                Floor = ii.Floor,
                    //                                                Room = ii.Room,
                    //                                                Condition = ii.Condition,
                    //                                                ClientID = ii.ClientID

                    //                                            }).ToList(),

                    //                     ImageName = i.MainImage,
                    //                     ImageUrl = _configuration.GetValue<string>("ImgUrl") + i.ClientID + "/"
                    //                     //  qty = (!string.IsNullOrEmpty(floor) ? i.InventoryItems.GroupBy(x => new { x.floor }).Count() : i.InventoryItems.Count),

                    //                 }).AsQueryable();
                    #endregion

                    //var resultInv = (from i in _dbContext.Inventories
                    //                 where i.ItemTypeID == itemTypeId
                    //                 where i.ClientID == client_id
                    //                 select new InventoryModel
                    //                 {
                    //                     InventoryID = i.InventoryID,
                    //                     ItemCode = i.ItemCode.Contains("Unknown") ? i.ItemCode.Replace("Unknown","") : i.ItemCode,
                    //                     Description = i.Description,
                    //                     //ManufacturerName = i.ManufacturerName.Contains("Unknown") ? i.ManufacturerName.Replace("Unknown", "") :i.ManufacturerName,
                    //                     ManufacturerName = i.ManufacturerName,
                    //                     Height = i.Height.HasValue ? i.Height : 0,
                    //                     Width = i.Width.HasValue ? i.Width : 0,
                    //                     Depth = i.Depth.HasValue ? i.Depth : 0,
                    //                     ////HWDStr = (i.Height.ToString() == "-1.000" 
                    //                     ////           ? "" : Decimal.Round(i.Height, 1).ToString() + "h X ") + (i.Width.ToString() == "-1.000" 
                    //                     ////           ? "" : Decimal.Round(i.Width, 1).ToString() + "w X ") + (i.Depth.ToString() == "-1.000" 
                    //                     ////           ? "" : Decimal.Round(i.Depth, 1).ToString() + "d ") + (i.Diameter.ToString() != "-1.000" 
                    //                     ////           ? "X ":"") + (i.Diameter.ToString() == "-1.000"
                    //                     ////           ? "" : Decimal.Round(i.Diameter,1).ToString() + "dia"),
                    //                     //HWDStr = (i.Height.ToString() == "-1.000" ? "" : Decimal.Round(i.Height, 1).ToString() + "h X ") +
                    //                     //     // (i.Width.ToString() != "-1.0000" ? " X " : "") +
                    //                     //     (i.Width.ToString() == "-1.000" ? "" : Decimal.Round(i.Width, 1).ToString() + "w X ") +
                    //                     //     // (i.Depth.ToString() != "-1.000" ? " X " : "") + 
                    //                     //     (i.Depth.ToString() == "-1.000" ? "" : Decimal.Round(i.Depth, 1).ToString() + "d") +
                    //                     //     ((i.Depth.ToString() != "-1.000" && i.Diameter.ToString() != "-1.000") ? " X " : "") +
                    //                     //     (i.Diameter.ToString() == "-1.000" ? "" : Decimal.Round(i.Diameter, 1).ToString() + "dia"),

                    //                     //vol = i.Height.HasValue ? Decimal.Round(i.Height.Value, 5) : 0;

                    //                     HWDStr = (i.Height < 0 && !i.Height.HasValue ? "" : Decimal.Round(i.Height.Value, 1).ToString() + "h X ") +
                    //                          // (i.Width.ToString() != "-1.0000" ? " X " : "") +
                    //                          (i.Width < 0 && !i.Width.HasValue ? "" : Decimal.Round(i.Width.Value, 1).ToString() + "w X ") +
                    //                          // (i.Depth.ToString() != "-1.000" ? " X " : "") + 
                    //                          (i.Depth < 0 && !i.Depth.HasValue ? "" : Decimal.Round(i.Depth.Value, 1).ToString() + "d") +
                    //                          ((i.Depth <= 0 && i.Depth.HasValue && i.Diameter <= 0 && i.Diameter.HasValue) ? " X " : "") +
                    //                          (i.Diameter < 0 && !i.Diameter.HasValue ? "" : Decimal.Round(i.Diameter.Value, 1).ToString() + "dia"),
                    //                     Fabric = i.Fabric.Trim() + (!string.IsNullOrEmpty(i.Fabric2) ? "," + i.Fabric2 : "").Trim(),
                    //                     Finish = i.Finish.Trim() + (!string.IsNullOrEmpty(i.Finish2) ? "," + i.Finish2 : "").Trim(),
                    //                     ClientID = i.ClientID,
                    //                     Category = i.Category,
                    //                     ImageName = i.MainImage,
                    //                     ImageUrl = _configuration.GetValue<string>("ImgUrl") + i.ClientID + "/images/",
                    //                     WarrantyYears = i.WarrantyYears,
                    //                     BucketName = _configuration.GetValue<string>("AwsConfig:BuketName"),
                    //                     ImagePath = $"{_configuration.GetValue<string>("AwsConfig:S3Folder")}/{i.ClientID}/images/{i.MainImage}",
                    //                     FilePath = $"{_configuration.GetValue<string>("AwsConfig:S3Folder")}/{i.ClientID}/sustainability_files/{i.InventoryID}",
                    //                     WarrantyFilePath = $"{_configuration.GetValue<string>("AwsConfig:S3Folder")}/{i.ClientID}/warranty_files/{i.InventoryID}",
                    //                     Top = i.Top,
                    //                     Edge = i.Edge,
                    //                     Base  = i.Base, 
                    //                     Frame = i.Frame,
                    //                     Seat  = i.Seat,
                    //                     Back  = i.Back,
                    //                     SeatHeight =  i.SeatHeight.HasValue ? i.SeatHeight : 0,
                    //                     Tag = !string.IsNullOrEmpty(i.Tag) ? i.Tag : ""
                    //                     //hwd_str = (i.height.ToString() == "0.000" ? "" : i.height.ToString() + "h X ") + (i.width.ToString() == "0.000" ? "" : i.width.ToString() + "w X ") + (i.depth.ToString() == "0.000" ? "" : i.depth.ToString() + "d"),
                    //                     //hwd_str = (i.height.ToString() == "0.000" ? "" : Math.Round(Convert.ToDecimal(i.height), 1).ToString() + "h X ") + (i.width.ToString() == "0.000" ? "" : Math.Round(Convert.ToDecimal(i.width),1).ToString() + "w X ") + (i.depth.ToString() == "0.000" ? "" : Math.Round(Convert.ToDecimal(i.depth),1).ToString() + "d"),
                    //                     //Description = (!string.IsNullOrEmpty(i.AdditionalDescription) ? i.Description + "-" + i.AdditionalDescription : i.Description),
                    //                     //Qty = (!string.IsNullOrEmpty(bulding) && !string.IsNullOrEmpty(floor) && !string.IsNullOrEmpty(room) ? i.InventoryItems.Where(x => x.Building == bulding && x.Floor == floor && x.Room == room && x.Condition == cond).Count()
                    //                     //                                                                      : (!string.IsNullOrEmpty(bulding) ? i.InventoryItems.Where(x => x.Building == bulding).Count()
                    //                     //                                                                                                        : !string.IsNullOrEmpty(floor) ? i.InventoryItems.Where(x => x.Floor == floor).Count()
                    //                     //                                                                                                        : !string.IsNullOrEmpty(room) ? i.InventoryItems.Where(x => x.Room == room).Count()
                    //                     //                                                                                                        : !string.IsNullOrEmpty(cond) ? i.InventoryItems.Where(x => x.Condition == cond).Count()
                    //                     //                                                                                                                                       : i.InventoryItems.Count)),
                    //                     // //  qty = (!string.IsNullOrEmpty(floor) ? i.InventoryItems.GroupBy(x => new { x.floor }).Count() : i.InventoryItems.Count),

                    //                     //}).AsQueryable().OrderBy(ord => ord.InventoryID).Take(page);
                    //                 }).AsQueryable().OrderBy(ord => ord.InventoryID).Skip(startindex).Take(50);
                    //                // }).AsQueryable();

                    //var inventoryModels = await resultInv.ToListAsync();



                    var resultInv = await _dbContext.Inventories.Where(i => i.ItemTypeID == itemTypeId && i.ClientID == client_id).OrderBy(ord=>ord.InventoryID).Skip(startindex).Take(50).ToListAsync();

                    var inventoryModels = resultInv.Select(x => new InventoryModel()
                    {
                        InventoryID = x.InventoryID,
                        ItemCode = x.ItemCode.Contains("Unknown") ? x.ItemCode.Replace("Unknown", "") : x.ItemCode,
                        Description = x.Description,
                        //ManufacturerName = i.ManufacturerName.Contains("Unknown") ? i.ManufacturerName.Replace("Unknown", "") :i.ManufacturerName,
                        ManufacturerName = x.ManufacturerName,
                        Height = x.Height.HasValue ? x.Height : 0,
                        Width = x.Width.HasValue ? x.Width : 0,
                        Depth = x.Depth.HasValue ? x.Depth : 0,
                        //HWDStr = (x.Height < 0 || !x.Height.HasValue) ? "" : Decimal.Round(x.Height.Value, 1).ToString(),
                        HWDStr = (x.Height <= 0 || !x.Height.HasValue ? "" : Decimal.Round(x.Height.Value, 1).ToString() + "h") +
                                 (x.Width <= 0 || !x.Width.HasValue || !x.Height.HasValue ? "" : " X ") +
                                 (x.Width <= 0 || !x.Width.HasValue ? "" : Decimal.Round(x.Width.Value, 1).ToString() + "w") +
                                 ((x.Depth <= 0 || !x.Depth.HasValue) && (!x.Height.HasValue || !x.Width.HasValue || !x.Depth.HasValue) ? "" : " X ") +
                                 (x.Depth <= 0 || !x.Depth.HasValue ? "" : Decimal.Round(x.Depth.Value, 1).ToString() + "d") +
                                 ((x.Diameter > 0 || x.Diameter.HasValue) && (x.Height.HasValue || x.Width.HasValue || x.Depth.HasValue) ? " X " : "") +
                                 (x.Diameter <= 0 || !x.Diameter.HasValue ? "" : Decimal.Round(x.Diameter.Value, 1).ToString() + "dia"),
                        Fabric = x.Fabric.Trim() + (!string.IsNullOrEmpty(x.Fabric2) ? "," + x.Fabric2 : "").Trim(),
                        Finish = x.Finish.Trim() + (!string.IsNullOrEmpty(x.Finish2) ? "," + x.Finish2 : "").Trim(),
                        ClientID = x.ClientID,
                        Category = x.Category,
                        ImageName = x.MainImage,
                        ImageUrl = _configuration.GetValue<string>("ImgUrl") + x.ClientID + "/images/",
                        WarrantyYears = x.WarrantyYears,
                        BucketName = _configuration.GetValue<string>("AwsConfig:BuketName"),
                        ImagePath = $"{_configuration.GetValue<string>("AwsConfig:S3Folder")}/{x.ClientID}/images/{x.MainImage}",
                        FilePath = $"{_configuration.GetValue<string>("AwsConfig:S3Folder")}/{x.ClientID}/sustainability_files/{x.InventoryID}",
                        WarrantyFilePath = $"{_configuration.GetValue<string>("AwsConfig:S3Folder")}/{x.ClientID}/warranty_files/{x.InventoryID}",
                        Top = x.Top,
                        Edge = x.Edge,
                        Base = x.Base,
                        Frame = x.Frame,
                        Seat = x.Seat,
                        Back = x.Back,
                        SeatHeight = x.SeatHeight.HasValue ? x.SeatHeight : 0,
                        Tag = !string.IsNullOrEmpty(x.Tag) ? x.Tag : "",
                        PartNumber = x.PartNumber
                    }).ToList();

                    _logger.LogInfo($"Inventory Result Count=>{inventoryModels.Count}");
                    if (inventoryModels.Count > 0)
                    {
                       // var invModels = inventoryModels.GroupBy(x => x.InventoryID).Select(s => s.First()).Distinct().ToList();
                        var invModels = inventoryModels;

                        //invModels.RemoveAll(i => i.Qty == 0);

                        foreach (var item in invModels)
                        {
                            //if(item.ManufacturerName.Contains('(') && item.ManufacturerName.Contains(')'))
                            //{
                            //    int mnStart = item.ManufacturerName.IndexOf('(');
                            //    int mnEnd = item.ManufacturerName.IndexOf(')');

                            //    int length = mnEnd - mnStart + 1;
                            //    string extracted = item.ManufacturerName.Substring(mnStart, length);

                            //    string newManName = item.ManufacturerName.Replace(extracted,"");
                            //    item.ManufacturerName = newManName.Trim();
                            //}

                            //if (item.ItemCode.Contains('(') && item.ItemCode.Contains(')'))
                            //{
                            //    int icStart = item.ItemCode.IndexOf('(');
                            //    int icEnd = item.ItemCode.IndexOf(')');

                            //    int length = icEnd - icStart + 1;
                            //    string extracted = item.ItemCode.Substring(icStart, length);

                            //    string newitemCode = item.ItemCode.Replace(extracted, "");

                            //    item.ItemCode = newitemCode.Trim();
                            //}

                            ////var base64String = await getBase64(item.ImageName, item.ClientID);

                            ////if (!string.IsNullOrEmpty(base64String))
                            ////{
                            ////    base64String = "data:image/jpeg;base64," + base64String;
                            ////    invModels.Where(r => r.InventoryID == item.InventoryID).Select(s => { s.ImageBase64 = base64String; return s; }).ToList();
                            ////}

                            var inventoryItemModel = await _dbContext.InventoryItems.Where(ii => ii.InventoryID == item.InventoryID
                                  && (bulding > 0 ? ii.InventoryBuildingID.Equals(bulding) : ii.InventoryBuildingID >= bulding)
                                  && (floor > 0 ? ii.InventoryFloorID.Equals(floor) : ii.InventoryFloorID >= floor)
                                  && ii.Room.Contains((!string.IsNullOrEmpty(room) ? room : ""))
                                  && (cond > 0 ? ii.ConditionID.Equals(cond) : ii.ConditionID > cond) 
                                  && ii.AddedToCartItem == false
                                  && ii.DisplayOnSite == true
                              ).ToListAsync();

                            _logger.LogInfo($"InventoryItem Result Count=>{inventoryItemModel.Count}");

                            if (inventoryItemModel.Count > 0)
                            {
                                var availInventoryItemModel = inventoryItemModel.Where(ii => ii.StatusID == (int)Enums.Status.Active).ToList();
                                var resInventoryItemModel = inventoryItemModel.Where(ii => ii.StatusID == (int)Enums.Status.Reserved || 
                                                                                           ii.StatusID == (int)Enums.Status.WarrantyService ||
                                                                                           ii.StatusID == (int)Enums.Status.Maintenance 
                                                                                           ).ToList();

                                invModels.Where(i => i.InventoryID == item.InventoryID)
                                    .Select(s => {
                                        s.Qty = availInventoryItemModel.Count;
                                        s.ReservedQty = resInventoryItemModel.Count;
                                        return s;
                                    }).ToList();

                                //invModels.Where(i => i.InventoryID == item.InventoryID).Select(s => s.Qty = availInventoryItemModel.Count).ToList();
                                //invModels.Where(i => i.InventoryID == item.InventoryID).Select(s => s.ReservedQty = resInventoryItemModel.Count).ToList();
                               
                          //      adminInventoryModelList.Where(i => i.InventoryID == item.InventoryID)
                          //.Select(s => { s.TotalQty = totqty; s.ReservedQty = reservedqty; return s; }).ToList();

                                invModels.Where(i => i.InventoryID == item.InventoryID)
                               // .Select(s => s.InventoryItemModels = availInventoryItemModel.  // doing seprate call so commented this line and using below line
                                .Select(s => s.InventoryItemModels = inventoryItemModel.
                                     Select(x => new InventoryItemModel
                                     {
                                         InventoryID = x.InventoryID,
                                         InventoryItemID = x.InventoryItemID,
                                         //InventoryImageName = GetCartImages(x.InventoryItemID, x.InventoryID, x.ConditionID) ??  item.ImageName, // doing seprate call so commented this line and using below line
                                         InventoryImageName = item.ImageName,
                                         InventoryImageUrl = item.ImageUrl,
                                         //Building = "",
                                         Building = (x.InventoryBuildingID == 0 ? "" : _requestContext.ClientLocations.Where(ib => ib.location_id == x.InventoryBuildingID).FirstOrDefault().location_name),
                                         Floor = _dbContext.InventoryFloors.Where(f => f.InventoryFloorID == x.InventoryFloorID).FirstOrDefault().InventoryFloorName,
                                         //Floor = "",
                                         InventoryBuildingID = x.InventoryBuildingID,
                                         InventoryFloorID = x.InventoryFloorID,
                                         Room = x.Room,
                                         ConditionId = x.ConditionID,
                                         Condition = _dbContext.InventoryItemConditions.Where(c=>c.InventoryItemConditionID == x.ConditionID).FirstOrDefault().ConditionName,
                                         Qty = 1,
                                         ItemCode = item.ItemCode,
                                         Description = item.Description,
                                         ClientID = x.ClientID,
                                         StatusId = x.StatusID
                                     }).ToList()
                                 ).ToList();

                                //var gropinglocation = item.InventoryItemModels.GroupBy(x => new { x.Building, x.Floor, x.Room, x.Condition })
                                //                    .Select(g => new { g.Key.Building, g.Key.Floor, g.Key.Room, g.Key.Condition, qtyCount = g.Count() })
                                //                    .ToList();


                                #region grouping model
                                //var invItemModelDisplay = gropinglocation.Select(x => new InventoryItemModel
                                //{
                                //    InventoryID = item.InventoryID,
                                //    InventoryItemID = item.InventoryItemModels.Where(i => i.Building == x.Building && i.Floor == x.Floor && i.Room == x.Room && i.Condition == x.Condition).FirstOrDefault().InventoryItemID,
                                //    InventoryImageName = item.InventoryItemModels.Where(i => i.Building == x.Building && i.Floor == x.Floor && i.Room == x.Room && i.Condition == x.Condition).FirstOrDefault().InventoryImageName,
                                //    InventoryImageUrl = item.ImageUrl,
                                //    Building = x.Building,
                                //    Floor = x.Floor,
                                //    Room = x.Room,
                                //    Condition = x.Condition,
                                //    ConditionId = _dbContext.InventoryItemConditions.Where(c => c.ConditionName == x.Condition).FirstOrDefault().InventoryItemConditionID,
                                //    Qty = x.qtyCount,
                                //    ItemCode = item.ItemCode,
                                //    Description = item.Description,
                                //    ClientID = item.ClientID,
                                //    InventoryBuildingID = item.InventoryItemModels.Where(i => i.Building == x.Building && i.Floor == x.Floor && i.Room == x.Room && i.Condition == x.Condition).FirstOrDefault().InventoryBuildingID,
                                //    InventoryFloorID = item.InventoryItemModels.Where(i => i.Building == x.Building && i.Floor == x.Floor && i.Room == x.Room && i.Condition == x.Condition).FirstOrDefault().InventoryFloorID,
                                //    BucketName = _configuration.GetValue<string>("AwsConfig:BuketName"),
                                //    ImagePath = $"inventory/{item.ClientID}/{item.InventoryItemModels.Where(i => i.Building == x.Building && i.Floor == x.Floor && i.Room == x.Room && i.Condition == x.Condition).FirstOrDefault().InventoryImageName}"
                                //});

                                //invModels.Where(r => r.InventoryID == item.InventoryID)
                                //  .Select(r => { r.inventoryItemModelsDisplay = invItemModelDisplay.ToList(); return r; }).ToList();
                                #endregion

                                //StringBuilder stringBuilder = new StringBuilder();
                                //int index = 1;
                                //foreach (var grp in gropinglocation)
                                //{
                                //    int tmp_inv_item_id = item.InventoryItemModels.Where(i => i.Building == grp.Building && i.Floor == grp.Floor && i.Room == grp.Room && i.Condition == grp.Condition).FirstOrDefault().InventoryItemID;

                                //    if (gropinglocation.Count == index)
                                //        stringBuilder.Append($"{grp.Building} : {grp.Floor} : Loc {grp.Room} : Cond {grp.Condition} : Qty {grp.qtyCount}");
                                //    else
                                //    {
                                //        stringBuilder.Append($"{grp.Building} : {grp.Floor} : Loc {grp.Room} : Cond {grp.Condition} : Qty {grp.qtyCount} {Environment.NewLine}");
                                //    }

                                //    index++;
                                //}

                                //invModels.Where(r => r.InventoryID == item.InventoryID)
                                //   .Select(r => { r.TootipInventoryItem = stringBuilder.ToString(); return r; }).ToList();

                                //if (resInventoryItemModel.Count > 0)
                                //{
                                //    var gropingReservedLocation = resInventoryItemModel.GroupBy(x => new { x.InventoryBuildingID, x.InventoryFloorID, x.Room, x.ConditionID }).Select(g => new { g.Key.InventoryBuildingID, g.Key.InventoryFloorID, g.Key.Room, g.Key.ConditionID, qtyCount = g.Count() }).ToList();


                                //    StringBuilder sbReserved = new StringBuilder();
                                //    int indexRes = 1;
                                //    foreach (var grpRes in gropingReservedLocation)
                                //    {
                                //        //  int tmp_inv_item_id = item.InventoryItemModels.Where(i => i.Building == grpRes.Building && i.Floor == grpRes.Floor && i.Room == grpRes.Room && i.Condition == grpRes.Condition).FirstOrDefault().InventoryItemID;
                                //        string building = _requestContext.ClientLocations.Where(ib => ib.InventoryBuildingID == grpRes.InventoryBuildingID).FirstOrDefault().InventoryBuildingName;
                                //        string floorRes = _dbContext.InventoryFloors.Where(f => f.InventoryFloorID == grpRes.InventoryFloorID).FirstOrDefault().InventoryFloorName;
                                //        string condRes = _dbContext.InventoryItemConditions.Where(c => c.InventoryItemConditionID == grpRes.ConditionID).FirstOrDefault().ConditionName;

                                //        if (gropingReservedLocation.Count == indexRes)
                                //            sbReserved.Append($"{building} : {floorRes} : Loc {grpRes.Room} : Cond {condRes} : Qty {grpRes.qtyCount}");
                                //        else
                                //        {
                                //            sbReserved.Append($"{building} : {floorRes} : Loc {grpRes.Room} : Cond {condRes} : Qty {grpRes.qtyCount} {Environment.NewLine}");
                                //        }

                                //        indexRes++;
                                //    }

                                //    invModels.Where(r => r.InventoryID == item.InventoryID)
                                //       .Select(r => { r.TootipReservedInventoryItem = sbReserved.ToString(); return r; }).ToList();
                                //}

                                //invModels.Where(r => r.InventoryID == item.InventoryID)
                                //.Select(r => {
                                //    r.MissingParts = ((!string.IsNullOrEmpty(r.TootipInventoryItem) && r.TootipInventoryItem.ToLower().Contains("missing parts")) ||
                                //                        (!string.IsNullOrEmpty(r.TootipReservedInventoryItem) && r.TootipReservedInventoryItem.ToLower().Contains("missing parts"))) ? true : false;
                                //    return r;
                                //}).ToList();

                                invModels.Where(r=>r.InventoryID == item.InventoryID)
                                .Select(r =>
                                {
                                    r.MissingParts = (availInventoryItemModel.Any(itm=>itm.ConditionID == (int)Enums.Condition.MissingParts)) ? true : false;
                                    return r;
                                }).ToList();
                            }
                        }


                        
                         invModels.RemoveAll(i => i.Qty == 0 && i.ReservedQty == 0);
                         return await Task.Run(() => invModels.ToList()); 

                    }
                    else
                        return new List<InventoryModel>();

                    #region Oldcode

                    //var inventoryResult = result.Select(x => new InventoryModel
                    //{
                    //    inventory_id = x.inventory_id
                    //    ,
                    //    description = x.description.ToString()
                    //    ,
                    //    manuf = x.manuf
                    //    ,
                    //    height = x.height.ToString()
                    //    ,
                    //    width = x.width.ToString()
                    //    ,
                    //    depth = x.depth.ToString()
                    //    //,
                    //    //building = x.building
                    //    //,
                    //    //floor = x.floor
                    //    //,
                    //    //mploc = x.mploc
                    //    //,
                    //    //cond = x.cond
                    //    //,
                    //    //image_name = x.image_name
                    //    //,
                    //    //image_url = x.image_url
                    //    //,
                    //    //inv_item_id = x.inv_item_id
                    //    ,
                    //    fabric = x.fabric
                    //    ,
                    //    finish = x.finish
                    //}).ToList();

                    //var qtyresult = inventoryResult.GroupBy(x => x.inventory_id,
                    //                                 (key, values) => new
                    //                                 {
                    //                                     inventory_id = key,
                    //                                     qty = values.Count()
                    //                                 });

                    //var newinventoryResult = inventoryResult.GroupBy(dist => dist.inventory_id).FirstOrDefault().ToList();


                    //var invitemresult = (inventoryResult.GroupBy(x => x.inventory_id, x => new { x.building, x.floor, x.mploc, x.cond })
                    //                    .Select(g => new { inventory_id = g.Key, Values = g.ToList() })).ToList();

                    ////Select(x => { 
                    ////bulding = x.by                    });
                    //List<InventoryItemModel> inventoryItemModels = new List<InventoryItemModel>();

                    //foreach (var item in invitemresult)
                    //{
                    //    inventoryItemModels.Add(new InventoryItemModel()
                    //    {
                    //        inventory_id = item.inventory_id,
                    //        invItemDetailsModels = item.Values.Select(x => new InvItemDetailsModel()
                    //        {
                    //            building = x.building,
                    //            floor = x.floor,
                    //            mploc = x.mploc,
                    //            cond = x.cond
                    //        }).ToList()

                    //    });
                    //}



                    //// var newinventoryResult = inventoryResult.GroupBy(dist => dist.inventory_id).FirstOrDefault().ToList();
                    //var newinventoryResult = inventoryResult.GroupBy(dist => dist.inventory_id).Select(r => r.FirstOrDefault()).ToList();

                    //foreach (var qty in qtyresult)
                    //{
                    //    newinventoryResult.Where(r => r.inventory_id == qty.inventory_id).Select(r => { r.qty = qty.qty; return r; }).ToList();
                    //}



                    //foreach (var iim in inventoryItemModels)
                    //{
                    //    //newinventoryResult.Where(r => r.inventory_id == iim.inventory_id)
                    //    //    .Select(r => { r.InventoryItemModelsList = iim; return r; }).ToList();


                    //    var gropinglocation = iim.invItemDetailsModels.GroupBy(x => new { x.building, x.floor, x.mploc, x.cond }).Select(g => new { g.Key.building, g.Key.floor, g.Key.mploc, g.Key.cond, qtyCount = g.Count() });

                    //    var invDetailsModel = gropinglocation.Select(x => new InvItemDetailsModel { building = x.building, floor = x.floor, mploc = x.mploc, cond = x.cond, qty = x.qtyCount });

                    //    newinventoryResult.Where(r => r.inventory_id == iim.inventory_id)
                    //      .Select(r => { r.invItemDetailsModel = invDetailsModel.ToList(); return r; }).ToList();

                    //    StringBuilder stringBuilder = new StringBuilder();
                    //    int index = 1;
                    //    foreach (var grp in invDetailsModel)
                    //    {
                    //        if (invDetailsModel.Count() == index)
                    //            stringBuilder.Append($"{grp.building} : {grp.floor} : Loc {grp.mploc} : Cond {grp.cond} : Qty {grp.qty}");
                    //        else
                    //            stringBuilder.Append($"{grp.building} : {grp.floor} : Loc {grp.mploc} : Cond {grp.cond} : Qty {grp.qty} \n");

                    //        index++;
                    //        //  inv.building + ': ' + inv.floor + ': Loc ' + inv.mploc + ': Cond ' + inv.cond
                    //        // newinventoryResult.Where(r => r.inventory_id == iim.inventory_id)
                    //        //.Select(r => { r.invItemDetailsModel = invDetailsModel.ToList(); return r; }).ToList();
                    //    }
                    //    newinventoryResult.Where(r => r.inventory_id == iim.inventory_id)
                    //       .Select(r => { r.tootip_inv_item = stringBuilder.ToString(); return r; }).ToList();



                    //}


                    //return newinventoryResult;
                    #endregion

                }
                //return _dbContext.Inventories.Where(inv => inv.category.ToLower().Equals(category.ToLower())).ToList();
                else
                    return new List<InventoryModel>();

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<string> GetAvailTooltip(List<InventoryItemModel> model)
        {
            try
            {
                model = model.Where(m => m.StatusId == (int)Enums.Status.Active).ToList();

                var gropinglocation = model.GroupBy(x => new { x.Building, x.Floor, x.Room, x.Condition })
                                    .Select(g => new { g.Key.Building, g.Key.Floor, g.Key.Room, g.Key.Condition, qtyCount = g.Count() })
                                    .ToList();


                StringBuilder stringBuilder = new StringBuilder();
                int index = 1;
                foreach (var grp in gropinglocation)
                {
                   // int tmp_inv_item_id = model.Where(i => i.Building == grp.Building && i.Floor == grp.Floor && i.Room == grp.Room && i.Condition == grp.Condition).FirstOrDefault().InventoryItemID;

                    if (gropinglocation.Count == index)
                        stringBuilder.Append($"{grp.Building} : {grp.Floor} : Loc {grp.Room} : Cond {grp.Condition} : Qty {grp.qtyCount}");
                    else
                    {
                        stringBuilder.Append($"{grp.Building} : {grp.Floor} : Loc {grp.Room} : Cond {grp.Condition} : Qty {grp.qtyCount} {Environment.NewLine}");
                    }

                    index++;
                }

                //return await Task.Run(() => invModels.ToList());
                return await Task.Run(() => stringBuilder.ToString());      
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<string> GetResTooltip(List<InventoryItemModel> model)
        {
            try
            {
                StringBuilder stringBuilder = new StringBuilder();

                var resmodel = model.Where(m => m.StatusId == (int)Enums.Status.Reserved).ToList();

                if(resmodel != null && resmodel.Count > 0)
                {
                    List<InventoryOrderItemModel> orderItemModels = new List<InventoryOrderItemModel>();

                    foreach (var item in resmodel)
                    {
                        var ordItemEntity = _dbContext.OrderItems.Where(oi => oi.InventoryItemID == item.InventoryItemID
                                                                        && (oi.StatusID == (int)Enums.Status.OrdOpen ||
                                                                           oi.StatusID == (int)Enums.Status.OrdWorkinProgress
                                                                           )
                                                                        //&& oi.Completed == false
                                                                        //&& oi.Delivered == false
                                                                        ).FirstOrDefault();

                        if (ordItemEntity != null)
                        {
                            orderItemModels.Add(new InventoryOrderItemModel
                            {
                                InventoryID = ordItemEntity.InventoryID,
                                //DestBuilding = Common.GetBuilding(ordItemEntity.DestBuildingID),
                                //DestFloor = Common.GetFloor(ordItemEntity.DestFloorID),
                                DestBuilding = (ordItemEntity.DestBuildingID == 0 ? "" : _requestContext.ClientLocations.Where(cl => cl.location_id == ordItemEntity.DestBuildingID).FirstOrDefault().location_name),
                                DestFloor = (ordItemEntity.DestFloorID == 0 ? "" : _dbContext.InventoryFloors.Where(f => f.InventoryFloorID == ordItemEntity.DestFloorID).FirstOrDefault().InventoryFloorName),
                                DestRoom = ordItemEntity.DestRoom,
                                Qty = ordItemEntity.Qty
                            });
                        }

                    }

                    var gropinglocation = orderItemModels.GroupBy(x => new { x.DestBuilding, x.DestFloor, x.DestRoom })
                                        .Select(g => new { g.Key.DestBuilding, g.Key.DestFloor, g.Key.DestRoom, qtyCount = g.Count() })
                                        .ToList();

                    int index = 1;
                    foreach (var grp in gropinglocation)
                    {
                        // int tmp_inv_item_id = model.Where(i => i.Building == grp.Building && i.Floor == grp.Floor && i.Room == grp.Room && i.Condition == grp.Condition).FirstOrDefault().InventoryItemID;

                        if (gropinglocation.Count == index)
                            stringBuilder.Append($"{grp.DestBuilding} : {grp.DestFloor} : Loc {grp.DestRoom} : Qty {grp.qtyCount}");
                        else
                        {
                            stringBuilder.Append($"{grp.DestBuilding} : {grp.DestFloor} : Loc {grp.DestRoom} : Qty {grp.qtyCount} {Environment.NewLine}");
                        }

                        index++;
                    }
                }


                var warrantymodel = model.Where(m => m.StatusId == (int)Enums.Status.WarrantyService).ToList();

                if(warrantymodel != null && warrantymodel.Count > 0)
                {
                    if(stringBuilder.Length > 0)
                        stringBuilder.Append($"{Environment.NewLine} Warranty Service : Qty {warrantymodel.Count}");
                    else
                        stringBuilder.Append($"Warranty Service : Qty {warrantymodel.Count}");

                }

                var maintenancemodel = model.Where(m => m.StatusId == (int)Enums.Status.Maintenance).ToList();

                if (maintenancemodel != null && maintenancemodel.Count > 0)
                {
                    if(stringBuilder.Length > 0)
                        stringBuilder.Append($"{Environment.NewLine} Maintenance : Qty {maintenancemodel.Count}");                    
                    else
                        stringBuilder.Append($"Maintenance : Qty {warrantymodel.Count}");
                }



                return await Task.Run(() => stringBuilder.ToString());
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<InventoryItemModel>> GetCartList(InventoryModel model)
        {
            try
            {
                var inventoryItemModels = model.InventoryItemModels.Where(m => m.StatusId == (int)Enums.Status.Active).ToList();

                var gropinglocation = inventoryItemModels.GroupBy(x => new { x.Building, x.Floor, x.Room, x.Condition })
                                    .Select(g => new { g.Key.Building, g.Key.Floor, g.Key.Room, g.Key.Condition, qtyCount = g.Count() })
                                    .ToList();



                var invItemModelDisplay = gropinglocation.Select(x => new InventoryItemModel
                {
                    InventoryID = model.InventoryID,
                    InventoryItemID = inventoryItemModels.Where(i => i.Building == x.Building && i.Floor == x.Floor && i.Room == x.Room && i.Condition == x.Condition).FirstOrDefault().InventoryItemID,
                    //InventoryImageName = item.InventoryItemModels.Where(i => i.Building == x.Building && i.Floor == x.Floor && i.Room == x.Room && i.Condition == x.Condition).FirstOrDefault().InventoryImageName,
                    InventoryImageUrl = model.ImageUrl,
                    Building = x.Building,
                    Floor = x.Floor,
                    Room = x.Room,
                    Condition = x.Condition,
                    ConditionId = _dbContext.InventoryItemConditions.Where(c => c.ConditionName == x.Condition).FirstOrDefault().InventoryItemConditionID,
                    Qty = x.qtyCount,
                    ItemCode = model.ItemCode,
                    Description = model.Description,
                    ClientID = model.ClientID,
                    InventoryBuildingID = inventoryItemModels.Where(i => i.Building == x.Building && i.Floor == x.Floor && i.Room == x.Room && i.Condition == x.Condition).FirstOrDefault().InventoryBuildingID,
                    InventoryFloorID = inventoryItemModels.Where(i => i.Building == x.Building && i.Floor == x.Floor && i.Room == x.Room && i.Condition == x.Condition).FirstOrDefault().InventoryFloorID,
                    BucketName = _configuration.GetValue<string>("AwsConfig:BuketName"),
                    ImagePath = $"{_configuration.GetValue<string>("AwsConfig:S3Folder")}/{model.ClientID}/images/{inventoryItemModels.Where(i => i.Building == x.Building && i.Floor == x.Floor && i.Room == x.Room && i.Condition == x.Condition).FirstOrDefault().InventoryImageName}"
                });


                return await Task.Run(()=> invItemModelDisplay.ToList());

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //public List<InventoryModel> GetInventory1(string category, string bulding, string floor)
        //{

        //    try
        //    {
        //        List<Inventory> result = new List<Inventory>();

        //        if (!string.IsNullOrEmpty(category))
        //        {
        //            if ((!string.IsNullOrEmpty(bulding) && bulding != "undefined") && (!string.IsNullOrEmpty(floor) && floor != "undefined"))
        //            {
        //                result = _dbContext.Inventories.Where(i => i.Category == category).Include(ii => ii.InventoryItems.Where(x => x.Building == bulding && x.Floor == floor)).ThenInclude(iii => iii.InventoryItemImages.Take(1)).ToList();
        //            }
        //            else if (!string.IsNullOrEmpty(bulding) && bulding != "undefined")
        //            {
        //                result = _dbContext.Inventories.Where(i => i.Category == category).Include(ii => ii.InventoryItems.Where(x => x.Building == bulding)).ThenInclude(iii => iii.InventoryItemImages.Take(1)).ToList();
        //            }
        //            else if (!string.IsNullOrEmpty(floor) && floor != "undefined")
        //            {
        //                result = _dbContext.Inventories.Where(i => i.Category == category).Include(ii => ii.InventoryItems.Where(x => x.Floor == floor)).ThenInclude(iii => iii.InventoryItemImages.Take(1)).ToList();
        //            }
        //            else
        //            {
        //                result = _dbContext.Inventories.Where(i => i.Category == category).Include(ii => ii.InventoryItems).ThenInclude(iii => iii.InventoryItemImages.Take(1)).ToList();

        //            }
        //            // var result = _dbContext.Inventories.Where(i => i.category == category).Include(ii => ii.InventoryItems).ThenInclude(iii => iii.InventoryItemImages.Take(1));

        //            result = result.Where(x => x.InventoryItems.Count > 0).Select(s => { return s; }).ToList();

        //            if(result.Count > 0)
        //            {
        //                var inventoryResult = result.Select(x => new InventoryModel
        //                {
        //                    inventory_id = x.InventoryID,
        //                    item_code = x.ItemCode,
        //                    description = x.Description.ToString(),
        //                    manuf = x.Manuf,
        //                    height = x.Height.ToString(),
        //                    width = x.Width.ToString(),
        //                    depth = x.Depth.ToString(),
        //                    hwd_str = (x.Height.ToString() == "0.000" ? "" : x.Height.ToString() + "X") + (x.Width.ToString() == "0.000" ? "" : x.Width.ToString() + "X") + (x.Depth.ToString() == "0.000" ? "" : x.Depth.ToString()),
        //                    //building = x.building
        //                    //    ,
        //                    //floor = x.floor
        //                    //    ,
        //                    //mploc = x.mploc
        //                    //    ,
        //                    //cond = x.cond
        //                    //    ,
        //                    //image_name = x.image_name
        //                    //    ,
        //                    //image_url = x.image_url
        //                    //    ,
        //                    //inv_item_id = x.inv_item_id
        //                    // ,
        //                    fabric = x.Fabric,
        //                    finish = x.Finish,
        //                    qty = x.InventoryItems.Count,
        //                    //InventoryItemModels  = x.InventoryItems.GroupBy(x => new { x.building, x.floor, x.mploc, x.cond }).Select(g => new { g.Key.building, g.Key.floor, g.Key.mploc, g.Key.cond, qtyCount = g.Count() }).ToList();
        //                    InventoryItemModels = x.InventoryItems.Select(y => new InventoryItemModel
        //                    {
        //                        inventory_id = y.InventoryID,
        //                        inv_item_id = y.InventoryItemID,
        //                        building = y.Building,
        //                        floor = y.Floor,
        //                        mploc = y.Room,
        //                        cond = y.Condition
        //                    }).ToList(),
        //                    image_name = x.InventoryItems.Where(i => i.InventoryItemImages.Count > 0).FirstOrDefault().InventoryItemImages.Select(r => r.ImageName).FirstOrDefault()

        //                }).ToList();

        //                foreach (var item in inventoryResult)
        //                {
        //                    var gropinglocation = item.InventoryItemModels.GroupBy(x => new { x.building, x.floor, x.mploc, x.cond }).Select(g => new { g.Key.building, g.Key.floor, g.Key.mploc, g.Key.cond, qtyCount = g.Count() }).ToList();

        //                    var invItemModelDisplay = gropinglocation.Select(x => new InventoryItemModel
        //                    {
        //                        inventory_id = item.inventory_id
        //                        ,
        //                        inv_item_id = item.InventoryItemModels.FirstOrDefault().inv_item_id
        //                        ,
        //                        inv_image_name = item.image_name
        //                        ,
        //                        building = x.building
        //                        ,
        //                        floor = x.floor
        //                        ,
        //                        mploc = x.mploc
        //                        ,
        //                        cond = x.cond
        //                        ,
        //                        qty = x.qtyCount
        //                        ,
        //                        item_code = item.item_code
        //                        ,
        //                        description = item.description
        //                    });

        //                    inventoryResult.Where(r => r.inventory_id == item.inventory_id)
        //                      .Select(r => { r.inventoryItemModelsDisplay = invItemModelDisplay.ToList(); return r; }).ToList();

        //                    StringBuilder stringBuilder = new StringBuilder();
        //                    int index = 1;
        //                    foreach (var grp in gropinglocation)
        //                    {
        //                        //inventoryResult.Where(r => r.inventory_id == item.inventory_id).Select(x => new InventoryItemModel() {
        //                        //    inventory_id = item.inventory_id,
        //                        //    inv_item_id = item.InventoryItemModels.FirstOrDefault().inv_item_id,
        //                        //    inv_image_name = item.image_name,
        //                        //    building = grp.building,
        //                        //    floor = grp.floor,
        //                        //    mploc = grp.mploc,
        //                        //    cond = grp.cond,
        //                        //    qty = grp.qtyCount
        //                        //}).ToList();



        //                        if (gropinglocation.Count == index)
        //                            stringBuilder.Append($"{grp.building} : {grp.floor} : Loc {grp.mploc} : Cond {grp.cond} : Qty {grp.qtyCount}");
        //                        else
        //                            stringBuilder.Append($"{grp.building} : {grp.floor} : Loc {grp.mploc} : Cond {grp.cond} : Qty {grp.qtyCount} \n");

        //                        index++;
        //                    }

        //                    inventoryResult.Where(r => r.inventory_id == item.inventory_id)
        //                       .Select(r => { r.tootip_inv_item = stringBuilder.ToString(); return r; }).ToList();
        //                }

        //                return inventoryResult;
        //            }
        //            else
        //            {
        //                return new List<InventoryModel>();
        //            }



        //        }
        //        else
        //            return new List<InventoryModel>();

        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}

        public async Task<List<ItemTypesModel>> GetInventoryCategory(int client_id)
        {
            try
            {
                var itemTypes = await _dbContext.ItemTypes.Where(i => i.ClientID == client_id || i.ClientID == 1 && i.ItemTypeID != 0)
                                        .Select(item => new ItemTypesModel { ItemTypeID = item.ItemTypeID, ItemTypeName = item.ItemTypeName })
                                        .Distinct().ToListAsync();

                
                //result = _dbContext.Inventories.Where(i => i.Category == category).Include(ii => ii.InventoryItems.Where(x => x.Building == bulding && x.Floor == floor)).ThenInclude(iii => iii.InventoryItemImages.Take(1)).ToList();

                var invItemTypes = _dbContext.Inventories.Where(i => i.ClientID == client_id).Select(item => new { ItemTypeID = item.ItemTypeID }).Distinct().ToList();

                //invItemTypes.Where(i=>i.ItemTypeID )
                //HashSet<int> sentIDs = new HashSet<int>(SentList.Select(s => s.MsgID));

               // var results = MsgList.Where(m => !sentIDs.Contains(m.MsgID));

                var result = itemTypes.Where(it => invItemTypes.Any(x=>x.ItemTypeID == it.ItemTypeID)).ToList();

                return result.OrderBy(ord=>ord.ItemTypeName).ToList();

                //return await _dbContext.Inventories.Where(i=>!string.IsNullOrEmpty(i.Category) && i.ClientID == client_id).Select(cat=>cat.Category).Distinct().ToListAsync();
                //return await _dbContext.ItemTypes.Where(i => i.ClientID == client_id || i.ClientID == 1 && i.ItemTypeID != 0).Select(item => new ItemTypesModel {ItemTypeID = item.ItemTypeID,ItemTypeName = item.ItemTypeName }).Distinct().ToListAsync();

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<InventoryModel>> SearchInventory(string strSearch,int client_id, int bulding, int floor, string room, int cond)
        {

            try
            {
                if (!string.IsNullOrEmpty(strSearch))
                {
                    strSearch = strSearch.ToLower();

                    var resultInv = (from i in _dbContext.Inventories                                    
                                     where i.ClientID == client_id
                                     where (i.ItemCode.ToLower().Contains(strSearch) || i.Description.ToLower().Contains(strSearch)
                                            || i.PartNumber.ToLower().Contains(strSearch) || i.ManufacturerName.ToLower().Contains(strSearch)
                                            || i.Fabric.ToLower().Contains(strSearch) || i.Fabric2.ToLower().Contains(strSearch)
                                            || i.Finish.ToLower().Contains(strSearch) || i.Finish2.ToLower().Contains(strSearch)
                                            || i.Top.ToLower().Contains(strSearch) || i.Edge.ToLower().Contains(strSearch)
                                            || i.Base.ToLower().Contains(strSearch) || i.Frame.ToLower().Contains(strSearch)
                                            || i.Seat.ToLower().Contains(strSearch) || i.Back.ToLower().Contains(strSearch)
                                            || i.Modular.ToLower().Contains(strSearch) || i.Tag.ToLower().Contains(strSearch)
                                            )
                                     select new InventoryModel
                                     {
                                         InventoryID = i.InventoryID,
                                         ItemCode = i.ItemCode.Contains("Unknown") ? i.ItemCode.Replace("Unknown", "") : i.ItemCode,
                                         Description = i.Description,
                                         //ManufacturerName = i.ManufacturerName.Contains("Unknown") ? i.ManufacturerName.Replace("Unknown", "") :i.ManufacturerName,
                                         ManufacturerName = i.ManufacturerName,
                                         Height = i.Height,
                                         Width = i.Width,
                                         Depth = i.Depth,
                                         HWDStr = (i.Height.ToString() == "-1.000" ? "" : Decimal.Round(i.Height.Value, 1).ToString() + "h X ") +
                                                  // (i.Width.ToString() != "-1.0000" ? " X " : "") +
                                                  (i.Width.ToString() == "-1.000" ? "" : Decimal.Round(i.Width.Value, 1).ToString() + "w X ") +
                                                  // (i.Depth.ToString() != "-1.000" ? " X " : "") + 
                                                  (i.Depth.ToString() == "-1.000" ? "" : Decimal.Round(i.Depth.Value, 1).ToString() + "d") +
                                                  ((i.Depth.ToString() != "-1.000" && i.Diameter.ToString() != "-1.000") ? " X " : "") +
                                                  (i.Diameter.ToString() == "-1.000" ? "" : Decimal.Round(i.Diameter.Value, 1).ToString() + "dia"),
                                         Fabric = i.Fabric.Trim() + (!string.IsNullOrEmpty(i.Fabric2) ? "," + i.Fabric2 : "").Trim(),
                                         Finish = i.Finish.Trim() + (!string.IsNullOrEmpty(i.Finish2) ? "," + i.Finish2 : "").Trim(),
                                         ClientID = i.ClientID,
                                         Category = i.Category,
                                         ImageName = i.MainImage,
                                         ImageUrl = _configuration.GetValue<string>("ImgUrl") + i.ClientID + "/",
                                         WarrantyYears = i.WarrantyYears,
                                         BucketName = _configuration.GetValue<string>("AwsConfig:BuketName"),
                                         ImagePath = $"{_configuration.GetValue<string>("AwsConfig:S3Folder")}/{i.ClientID}/images/{i.MainImage}",
                                         FilePath = $"{_configuration.GetValue<string>("AwsConfig:S3Folder")}/{i.ClientID}/sustainability_files/{i.InventoryID}",
                                         WarrantyFilePath = $"{_configuration.GetValue<string>("AwsConfig:S3Folder")}/{i.ClientID}/warranty_files/{i.InventoryID}",
                                         Top = i.Top,
                                         Edge = i.Edge,
                                         Base = i.Base,
                                         Frame = i.Frame,
                                         Seat = i.Seat,
                                         Back = i.Back,
                                         SeatHeight = i.SeatHeight,
                                         Tag = !string.IsNullOrEmpty(i.Tag) ? i.Tag : ""
                                     }).AsQueryable();
                  

                    var inventoryModels = await resultInv.ToListAsync();
                    _logger.LogInfo($"Inventory Result Count=>{inventoryModels.Count}");

                    if (inventoryModels.Count > 0)
                    {
                        var invModels = inventoryModels;

                        foreach (var item in invModels)
                        {
                            var inventoryItemModel = await _dbContext.InventoryItems.Where(ii => ii.InventoryID == item.InventoryID
                                  && (bulding > 0 ? ii.InventoryBuildingID.Equals(bulding) : ii.InventoryBuildingID >= bulding)
                                  && (floor > 0 ? ii.InventoryFloorID.Equals(floor) : ii.InventoryFloorID >= floor)
                                  && ii.Room.Contains((!string.IsNullOrEmpty(room) ? room : ""))
                                  && (cond > 0 ? ii.ConditionID.Equals(cond) : ii.ConditionID > cond)
                                  && ii.AddedToCartItem == false
                                  && ii.DisplayOnSite == true
                              ).ToListAsync();

                            _logger.LogInfo($"InventoryItem Result Count=>{inventoryItemModel.Count}");

                            if (inventoryItemModel.Count > 0)
                            { 
                                var availInventoryItemModel = inventoryItemModel.Where(ii => ii.StatusID == (int)Enums.Status.Active).ToList();
                                var resInventoryItemModel = inventoryItemModel.Where(ii => ii.StatusID == (int)Enums.Status.Reserved).ToList();

                                //invModels.Where(i => i.InventoryID == item.InventoryID).Select(s => s.Qty = availInventoryItemModel.Count).ToList();
                                //invModels.Where(i => i.InventoryID == item.InventoryID).Select(s => s.ReservedQty = resInventoryItemModel.Count).ToList();
                                
                                invModels.Where(i => i.InventoryID == item.InventoryID)
                                   .Select(s => {
                                       s.Qty = availInventoryItemModel.Count;
                                       s.ReservedQty = resInventoryItemModel.Count;
                                       return s;
                                   }).ToList();


                                invModels.Where(i => i.InventoryID == item.InventoryID)
                                // .Select(s => s.InventoryItemModels = availInventoryItemModel.  // doing seprate call so commented this line and using below line
                                .Select(s => s.InventoryItemModels = inventoryItemModel.
                                     Select(x => new InventoryItemModel
                                     {
                                         InventoryID = x.InventoryID,
                                         InventoryItemID = x.InventoryItemID,
                                         InventoryImageName = item.ImageName,
                                         //InventoryImageName =  GetCartImages(x.InventoryItemID, x.InventoryID, x.ConditionID) ?? item.ImageName,
                                         InventoryImageUrl = item.ImageUrl,
                                         Building = (x.InventoryBuildingID == 0 ? "" :_requestContext.ClientLocations.Where(ib => ib.location_id == x.InventoryBuildingID).FirstOrDefault().location_name),
                                         Floor = _dbContext.InventoryFloors.Where(f => f.InventoryFloorID == x.InventoryFloorID).FirstOrDefault().InventoryFloorName,
                                         InventoryBuildingID = x.InventoryBuildingID,
                                         InventoryFloorID = x.InventoryFloorID,
                                         Room = x.Room,
                                         ConditionId = x.ConditionID,
                                         Condition = _dbContext.InventoryItemConditions.Where(c=>c.InventoryItemConditionID == x.ConditionID).FirstOrDefault().ConditionName,
                                         Qty = 1,
                                         ItemCode = item.ItemCode,
                                         Description = item.Description,
                                         ClientID = x.ClientID,                                         
                                         StatusId = x.StatusID
                                     }).ToList()
                                 ).ToList();


                                //var gropinglocation = item.InventoryItemModels.GroupBy(x => new { x.Building, x.Floor, x.Room, x.Condition })
                                //                        .Select(g => new { g.Key.Building, g.Key.Floor, g.Key.Room, 
                                //                                g.Key.Condition, qtyCount = g.Count() })
                                //                        .ToList();

                                #region grouping model
                                //var invItemModelDisplay = gropinglocation.Select(x => new InventoryItemModel
                                //{
                                //    InventoryID = item.InventoryID,
                                //    //inv_item_id = item.InventoryItemModels.FirstOrDefault().inv_item_id,
                                //    InventoryItemID = item.InventoryItemModels.Where(i => i.Building == x.Building && i.Floor == x.Floor && i.Room == x.Room && i.Condition == x.Condition).FirstOrDefault().InventoryItemID,
                                //    InventoryImageName = item.ImageName,
                                //    InventoryImageUrl = item.ImageUrl,
                                //    Building = x.Building,
                                //    Floor = x.Floor,
                                //    Room = x.Room,
                                //    Condition = x.Condition,
                                //    Qty = x.qtyCount,
                                //    ItemCode = item.ItemCode,
                                //    Description = item.Description,
                                //    ClientID = item.ClientID,
                                //    InventoryBuildingID = item.InventoryItemModels.Where(i => i.Building == x.Building && i.Floor == x.Floor && i.Room == x.Room && i.Condition == x.Condition).FirstOrDefault().InventoryBuildingID,
                                //    InventoryFloorID = item.InventoryItemModels.Where(i => i.Building == x.Building && i.Floor == x.Floor && i.Room == x.Room && i.Condition == x.Condition).FirstOrDefault().InventoryFloorID,

                                //});

                                //invModels.Where(r => r.InventoryID == item.InventoryID)
                                //  .Select(r => { r.inventoryItemModelsDisplay = invItemModelDisplay.ToList(); return r; }).ToList();
                                #endregion

                                //StringBuilder stringBuilder = new StringBuilder();
                                //int index = 1;
                                //foreach (var grp in gropinglocation)
                                //{
                                //    int tmp_inv_item_id = item.InventoryItemModels.Where(i => i.Building == grp.Building && i.Floor == grp.Floor && i.Room == grp.Room && i.Condition == grp.Condition).FirstOrDefault().InventoryItemID;

                                //    if (gropinglocation.Count == index)
                                //        stringBuilder.Append($"{grp.Building} : {grp.Floor} : Loc {grp.Room} : Cond {grp.Condition} : Qty {grp.qtyCount}");
                                //    else
                                //    {
                                //        stringBuilder.Append($"{grp.Building} : {grp.Floor} : Loc {grp.Room} : Cond {grp.Condition} : Qty {grp.qtyCount} {Environment.NewLine}");
                                //    }

                                //    index++;
                                //}

                                //invModels.Where(r => r.InventoryID == item.InventoryID)
                                //   .Select(r => { r.TootipInventoryItem = stringBuilder.ToString(); return r; }).ToList();

                                //if (resInventoryItemModel.Count > 0)
                                //{
                                //    var gropingReservedLocation = resInventoryItemModel.GroupBy(x => new { x.InventoryBuildingID, x.InventoryFloorID, x.Room, x.ConditionID }).Select(g => new { g.Key.InventoryBuildingID, g.Key.InventoryFloorID, g.Key.Room, g.Key.ConditionID, qtyCount = g.Count() }).ToList();


                                //    StringBuilder sbReserved = new StringBuilder();
                                //    int indexRes = 1;
                                //    foreach (var grpRes in gropingReservedLocation)
                                //    {
                                //        //  int tmp_inv_item_id = item.InventoryItemModels.Where(i => i.Building == grpRes.Building && i.Floor == grpRes.Floor && i.Room == grpRes.Room && i.Condition == grpRes.Condition).FirstOrDefault().InventoryItemID;
                                //        string building = _requestContext.ClientLocations.Where(ib => ib.InventoryBuildingID == grpRes.InventoryBuildingID).FirstOrDefault().InventoryBuildingName;
                                //        string floorRes = _dbContext.InventoryFloors.Where(f => f.InventoryFloorID == grpRes.InventoryFloorID).FirstOrDefault().InventoryFloorName;
                                //        string condRes = _dbContext.InventoryItemConditions.Where(c => c.InventoryItemConditionID == grpRes.ConditionID).FirstOrDefault().ConditionName;

                                //        if (gropingReservedLocation.Count == indexRes)
                                //            sbReserved.Append($"{building} : {floorRes} : Loc {grpRes.Room} : Cond {condRes} : Qty {grpRes.qtyCount}");
                                //        else
                                //        {
                                //            sbReserved.Append($"{building} : {floorRes} : Loc {grpRes.Room} : Cond {condRes} : Qty {grpRes.qtyCount} {Environment.NewLine}");
                                //        }

                                //        indexRes++;
                                //    }

                                //    invModels.Where(r => r.InventoryID == item.InventoryID)
                                //       .Select(r => { r.TootipReservedInventoryItem = sbReserved.ToString(); return r; }).ToList();
                                //}

                                //invModels.Where(r => r.InventoryID == item.InventoryID)
                                //.Select(r => {
                                //    r.MissingParts = ((!string.IsNullOrEmpty(r.TootipInventoryItem) && r.TootipInventoryItem.ToLower().Contains("missing parts")) ||
                                //                        (!string.IsNullOrEmpty(r.TootipReservedInventoryItem) && r.TootipReservedInventoryItem.ToLower().Contains("missing parts"))) ? true : false;
                                //    return r;
                                //}).ToList();

                                invModels.Where(r => r.InventoryID == item.InventoryID)
                                .Select(r =>
                                {
                                    r.MissingParts = (availInventoryItemModel.Any(itm => itm.ConditionID == (int)Enums.Condition.MissingParts)) ? true : false;
                                    return r;
                                }).ToList();
                            }
                        }

                        //await Task.Run(() => manager.GetList());

                        //return invModels;
                        //invModels.RemoveAll(i => i.Qty == 0);                      
                        invModels.RemoveAll(i => i.Qty == 0 && i.ReservedQty == 0);
                        return await Task.Run(() => invModels.ToList());
                        //return await Task.Run(() => inventories);

                    }
                    else
                        return new List<InventoryModel>();
                }
                else
                return new List<InventoryModel>();


            #region old code
            //List<Inventory> result = new List<Inventory>();

            //if (!string.IsNullOrEmpty(strSearch))
            //{

            //    var inventoryModels = (from i in _dbContext.Inventories
            //                           join ii in _dbContext.InventoryItems on i.InventoryID equals ii.InventoryID into invitem
            //                           from ii in invitem.DefaultIfEmpty()
            //                           where i.ClientID == client_id
            //                           //where ii.InventoryBuildingID.Equals((bulding > 0 ? bulding : 0))
            //                           //where ii.InventoryFloorID.Equals((floor > 0 ? floor : 0))
            //                           where (bulding >= 0 ? ii.InventoryBuildingID.Equals(bulding) : ii.InventoryBuildingID > bulding)
            //                           where (floor >= 0 ? ii.InventoryFloorID.Equals(floor) : ii.InventoryFloorID > floor)
            //                           where ii.Room.Contains((!string.IsNullOrEmpty(room) ? room : ""))
            //                           where ii.Condition.Contains((!string.IsNullOrEmpty(cond) ? cond : ""))
            //                           where ii.AddedToCartItem == false
            //                           where ii.DisplayOnSite == true
            //                           //where ii.Building != "Donate" // need to workout for the donation type of building
            //                           //where (i.ItemCode.Contains(strSearch) || i.Description.Contains(strSearch) || i.AdditionalDescription.Contains(strSearch))
            //                           where (i.ItemCode.Contains(strSearch) || i.Description.Contains(strSearch))
            //                           select new InventoryModel()
            //                           {
            //                               InventoryID = i.InventoryID,
            //                               ItemCode = i.ItemCode,
            //                               //Description = (!string.IsNullOrEmpty(i.AdditionalDescription) ? i.Description + "-" + i.AdditionalDescription : i.Description),
            //                               Description = i.Description,
            //                               ManufacturerName = i.ManufacturerName,
            //                               Height = i.Height.ToString(),
            //                               Width = i.Width.ToString(),
            //                               Depth = i.Depth.ToString(),
            //                               HWDStr = (i.Height.ToString() == "0.000" ? "" : Decimal.Round(i.Height, 1).ToString() + "h X ") + (i.Width.ToString() == "0.000" ? "" : Decimal.Round(i.Width, 1).ToString() + "w X ") + (i.Depth.ToString() == "0.000" ? "" : Decimal.Round(i.Depth, 1).ToString() + "d"),
            //                               Fabric = i.Fabric,
            //                               Finish = i.Finish,
            //                               ClientID = i.ClientID,
            //                               Category = i.Category,
            //                               Qty = (bulding >0 && floor > 0 && !string.IsNullOrEmpty(room) ? i.InventoryItems.Where(x => x.InventoryBuildingID == bulding && x.InventoryFloorID == floor && x.Room == room && x.Condition == cond).Count()
            //                                                                                                     : (bulding > 0 ? i.InventoryItems.Where(x => x.InventoryBuildingID == bulding).Count()
            //                                                                                                                                       : (floor > 0) ? i.InventoryItems.Where(x => x.InventoryFloorID == floor).Count()
            //                                                                                                                                       : !string.IsNullOrEmpty(room) ? i.InventoryItems.Where(x => x.Room == room).Count()
            //                                                                                                                                       : !string.IsNullOrEmpty(cond) ? i.InventoryItems.Where(x => x.Condition == cond).Count()
            //                                                                                                                                                                      : i.InventoryItems.Count)),

            //                               InventoryItemModels = (from ii in _dbContext.InventoryItems
            //                                                      where ii.InventoryID == i.InventoryID
            //                                                      //where ii.InventoryBuildingID.Equals((bulding > 0 ? bulding : 0))
            //                                                      //where ii.InventoryFloorID.Equals((floor > 0 ? floor : 0))
            //                                                      where (bulding >= 0 ? ii.InventoryBuildingID.Equals(bulding) : ii.InventoryBuildingID > bulding)
            //                                                      where (floor >= 0 ? ii.InventoryFloorID.Equals(floor) : ii.InventoryFloorID > floor)
            //                                                      where ii.Room.Contains((!string.IsNullOrEmpty(room) ? room : ""))
            //                                                      where ii.Condition.Contains((!string.IsNullOrEmpty(cond) ? cond : ""))
            //                                                      where ii.AddedToCartItem == false
            //                                                      where ii.DisplayOnSite == true
            //                                                      //where ii.Building != "Donate"
            //                                                      select new InventoryItemModel
            //                                                      {
            //                                                          InventoryID = ii.InventoryID,
            //                                                          InventoryItemID = ii.InventoryItemID,
            //                                                          Building = _requestContext.ClientLocations.Where(ib => ib.InventoryBuildingID == ii.InventoryBuildingID).FirstOrDefault().InventoryBuildingName,
            //                                                          Floor = _dbContext.InventoryFloors.Where(f => f.InventoryFloorID == ii.InventoryFloorID).FirstOrDefault().InventoryFloorName,
            //                                                          Room = ii.Room,
            //                                                          Condition = ii.Condition,
            //                                                          ClientID = ii.ClientID,
            //                                                          InventoryBuildingID = ii.InventoryBuildingID,
            //                                                          InventoryFloorID = ii.InventoryFloorID
            //                                                      }).ToList(),

            //                               ImageName = i.InventoryItems.Where(i => i.InventoryItemImages.Count > 0).FirstOrDefault().InventoryItemImages.Select(r => r.ImageName).FirstOrDefault(),
            //                               ImageUrl = i.InventoryItems.Where(i => i.InventoryItemImages.Count > 0).FirstOrDefault().InventoryItemImages.Select(r => r.ImageUrl).FirstOrDefault()
            //                               //  qty = (!string.IsNullOrEmpty(floor) ? i.InventoryItems.GroupBy(x => new { x.floor }).Count() : i.InventoryItems.Count),

            //                           }).AsQueryable().ToList();

            //    if (inventoryModels.Count > 0)
            //    {

            //        var invModels = inventoryModels.GroupBy(x => x.InventoryID).Select(s => s.First()).Distinct().ToList();

            //        invModels.RemoveAll(i => i.Qty == 0);

            //        foreach (var item in invModels)
            //        {
            //            var gropinglocation = item.InventoryItemModels.GroupBy(x => new { x.Building, x.Floor, x.Room, x.Condition }).Select(g => new { g.Key.Building, g.Key.Floor, g.Key.Room, g.Key.Condition, qtyCount = g.Count() }).ToList();

            //            var invItemModelDisplay = gropinglocation.Select(x => new InventoryItemModel
            //            {
            //                InventoryID = item.InventoryID,
            //                //inv_item_id = item.InventoryItemModels.FirstOrDefault().inv_item_id,
            //                InventoryItemID = item.InventoryItemModels.Where(i => i.Building == x.Building && i.Floor == x.Floor && i.Room == x.Room && i.Condition == x.Condition).FirstOrDefault().InventoryItemID,
            //                InventoryImageName = item.ImageName,
            //                InventoryImageUrl = item.ImageUrl,
            //                Building = x.Building,
            //                Floor = x.Floor,
            //                Room = x.Room,
            //                Condition = x.Condition,
            //                Qty = x.qtyCount,
            //                ItemCode = item.ItemCode,
            //                Description = item.Description,
            //                ClientID = item.ClientID
            //            });

            //            invModels.Where(r => r.InventoryID == item.InventoryID)
            //              .Select(r => { r.inventoryItemModelsDisplay = invItemModelDisplay.ToList(); return r; }).ToList();

            //            StringBuilder stringBuilder = new StringBuilder();
            //            int index = 1;
            //            foreach (var grp in gropinglocation)
            //            {
            //                int tmp_inv_item_id = item.InventoryItemModels.Where(i => i.Building == grp.Building && i.Floor == grp.Floor && i.Room == grp.Room && i.Condition == grp.Condition).FirstOrDefault().InventoryItemID;

            //                if (gropinglocation.Count == index)
            //                    stringBuilder.Append($"{grp.Building} : {grp.Floor} : Loc {grp.Room} : Cond {grp.Condition} : Qty {grp.qtyCount}");
            //                else                                            
            //                    stringBuilder.Append($"{grp.Building} : {grp.Floor} : Loc {grp.Room} : Cond {grp.Condition} : Qty {grp.qtyCount} \n");

            //                index++;
            //            }

            //            invModels.Where(r => r.InventoryID == item.InventoryID)
            //               .Select(r => { r.TootipInventoryItem = stringBuilder.ToString(); return r; }).ToList();
            //        }

            //        return await Task.Run(() => invModels.ToList());

            //    }
            //    else
            //        return new List<InventoryModel>();



            //}
            //else
            //    return new List<InventoryModel>();

            #endregion

        }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<InventoryItemsTypesModel> GetSpecificInventoryItemsTypes(int clientid)
        {
            try
            {
                var queryResultSpaceType = (from st in _dbContext.SpaceTypes
                                           join it in _dbContext.InventoryItems on st.SpaceTypeID equals it.InventorySpaceTypeID
                                           where st.ClientID == clientid
                                           where it.InventorySpaceTypeID != 0
                                           select new SpaceTypesModel()
                                           {
                                               ClientID = st.ClientID,
                                               SpaceTypeID = st.SpaceTypeID,
                                               SpaceTypeName = st.SpaceTypeName
                                           }).AsQueryable().Distinct();

                var queryResultOwnership = (from own in _dbContext.InventoryOwners
                                            join it in _dbContext.InventoryItems on own.InventoryOwnerID equals it.InventoryOwnerID
                                            where own.ClientID == clientid
                                            where it.InventoryOwnerID != 0
                                            select new InventoryOwnersModel()
                                            {
                                                ClientID = own.ClientID,
                                                InventoryOwnerID = own.InventoryOwnerID,
                                                OwnerName = own.OwnerName
                                            }).AsQueryable().Distinct();

                // InventoryItemsTypesModel

                //return await queryResultSpaceType.ToListAsync();

                InventoryItemsTypesModel inventoryItemsTypesModel = new InventoryItemsTypesModel()
                {
                    SpaceTypesModels = await queryResultSpaceType.ToListAsync(),
                    InventoryOwnersModels = await queryResultOwnership.ToListAsync()
                };

                return inventoryItemsTypesModel;

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<InventoryModel>> GetItemTypesInventoy(int spacetypeid, int ownershipid, int clientid, int buildingid, int floorid, string room, int condition)
        {
            var resultInv = (from i in _dbContext.Inventories
                             join ii in _dbContext.InventoryItems on i.InventoryID equals ii.InventoryID //into invitem
                             //from ii in invitem.DefaultIfEmpty()
                             //where i.ItemTypeID == itemTypeId
                             where i.ClientID == clientid
                             where ((spacetypeid > 0 ? ii.InventorySpaceTypeID.Equals(spacetypeid) : ii.InventorySpaceTypeID > spacetypeid)
                                    || (ownershipid > 0 ? ii.InventoryOwnerID.Equals(ownershipid) : ii.InventoryOwnerID > ownershipid))
                             where ii.AddedToCartItem == false
                             where ii.DisplayOnSite == true
                             select new InventoryModel
                             {
                                 InventoryID = i.InventoryID,
                                 ItemCode = i.ItemCode.Contains("Unknown") ? i.ItemCode.Replace("Unknown", "") : i.ItemCode,
                                 Description = i.Description,
                                 //ManufacturerName = i.ManufacturerName.Contains("Unknown") ? i.ManufacturerName.Replace("Unknown", "") :i.ManufacturerName,
                                 ManufacturerName = i.ManufacturerName,
                                 Height = i.Height,
                                 Width = i.Width,
                                 Depth = i.Depth,
                                 HWDStr = (i.Height.ToString() == "-1.000" ? "" : Decimal.Round(i.Height.Value, 1).ToString() + "h X ") + 
                                         // (i.Width.ToString() != "-1.0000" ? " X " : "") +
                                          (i.Width.ToString() == "-1.000" ? "" : Decimal.Round(i.Width.Value, 1).ToString() + "w X ") + 
                                         // (i.Depth.ToString() != "-1.000" ? " X " : "") + 
                                          (i.Depth.ToString() == "-1.000" ? "" : Decimal.Round(i.Depth.Value, 1).ToString() + "d") +
                                          ((i.Depth.ToString() != "-1.000" && i.Diameter.ToString() != "-1.000") ? " X " : "") + 
                                          (i.Diameter.ToString() == "-1.000" ? "" : Decimal.Round(i.Diameter.Value, 1).ToString() + "dia"),

                                 Fabric = i.Fabric.Trim() + (!string.IsNullOrEmpty(i.Fabric2) ? "," + i.Fabric2 : "").Trim(),
                                 Finish = i.Finish.Trim() + (!string.IsNullOrEmpty(i.Finish2) ? "," + i.Finish2 : "").Trim(),
                                 ClientID = i.ClientID,
                                 Category = i.Category,
                                 ImageName = i.MainImage,
                                 ImageUrl = _configuration.GetValue<string>("ImgUrl") + i.ClientID + "/",
                                 WarrantyYears = i.WarrantyYears,
                                 BucketName = _configuration.GetValue<string>("AwsConfig:BuketName"),
                                 ImagePath = $"{_configuration.GetValue<string>("AwsConfig:S3Folder")}/{i.ClientID}/images/{i.MainImage}",
                                 FilePath = $"{_configuration.GetValue<string>("AwsConfig:S3Folder")}/{i.ClientID}/sustainability_files/{i.InventoryID}",
                                 WarrantyFilePath = $"{_configuration.GetValue<string>("AwsConfig:S3Folder")}/{i.ClientID}/warranty_files/{i.InventoryID}",
                                 Top = i.Top,
                                 Edge = i.Edge,
                                 Base = i.Base,
                                 Frame = i.Frame,
                                 Seat = i.Seat,
                                 Back = i.Back,
                                 SeatHeight = i.SeatHeight,
                                 Tag = !string.IsNullOrEmpty(i.Tag) ? i.Tag : ""
                                                               
                             }).Distinct().AsQueryable();


            var inventoryModels = await resultInv.ToListAsync();

            _logger.LogInfo($"Inventory Result Count=>{inventoryModels.Count}");
            if (inventoryModels.Count > 0)
            {
                var invModels = inventoryModels;
                foreach (var item in invModels)
                {
                    var inventoryItemModel = await _dbContext.InventoryItems.Where(ii => ii.InventoryID == item.InventoryID
                          && (buildingid > 0 ? ii.InventoryBuildingID.Equals(buildingid) : ii.InventoryBuildingID >= buildingid)
                          && (floorid > 0 ? ii.InventoryFloorID.Equals(floorid) : ii.InventoryFloorID >= floorid)
                          && ii.Room.Contains((!string.IsNullOrEmpty(room) ? room : ""))                          
                          && (condition > 0 ? ii.ConditionID.Equals(condition) : ii.ConditionID > condition)
                      // && ii.AddedToCartItem == false
                      //&& ii.DisplayOnSite == true
                      //&& (spacetypeid > 0 ? ii.InventorySpaceTypeID.Equals(spacetypeid) : ii.InventorySpaceTypeID > spacetypeid)
                      //&& (ownershipid > 0 ? ii.InventoryOwnerID.Equals(ownershipid) : ii.InventoryOwnerID > ownershipid)
                      ).ToListAsync();

                    _logger.LogInfo($"InventoryItem Result Count=>{inventoryItemModel.Count}");

                    if (inventoryItemModel.Count > 0)
                    {
                        var availInventoryItemModel = inventoryItemModel.Where(ii => ii.StatusID == (int)Enums.Status.Active).ToList();
                        var resInventoryItemModel = inventoryItemModel.Where(ii => ii.StatusID == (int)Enums.Status.Reserved).ToList();

                        //invModels.Where(i => i.InventoryID == item.InventoryID).Select(s => s.Qty = availInventoryItemModel.Count).ToList();
                        //invModels.Where(i => i.InventoryID == item.InventoryID).Select(s => s.ReservedQty = resInventoryItemModel.Count).ToList();
                       
                        invModels.Where(i => i.InventoryID == item.InventoryID)
                                  .Select(s => {
                                      s.Qty = availInventoryItemModel.Count;
                                      s.ReservedQty = resInventoryItemModel.Count;
                                      return s;
                                  }).ToList();

                        invModels.Where(i => i.InventoryID == item.InventoryID)
                                 // .Select(s => s.InventoryItemModels = availInventoryItemModel.  // doing seprate call so commented this line and using below line
                                 .Select(s => s.InventoryItemModels = inventoryItemModel.
                                      Select(x => new InventoryItemModel
                                      {
                                          InventoryID = x.InventoryID,
                                          InventoryItemID = x.InventoryItemID,
                                         //InventoryImageName = GetCartImages(x.InventoryItemID, x.InventoryID, x.ConditionID) ??  item.ImageName, // doing seprate call so commented this line and using below line
                                         InventoryImageName = item.ImageName,
                                          InventoryImageUrl = item.ImageUrl,
                                          Building = (x.InventoryBuildingID == 0 ? "" : _requestContext.ClientLocations.Where(ib => ib.location_id == x.InventoryBuildingID).FirstOrDefault().location_name),
                                          Floor = _dbContext.InventoryFloors.Where(f => f.InventoryFloorID == x.InventoryFloorID).FirstOrDefault().InventoryFloorName,
                                          InventoryBuildingID = x.InventoryBuildingID,
                                          InventoryFloorID = x.InventoryFloorID,
                                          Room = x.Room,
                                          ConditionId = x.ConditionID,
                                          Condition = _dbContext.InventoryItemConditions.Where(c => c.InventoryItemConditionID == x.ConditionID).FirstOrDefault().ConditionName,
                                          Qty = 1,
                                          ItemCode = item.ItemCode,
                                          Description = item.Description,
                                          ClientID = x.ClientID,
                                          StatusId = x.StatusID
                                      }).ToList()
                                  ).ToList();


                        //var gropinglocation = item.InventoryItemModels.GroupBy(x => new { x.Building, x.Floor, x.Room, x.Condition }).Select(g => new { g.Key.Building, g.Key.Floor, g.Key.Room, g.Key.Condition, qtyCount = g.Count() }).ToList();

                        #region due to new architecture not required this commented code
                        //var invItemModelDisplay = gropinglocation.Select(x => new InventoryItemModel
                        //{
                        //    InventoryID = item.InventoryID,                          
                        //    InventoryItemID = item.InventoryItemModels.Where(i => i.Building == x.Building && i.Floor == x.Floor && i.Room == x.Room && i.Condition == x.Condition).FirstOrDefault().InventoryItemID,
                        //    InventoryImageName = item.ImageName,
                        //    InventoryImageUrl = item.ImageUrl,
                        //    Building = x.Building,
                        //    Floor = x.Floor,
                        //    Room = x.Room,
                        //    Condition = x.Condition,
                        //    Qty = x.qtyCount,
                        //    ItemCode = item.ItemCode,
                        //    Description = item.Description,
                        //    ClientID = item.ClientID,
                        //    InventoryBuildingID = item.InventoryItemModels.Where(i => i.Building == x.Building && i.Floor == x.Floor && i.Room == x.Room && i.Condition == x.Condition).FirstOrDefault().InventoryBuildingID,
                        //    InventoryFloorID = item.InventoryItemModels.Where(i => i.Building == x.Building && i.Floor == x.Floor && i.Room == x.Room && i.Condition == x.Condition).FirstOrDefault().InventoryFloorID,

                        //});

                        //invModels.Where(r => r.InventoryID == item.InventoryID)
                        //  .Select(r => { r.inventoryItemModelsDisplay = invItemModelDisplay.ToList(); return r; }).ToList();
                        #endregion

                        //StringBuilder stringBuilder = new StringBuilder();
                        //int index = 1;
                        //foreach (var grp in gropinglocation)
                        //{
                        //    int tmp_inv_item_id = item.InventoryItemModels.Where(i => i.Building == grp.Building && i.Floor == grp.Floor && i.Room == grp.Room && i.Condition == grp.Condition).FirstOrDefault().InventoryItemID;

                        //    if (gropinglocation.Count == index)
                        //        stringBuilder.Append($"{grp.Building} : {grp.Floor} : Loc {grp.Room} : Cond {grp.Condition} : Qty {grp.qtyCount}");
                        //    else
                        //    {
                        //        stringBuilder.Append($"{grp.Building} : {grp.Floor} : Loc {grp.Room} : Cond {grp.Condition} : Qty {grp.qtyCount} {Environment.NewLine}");
                        //    }

                        //    index++;
                        //}

                        //invModels.Where(r => r.InventoryID == item.InventoryID)
                        //   .Select(r => { r.TootipInventoryItem = stringBuilder.ToString(); return r; }).ToList();

                        //if (resInventoryItemModel.Count > 0)
                        //{
                        //    var gropingReservedLocation = resInventoryItemModel.GroupBy(x => new { x.InventoryBuildingID, x.InventoryFloorID, x.Room, x.ConditionID })
                        //        .Select(g => new { g.Key.InventoryBuildingID, g.Key.InventoryFloorID, g.Key.Room, 
                        //            g.Key.ConditionID, qtyCount = g.Count() })
                        //        .ToList();


                        //    StringBuilder sbReserved = new StringBuilder();
                        //    int indexRes = 1;
                        //    foreach (var grpRes in gropingReservedLocation)
                        //    {
                        //        //  int tmp_inv_item_id = item.InventoryItemModels.Where(i => i.Building == grpRes.Building && i.Floor == grpRes.Floor && i.Room == grpRes.Room && i.Condition == grpRes.Condition).FirstOrDefault().InventoryItemID;
                        //        string building = _requestContext.ClientLocations.Where(ib => ib.InventoryBuildingID == grpRes.InventoryBuildingID).FirstOrDefault().InventoryBuildingName;
                        //        string floorRes = _dbContext.InventoryFloors.Where(f => f.InventoryFloorID == grpRes.InventoryFloorID).FirstOrDefault().InventoryFloorName;
                        //        string condRes = _dbContext.InventoryItemConditions.Where(c => c.InventoryItemConditionID == grpRes.ConditionID).FirstOrDefault().ConditionName;

                        //        if (gropingReservedLocation.Count == indexRes)
                        //            sbReserved.Append($"{building} : {floorRes} : Loc {grpRes.Room} : Cond {condRes} : Qty {grpRes.qtyCount}");
                        //        else
                        //        {
                        //            sbReserved.Append($"{building} : {floorRes} : Loc {grpRes.Room} : Cond {condRes} : Qty {grpRes.qtyCount} {Environment.NewLine}");
                        //        }

                        //        indexRes++;
                        //    }

                        //    invModels.Where(r => r.InventoryID == item.InventoryID)
                        //       .Select(r => {r.TootipReservedInventoryItem = sbReserved.ToString(); return r; }).ToList();
                        //}

                        //invModels.Where(r => r.InventoryID == item.InventoryID)
                        //     .Select(r => {                                
                        //         r.MissingParts = ((!string.IsNullOrEmpty(r.TootipInventoryItem) && r.TootipInventoryItem.ToLower().Contains("missing parts")) ||
                        //                           (!string.IsNullOrEmpty(r.TootipReservedInventoryItem) && r.TootipReservedInventoryItem.ToLower().Contains("missing parts"))) ? true : false;
                        //         return r;
                        //     }).ToList();

                        invModels.Where(r => r.InventoryID == item.InventoryID)
                               .Select(r =>
                               {
                                   r.MissingParts = (availInventoryItemModel.Any(itm => itm.ConditionID == (int)Enums.Condition.MissingParts)) ? true : false;
                                   return r;
                               }).ToList();
                    }
                }

               
                //invModels.RemoveAll(i => i.Qty == 0);
                invModels.RemoveAll(i => i.Qty == 0 && i.ReservedQty == 0);
                //return await Task.Run(() => invModels.ToList());
                return invModels.ToList();
               

            }
            else
                return new List<InventoryModel>();
        }

        private string GetCartImages(int inventoryitemid,int inventoryid,int conditionid)
        {
            string imagename;
            var entity = _dbContext.InventoryImages.FirstOrDefault(invimg => invimg.InventoryItemID == inventoryitemid && invimg.ConditionID == conditionid);
            
            if(entity != null)
            {
                imagename = entity.ImageName;
            }
            else
            {
                var inventoryImagesEntity = _dbContext.InventoryImages.FirstOrDefault(invimg => invimg.InventoryID == inventoryid && invimg.ConditionID == conditionid);

                if(inventoryImagesEntity == null)
                {
                     inventoryImagesEntity = _dbContext.InventoryImages.FirstOrDefault(invimg => invimg.InventoryID == inventoryid && invimg.InventoryItemID == null);
                }
                imagename = (inventoryImagesEntity != null ? inventoryImagesEntity.ImageName : "");
            }
            return imagename;
            //return await Task.Run(() => imagename);
        }


        //private async Task<string> getBase64(string url, int clientid)
        //{
        //    var bytes = await _aws3Repository.DownloadFileAsync(url,clientid); //http url image convert to byte so, base on need do uncomment and comment
        //    var base64String = Convert.ToBase64String(bytes);
        //    return base64String;
        //}


    }
}
