using InventoryApi.DBContext;
using InventoryApi.DBModels.InventoryDBModels;
using InventoryApi.DBModels.SSIDBModels;
using InventoryApi.Helpers;
using InventoryApi.Models;
using InventoryApi.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static InventoryApi.Helpers.Enums;

namespace InventoryApi.Repository
{
    public class AdminInventoryRepository : IAdminInventoryRepository
    {
        InventoryContext _dbContext;
        SSIRequestContext _requestContext;

        IConfiguration _configuration { get; }
        IAws3Repository _aws3Repository { get; }
        IAwsDownloadRepository _awsDownloadRepository { get; }
        IAwsUploadRepository _awsUploadRepository { get; }
        public AdminInventoryRepository(InventoryContext dbContext
                                       , IConfiguration configuration
                                       , IAws3Repository aws3Repository
                                       , IAwsDownloadRepository awsDownloadRepository
                                       , IAwsUploadRepository awsUploadRepository
                                       , SSIRequestContext requestContext
            )
        {
            _dbContext = dbContext;
            _configuration = configuration;
            _aws3Repository = aws3Repository;
            _awsDownloadRepository = awsDownloadRepository;
            _awsUploadRepository = awsUploadRepository;
            Common._dbContext = dbContext;
            _requestContext = requestContext;

        }


        public async Task<AdminInventoryModel> GetAdminInventory(int clientId, int currentPage, int perPageRows, int startIndex)
        {
            AdminInventoryModel adminInventoryModel = new AdminInventoryModel();

            try
            {
                var totalRows = _dbContext.Inventories.Where(i => i.ClientID == clientId).Count();
                var totalPages = (int)Math.Ceiling((decimal)totalRows / perPageRows);

                adminInventoryModel.Count = totalRows;
                adminInventoryModel.TotalPages = totalPages;
                adminInventoryModel.PageNo = currentPage;
                adminInventoryModel.RowsPerPage = perPageRows;

                var inventory = (from i in _dbContext.Inventories
                                 where i.ClientID == clientId
                                 //where ii.Building != "Donate"
                                 select new AdminInventory
                                 {
                                     InventoryID = i.InventoryID,
                                     ClientID = i.ClientID,
                                     ItemTypeID = i.ItemTypeID,
                                     ItemRowID = i.ItemRowID,
                                     ItemCode = i.ItemCode,
                                     GlobalProductCatalogID = i.GlobalProductCatalogID,
                                     Category = _dbContext.ItemTypes.Where(it => it.ItemTypeID == i.ItemTypeID).FirstOrDefault().ItemTypeName,
                                     Description = i.Description,
                                     ManufacturerID = i.ManufacturerID,
                                     ManufacturerName = i.ManufacturerName,
                                     Fabric = i.Fabric,
                                     Fabric2 = i.Fabric2,
                                     Finish = i.Finish,
                                     Finish2 = i.Finish2,
                                     Size = i.Size,
                                     RFIDCode = i.RFIDCode,
                                     BarCode = i.BarCode,
                                     QRCode = i.QRCode,
                                     PartNumber = i.PartNumber,
                                     Height = i.Height,
                                     Width = i.Width,
                                     Depth = i.Depth,
                                     Diameter = i.Diameter,
                                     Top = i.Top,
                                     Edge = i.Edge,
                                     Base = i.Base,
                                     Frame = i.Frame,
                                     Seat = i.Seat,
                                     Back = i.Back,
                                     SeatHeight = i.SeatHeight,
                                     Modular = i.Modular,
                                     ImageName = i.MainImage,
                                     WarrantyYears = i.WarrantyYears,
                                     Tag = i.Tag,
                                     Unit = i.Unit,
                                     ImageUrl = _configuration.GetValue<string>("ImgUrl") + i.ClientID + "/",
                                     ImageSrc = _configuration.GetValue<string>("ImgUrl") + i.ClientID + "/images/" + i.MainImage,
                                     //Files = new List<string>(),
                                     BucketName = _configuration.GetValue<string>("AwsConfig:BuketName"),
                                     ImagePath = $"{_configuration.GetValue<string>("AwsConfig:S3Folder")}/{i.ClientID}/images/{i.MainImage}",
                                     FilePath = $"{_configuration.GetValue<string>("AwsConfig:S3Folder")}/{i.ClientID}/sustainability_files/{i.InventoryID}"
                                     // }).AsQueryable().Take(100);
                                     //}).AsQueryable().OrderByDescending(ord=>ord.InventoryID).Skip(currentPage).Take(perPageRows);
                                 //}).AsQueryable().OrderBy(ord => ord.Category).Skip(startIndex).Take(perPageRows);
                                 }).AsQueryable().OrderByDescending(ord => ord.InventoryID).Skip(startIndex).Take(perPageRows);

                var adminInventoryModelList = await inventory.ToListAsync();



                if (adminInventoryModelList.Count > 0)
                {

                    foreach (var item in adminInventoryModelList)
                    {
                        //string path = !string.IsNullOrEmpty(item.ImageName) ? $"inventory/{item.ClientID}/{item.ImageName}" : "";
                        ////var imageExist = !string.IsNullOrEmpty(item.ImageName) ? await _aws3Repository.IsFileExists(item.ImageName, item.ClientID, "") : false;
                        //var imageExist = !string.IsNullOrEmpty(path) ? await _aws3Repository.IsFileExists("", path) : false;

                        //string docfilepath = $"inventory/{item.ClientID}/fimg/{item.InventoryID}";            

                        ////var files = await _aws3Repository.ListS3FileNames(item.ClientID, item.InventoryID);
                        //var files = await _aws3Repository.ListS3FileNames("",docfilepath);

                        //files.Remove("");

                        //adminInventoryModelList.Where(img => img.InventoryID == item.InventoryID).Select(s => s.ImageExist = imageExist).ToList();
                        //adminInventoryModelList.Where(img => img.InventoryID == item.InventoryID).Select(s => s.Files = files).ToList();

                        //new implementation to get img data
                        //string path = !string.IsNullOrEmpty(item.ImageName) ? $"inventory/{item.ClientID}/{item.ImageName}" : "";
                        //var imgBase64 = await _awsDownloadRepository.DownloadFileAsync(_configuration.GetValue<string>("AwsConfig:BuketName"), path);

                        //adminInventoryModelList.Where(img => img.InventoryID == item.InventoryID).Select(s => s.ImageBase64 = imgBase64).ToList();

                        var invitementity = _dbContext.InventoryItems.Where(ii => ii.InventoryID == item.InventoryID);
                        int totqty = invitementity.Count();

                        int reservedqty = invitementity.Where(ii => ii.StatusID == (int)Enums.Status.Reserved).Count();

                        //adminInventoryModelList.Where(i => i.InventoryID == item.InventoryID)
                        //    .Select(s => new AdminInventory() { TotalQty = totqty, ReservedQty = reservedqty }).ToList();

                        adminInventoryModelList.Where(i => i.InventoryID == item.InventoryID)
                            .Select(s => { s.TotalQty = totqty; s.ReservedQty = reservedqty; return s; }).ToList();

                        ////int totqty = _dbContext.InventoryItems.Where(ii => ii.InventoryID == item.InventoryID && ii.Building != "Donate").Count();
                        // int totqty = _dbContext.InventoryItems.Where(ii => ii.InventoryID == item.InventoryID).Count();
                        // adminInventoryModelList.Where(i => i.InventoryID == item.InventoryID).Select(s => s.TotalQty = totqty).ToList();

                        //// int reservedqty = _dbContext.InventoryItems.Where(ii => ii.InventoryID == item.InventoryID && ii.Building != "Donate" && ii.StatusID == (int)Enums.Status.Reserved).Count();
                        // int reservedqty = _dbContext.InventoryItems.Where(ii => ii.InventoryID == item.InventoryID && ii.StatusID == (int)Enums.Status.Reserved).Count();
                        // adminInventoryModelList.Where(i => i.InventoryID == item.InventoryID).Select(s => s.ReservedQty = reservedqty).ToList();

                        //var inventoryItemModel = await _dbContext.InventoryItems
                        //                                .Where(ii => ii.InventoryID == item.InventoryID
                        //                                        && ii.Building != "Donate")
                        //                                .ToListAsync();

                        //if (inventoryItemModel.Count > 0)
                        //{
                        //    //adminInventoryModelList.Where(i => i.InventoryID == item.InventoryID).Select(s => s.Qty = inventoryItemModel.Count).ToList();

                        //    var gropinglocation = inventoryItemModel.GroupBy(x => new { x.Building, x.Floor, x.Room, x.Condition }).Select(g => new { g.Key.Building, g.Key.Floor, g.Key.Room, g.Key.Condition, qtyCount = g.Count() }).ToList();

                        //    var inventoryItemModelList = gropinglocation.Select(x => new InventoryItemModel
                        //    {
                        //        InventoryID = item.InventoryID,
                        //        InventoryItemID = inventoryItemModel.Where(i => i.Building == x.Building && i.Floor == x.Floor && i.Room == x.Room && i.Condition == x.Condition).FirstOrDefault().InventoryItemID,
                        //        Building = x.Building,
                        //        Floor = x.Floor,
                        //        Room = x.Room,
                        //        Condition = x.Condition,
                        //        Qty = x.qtyCount,
                        //        ClientID = item.ClientID
                        //    }).ToList();

                        //    adminInventoryModelList.Where(ai => ai.InventoryID == item.InventoryID).Select(s => s.InventoryItemModels = inventoryItemModelList).ToList();
                        //}
                    }
                }

                //adminInventoryModelList = await inventory.ToListAsync();

                adminInventoryModel.AdminInventories = adminInventoryModelList;

            }
            catch (Exception ex)
            {
                throw;
            }

            return adminInventoryModel;

        }

        public async Task<List<InventoryItemModel>> GetAdminInventoryItem(int clientId, int inventoryId, int buildingid, int floorid, string room)
        {
            List<InventoryItemModel> inventoryItemModelList = new List<InventoryItemModel>();
            string mainImage = _dbContext.Inventories.Where(i => i.InventoryID == inventoryId).FirstOrDefault().MainImage;

            try
            {
                var tmpinventoryItemModelList = _dbContext.InventoryItems
                                                .Where(ii => ii.InventoryID == inventoryId && ii.ClientID == clientId
                                                        && (buildingid >= 0 ? ii.InventoryBuildingID.Equals(buildingid) : ii.InventoryBuildingID > buildingid)
                                                        && (floorid >= 0 ? ii.InventoryFloorID.Equals(floorid) : ii.InventoryFloorID > floorid)
                                                        && ii.Room.Contains((!string.IsNullOrEmpty(room) ? room : ""))
                                                       )
                                                //&& ii.Building != "Donate")
                                                .AsQueryable();

                var inventoryItemModel = await tmpinventoryItemModelList.ToListAsync();

                //inventoryItemModelList = inventoryItemModel.Select(x => new InventoryItemModel
                //{
                //    InventoryID = x.InventoryID,
                //    InventoryItemID = x.InventoryItemID,
                //    Building = _requestContext.ClientLocations.Where(ib => ib.InventoryBuildingID == x.InventoryBuildingID).FirstOrDefault().InventoryBuildingName,
                //    Floor = _dbContext.InventoryFloors.Where(f => f.InventoryFloorID == x.InventoryFloorID).FirstOrDefault().InventoryFloorName,
                //    Room = x.Room,
                //    ConditionId = x.ConditionID,
                //    Condition = _dbContext.InventoryItemConditions.Where(c => c.InventoryItemConditionID == x.ConditionID).FirstOrDefault().ConditionName,
                //    Qty = 1,
                //    ClientID = clientId,
                //    StatusId = x.StatusID,
                //    StatusName = (x.StatusID == (int)Enums.Status.Active ? "Available" : (x.StatusID == (int)Enums.Status.Reserved ? Enums.Status.Reserved.ToString() :"")),
                //    InventoryImageUrl = _configuration.GetValue<string>("ImgUrl") + x.ClientID + "/",                   
                //    BucketName = _configuration.GetValue<string>("AwsConfig:BuketName")                    
                //}).ToList();

                //inventoryItemModelList.ForEach(ch =>
                //{
                //    var inventoryImageName = Common.GetCartImages(ch.InventoryItemID, ch.InventoryID, ch.ConditionId) ?? mainImage;
                //    ch.InventoryImageName = inventoryImageName;
                //    ch.ImagePath = $"inventory/{ch.ClientID}/{inventoryImageName}";
                //});

                if (inventoryItemModel.Count > 0)
                {
                    var gropinglocation = inventoryItemModel.GroupBy(x => new { x.InventoryBuildingID, x.InventoryFloorID, x.Room, x.ConditionID })
                        .Select(g => new { g.Key.InventoryBuildingID, g.Key.InventoryFloorID, g.Key.Room, g.Key.ConditionID, qtyCount = g.Count() }).ToList();

                    inventoryItemModelList = gropinglocation.Select(x => new InventoryItemModel
                    {
                        InventoryID = inventoryId,
                        InventoryItemID = inventoryItemModel.Where(i => i.InventoryBuildingID == x.InventoryBuildingID && i.InventoryFloorID == x.InventoryFloorID && i.Room == x.Room && i.ConditionID == x.ConditionID).FirstOrDefault().InventoryItemID,
                        Building = (x.InventoryBuildingID == 0 ? "" : _requestContext.ClientLocations.Where(ib => ib.location_id == x.InventoryBuildingID).FirstOrDefault().location_name),
                        InventoryBuildingID = x.InventoryBuildingID,
                        Floor = _dbContext.InventoryFloors.Where(f => f.InventoryFloorID == x.InventoryFloorID).FirstOrDefault().InventoryFloorName,
                        InventoryFloorID = x.InventoryFloorID,
                        Room = x.Room,
                        ConditionId = x.ConditionID,
                        Condition = _dbContext.InventoryItemConditions.Where(c => c.InventoryItemConditionID == x.ConditionID).FirstOrDefault().ConditionName,
                        Qty = x.qtyCount,
                        ClientID = clientId,
                        StatusId = inventoryItemModel.Where(i => i.InventoryBuildingID == x.InventoryBuildingID && i.InventoryFloorID == x.InventoryFloorID && i.Room == x.Room && i.ConditionID == x.ConditionID).FirstOrDefault().StatusID,
                        //StatusName = (x.StatusID == (int)Enums.Status.Active ? "Available" : (x.StatusID == (int)Enums.Status.Reserved ? Enums.Status.Reserved.ToString() : "")),
                        InventoryImageUrl = _configuration.GetValue<string>("ImgUrl") + clientId + "/",
                        BucketName = _configuration.GetValue<string>("AwsConfig:BuketName"),
                        InventorySpaceTypeId = inventoryItemModel.Where(i => i.InventoryBuildingID == x.InventoryBuildingID && i.InventoryFloorID == x.InventoryFloorID && i.Room == x.Room && i.ConditionID == x.ConditionID).FirstOrDefault().InventorySpaceTypeID,
                        InventoryOwnerId = inventoryItemModel.Where(i => i.InventoryBuildingID == x.InventoryBuildingID && i.InventoryFloorID == x.InventoryFloorID && i.Room == x.Room && i.ConditionID == x.ConditionID).FirstOrDefault().InventoryOwnerID,
                        ProposalNumber = inventoryItemModel.Where(i => i.InventoryBuildingID == x.InventoryBuildingID && i.InventoryFloorID == x.InventoryFloorID && i.Room == x.Room && i.ConditionID == x.ConditionID).FirstOrDefault().ProposalNumber,
                        PoOrderNo = inventoryItemModel.Where(i => i.InventoryBuildingID == x.InventoryBuildingID && i.InventoryFloorID == x.InventoryFloorID && i.Room == x.Room && i.ConditionID == x.ConditionID).FirstOrDefault().PoOrderNo.ToString()
                    }).ToList();

                    inventoryItemModelList.ForEach(ch =>
                    {
                        var inventoryImageName = Common.GetCartImages(ch.InventoryItemID, ch.InventoryID, ch.ConditionId) ?? mainImage;
                        ch.InventoryImageName = inventoryImageName;
                        ch.ImagePath = $"{_configuration.GetValue<string>("AwsConfig:S3Folder")}/{ch.ClientID}/images/{inventoryImageName}";
                        ch.StatusName = (ch.StatusId == (int)Enums.Status.Active ? "Available" : (ch.StatusId == (int)Enums.Status.Reserved ? Enums.Status.Reserved.ToString() : ""));
                        ch.InventorySpaceType = _dbContext.SpaceTypes.Where(st => st.SpaceTypeID == ch.InventorySpaceTypeId).FirstOrDefault().SpaceTypeName;
                        ch.InventoryOwner = _dbContext.InventoryOwners.Where(own => own.InventoryOwnerID == ch.InventoryOwnerId).FirstOrDefault().OwnerName;

                    });
                }

            }
            catch (Exception ex)
            {
                throw;
            }

            return inventoryItemModelList;

        }

        public async Task<AdminInventoryModel> SearchAdminInventory(int clientId, int currentPage, int perPageRows, int startIndex,string search)
        {
            AdminInventoryModel adminInventoryModel = new AdminInventoryModel();

            try
            {
                //if (!string.IsNullOrEmpty(search))
                //{

                    var totalRows = _dbContext.Inventories
                                .Where (i => i.ClientID == clientId &&
                                        (
                                            i.ItemCode.ToLower().Contains(search) || i.Description.ToLower().Contains(search)
                                            || i.PartNumber.ToLower().Contains(search) || i.ManufacturerName.ToLower().Contains(search)
                                            || i.Fabric.ToLower().Contains(search) || i.Fabric2.ToLower().Contains(search)
                                            || i.Finish.ToLower().Contains(search) || i.Finish2.ToLower().Contains(search)
                                            || i.Top.ToLower().Contains(search) || i.Edge.ToLower().Contains(search)
                                            || i.Base.ToLower().Contains(search) || i.Frame.ToLower().Contains(search)
                                            || i.Seat.ToLower().Contains(search) || i.Back.ToLower().Contains(search)
                                            || i.Modular.ToLower().Contains(search) || i.Tag.ToLower().Contains(search)
                                        )
                                        )
                                .Count();
                    var totalPages = (int)Math.Ceiling((decimal)totalRows / perPageRows);

                    adminInventoryModel.Count = totalRows;
                    adminInventoryModel.TotalPages = totalPages;
                    adminInventoryModel.PageNo = currentPage;
                    adminInventoryModel.RowsPerPage = perPageRows;

                    var inventory = (from i in _dbContext.Inventories
                                     where (i.ClientID == clientId &&
                                      (
                                        i.ItemCode.ToLower().Contains(search) || i.Description.ToLower().Contains(search)
                                        || i.PartNumber.ToLower().Contains(search) || i.ManufacturerName.ToLower().Contains(search)
                                        || i.Fabric.ToLower().Contains(search) || i.Fabric2.ToLower().Contains(search)
                                        || i.Finish.ToLower().Contains(search) || i.Finish2.ToLower().Contains(search)
                                        || i.Top.ToLower().Contains(search) || i.Edge.ToLower().Contains(search)
                                        || i.Base.ToLower().Contains(search) || i.Frame.ToLower().Contains(search)
                                        || i.Seat.ToLower().Contains(search) || i.Back.ToLower().Contains(search)
                                        || i.Modular.ToLower().Contains(search) || i.Tag.ToLower().Contains(search)

                                      )
                                     )
                                      //where ii.Building != "Donate"
                                     select new AdminInventory
                                     {
                                         InventoryID = i.InventoryID,
                                         ClientID = i.ClientID,
                                         ItemTypeID = i.ItemTypeID,
                                         ItemRowID = i.ItemRowID,
                                         ItemCode = i.ItemCode,
                                         GlobalProductCatalogID = i.GlobalProductCatalogID,
                                         Category = _dbContext.ItemTypes.Where(it => it.ItemTypeID == i.ItemTypeID).FirstOrDefault().ItemTypeName,
                                         Description = i.Description,
                                         ManufacturerID = i.ManufacturerID,
                                         ManufacturerName = i.ManufacturerName,
                                         Fabric = i.Fabric,
                                         Fabric2 = i.Fabric2,
                                         Finish = i.Finish,
                                         Finish2 = i.Finish2,
                                         Size = i.Size,
                                         RFIDCode = i.RFIDCode,
                                         BarCode = i.BarCode,
                                         QRCode = i.QRCode,
                                         PartNumber = i.PartNumber,
                                         Height = i.Height,
                                         Width = i.Width,
                                         Depth = i.Depth,
                                         Diameter = i.Diameter,
                                         Top = i.Top,
                                         Edge = i.Edge,
                                         Base = i.Base,
                                         Frame = i.Frame,
                                         Seat = i.Seat,
                                         Back = i.Back,
                                         SeatHeight = i.SeatHeight,
                                         Modular = i.Modular,
                                         ImageName = i.MainImage,
                                         WarrantyYears = i.WarrantyYears,
                                         Tag = i.Tag,
                                         Unit = i.Unit,
                                         ImageUrl = _configuration.GetValue<string>("ImgUrl") + i.ClientID + "/",
                                         ImageSrc = _configuration.GetValue<string>("ImgUrl") + i.ClientID + "/images/" + i.MainImage,
                                         //Files = new List<string>(),
                                         BucketName = _configuration.GetValue<string>("AwsConfig:BuketName"),
                                         ImagePath = $"{_configuration.GetValue<string>("AwsConfig:S3Folder")}/{i.ClientID}/images/{i.MainImage}",
                                         FilePath = $"{_configuration.GetValue<string>("AwsConfig:S3Folder")}/{i.ClientID}/sustainability_files/{i.InventoryID}"
                                         // }).AsQueryable().Take(100);
                                         //}).AsQueryable().OrderByDescending(ord=>ord.InventoryID).Skip(currentPage).Take(perPageRows);
                                         //}).AsQueryable().OrderBy(ord => ord.Category).Skip(startIndex).Take(perPageRows);
                                     }).AsQueryable().OrderByDescending(ord => ord.InventoryID).Skip(startIndex).Take(perPageRows);

                    var adminInventoryModelList = await inventory.ToListAsync();



                    if (adminInventoryModelList.Count > 0)
                    {

                        foreach (var item in adminInventoryModelList)
                        {
                            var invitementity = _dbContext.InventoryItems.Where(ii => ii.InventoryID == item.InventoryID);
                            int totqty = invitementity.Count();

                            int reservedqty = invitementity.Where(ii => ii.StatusID == (int)Enums.Status.Reserved).Count();


                            adminInventoryModelList.Where(i => i.InventoryID == item.InventoryID)
                                .Select(s => { s.TotalQty = totqty; s.ReservedQty = reservedqty; return s; }).ToList();


                        }
                    }


                    adminInventoryModel.AdminInventories = adminInventoryModelList;
               // }
            }
            catch (Exception ex)
            {
                throw;
            }

            return adminInventoryModel;

        }


        public async Task<List<InventoryItemModel>> GetAdminChildInventoryItem(int inventoryid, int buildingid, int floorid, string room, int conditionid)
        {
            List<InventoryItemModel> inventoryItemModelList = new List<InventoryItemModel>();
           // string mainImage = _dbContext.Inventories.Where(i => i.InventoryID == inventoryId).FirstOrDefault().MainImage;

            try
            {
                var tmpinventoryItemModelList = _dbContext.InventoryItems
                                                .Where(ii => ii.InventoryID == inventoryid && 
                                                        ii.InventoryBuildingID == buildingid &&
                                                        ii.InventoryFloorID == floorid &&
                                                        ii.Room.Contains(room) &&
                                                        ii.ConditionID == conditionid
                                                        )
                                                //&& ii.Building != "Donate")
                                                .AsQueryable();

                var inventoryItemModel = await tmpinventoryItemModelList.ToListAsync();

                inventoryItemModelList = inventoryItemModel.Select(x => new InventoryItemModel
                {
                    InventoryID = x.InventoryID,
                    InventoryItemID = x.InventoryItemID,
                    Building = (x.InventoryBuildingID == 0 ? "" : _requestContext.ClientLocations.Where(ib => ib.location_id == x.InventoryBuildingID).FirstOrDefault().location_name),
                    Floor = _dbContext.InventoryFloors.Where(f => f.InventoryFloorID == x.InventoryFloorID).FirstOrDefault().InventoryFloorName,
                    Room = x.Room,
                    ConditionId = x.ConditionID,
                    Condition = _dbContext.InventoryItemConditions.Where(c => c.InventoryItemConditionID == x.ConditionID).FirstOrDefault().ConditionName,
                    Qty = 1,
                    ClientID = x.ClientID,
                    StatusId = x.StatusID,
                    StatusName = (x.StatusID == (int)Enums.Status.Active ? "Available" : (x.StatusID == (int)Enums.Status.Reserved ? Enums.Status.Reserved.ToString() : "")),
                    InventoryImageUrl = _configuration.GetValue<string>("ImgUrl") + x.ClientID + "/",
                    BucketName = _configuration.GetValue<string>("AwsConfig:BuketName"),
                    InventorySpaceTypeId = inventoryItemModel.Where(i => i.InventoryBuildingID == x.InventoryBuildingID && i.InventoryFloorID == x.InventoryFloorID && i.Room == x.Room && i.ConditionID == x.ConditionID).FirstOrDefault().InventorySpaceTypeID,
                    InventoryOwnerId = inventoryItemModel.Where(i => i.InventoryBuildingID == x.InventoryBuildingID && i.InventoryFloorID == x.InventoryFloorID && i.Room == x.Room && i.ConditionID == x.ConditionID).FirstOrDefault().InventoryOwnerID
                }).ToList();

                inventoryItemModelList.ForEach(ch =>
                {
                   // var inventoryImageName = Common.GetCartImages(ch.InventoryItemID, ch.InventoryID, ch.ConditionId) ?? mainImage;
                    //ch.InventoryImageName = inventoryImageName;
                    //ch.ImagePath = $"{_configuration.GetValue<string>("AwsConfig:S3Folder")}/{ch.ClientID}/images/{inventoryImageName}";
                    //ch.StatusName = (ch.StatusId == (int)Enums.Status.Active ? "Available" : (ch.StatusId == (int)Enums.Status.Reserved ? Enums.Status.Reserved.ToString() : ""));
                    ch.InventorySpaceType = _dbContext.SpaceTypes.Where(st => st.SpaceTypeID == ch.InventorySpaceTypeId).FirstOrDefault().SpaceTypeName;
                    ch.InventoryOwner = _dbContext.InventoryOwners.Where(own => own.InventoryOwnerID == ch.InventoryOwnerId).FirstOrDefault().OwnerName;

                });
                //inventoryItemModelList.ForEach(ch =>
                //{
                //    var inventoryImageName = Common.GetCartImages(ch.InventoryItemID, ch.InventoryID, ch.ConditionId) ?? mainImage;
                //    ch.InventoryImageName = inventoryImageName;
                //    ch.ImagePath = $"inventory/{ch.ClientID}/{inventoryImageName}";
                //});


            }
            catch (Exception ex)
            {
                throw;
            }

            return inventoryItemModelList;

        }

        public async Task<AdminInventoryModel> GetAdminInventoryByLocation(int clientId, int currentPage, int perPageRows, int startIndex, int buildingid, int floorid, string room)
        {
            AdminInventoryModel adminInventoryModel = new AdminInventoryModel();

            try
            {
                // var inventory = _dbContext.Inventories.Where(i => i.ClientID == clientId).Include(ii => ii.InventoryItems.Where(x => x.building == bulding && x.floor == floor)).ThenInclude(iii => iii.InventoryItemImages.Take(1)).ToList();

                //(from item in ListProducts
                // where item.ProductName != string.Empty
                // where item.CategoryId != (from subitem in categories
                //                           where subitem.CategoryName.Trim() == "Mobile"
                //                           select subitem.Id).First()
                // select item).ToList();

                var invIds = await _dbContext.InventoryItems.Where(ii => ii.ClientID == clientId
                                                          && (buildingid >= 0 ? ii.InventoryBuildingID.Equals(buildingid) : ii.InventoryBuildingID > buildingid)
                                                          && (floorid >= 0 ? ii.InventoryFloorID.Equals(floorid) : ii.InventoryFloorID > floorid)
                                                          && ii.Room.Contains((!string.IsNullOrEmpty(room) ? room : ""))
                                                          )
                                                        .Select(ii => ii.InventoryID).Distinct().ToListAsync();


                var inventory = await _dbContext.Inventories.Where(i => invIds.Contains(i.InventoryID) && i.ClientID == clientId).ToListAsync();
                                   

                //var inventory = await _dbContext.Inventories.Where(i => i.ClientID == clientId &&
                //                                                 _dbContext.InventoryItems
                //                                                .Any(ii => ii.Room.Contains((!string.IsNullOrEmpty(room) ? room : "")) ||
                //                                                        (buildingid >= 0 ? ii.InventoryBuildingID.Equals(buildingid) : ii.InventoryBuildingID > buildingid) ||
                //                                                        (floorid >= 0 ? ii.InventoryFloorID.Equals(floorid) : ii.InventoryFloorID > floorid)
                //                                                       )
                //                                                ).ToListAsync();

                //var inventory = (from i in _dbContext.Inventories
                //                 let invID = (from ii in _dbContext.InventoryItems
                //                              where ii.ClientID == clientId
                //                              where (
                //                                    (buildingid >= 0 ? ii.InventoryBuildingID.Equals(buildingid) : ii.InventoryBuildingID > buildingid) ||
                //                                    (floorid >= 0 ? ii.InventoryFloorID.Equals(floorid) : ii.InventoryFloorID > floorid) ||
                //                                    (ii.Room.Contains((!string.IsNullOrEmpty(room) ? room : "")))
                //                                    )
                //                              select ii.InventoryID
                //                              )
                                              
                //                 where i.ClientID == clientId
                //                 where invID.Contains(i.InventoryID)
                //                 select i).AsQueryable();

                                 


                //var inventory = (from i in _dbContext.Inventories
                //                 join ii in _dbContext.InventoryItems on i.InventoryID equals ii.InventoryID  
                //                 where i.ClientID == clientId
                //                 where (buildingid >= 0 ? ii.InventoryBuildingID.Equals(buildingid) : ii.InventoryBuildingID > buildingid)
                //                 where (floorid >= 0 ? ii.InventoryFloorID.Equals(floorid) : ii.InventoryFloorID > floorid)
                //                 where ii.Room.Contains((!string.IsNullOrEmpty(room) ? room : ""))
                //                 select i
                //                 //where ii.Building != "Donate"                               
                //                 ).AsQueryable();

                // var totalRows = _dbContext.Inventories.Where(i => i.ClientID == clientId).Count();
                var totalRows = inventory.Count();
                var totalPages = (int)Math.Ceiling((decimal)totalRows / perPageRows);

                adminInventoryModel.Count = totalRows;
                adminInventoryModel.TotalPages = totalPages;
                adminInventoryModel.PageNo = currentPage;
                adminInventoryModel.RowsPerPage = perPageRows;

                var admininv = inventory.Select(x => new AdminInventory {
                                    InventoryID = x.InventoryID,
                                    ClientID = x.ClientID,
                                    ItemTypeID = x.ItemTypeID,
                                    ItemRowID = x.ItemRowID,
                                    ItemCode = x.ItemCode,
                                    GlobalProductCatalogID = x.GlobalProductCatalogID,
                                    Category = _dbContext.ItemTypes.Where(it => it.ItemTypeID == x.ItemTypeID).FirstOrDefault().ItemTypeName,
                                    Description = x.Description,
                                    ManufacturerID = x.ManufacturerID,
                                    ManufacturerName = x.ManufacturerName,
                                    Fabric = x.Fabric,
                                    Fabric2 = x.Fabric2,
                                    Finish = x.Finish,
                                    Finish2 = x.Finish2,
                                    Size = x.Size,
                                    RFIDCode = x.RFIDCode,
                                    BarCode = x.BarCode,
                                    QRCode = x.QRCode,
                                    PartNumber = x.PartNumber,
                                    Height = x.Height,
                                    Width = x.Width,
                                    Depth = x.Depth,
                                    Diameter = x.Diameter,
                                    Top = x.Top,
                                    Edge = x.Edge,
                                    Base = x.Base,
                                    Frame = x.Frame,
                                    Seat = x.Seat,
                                    Back = x.Back,
                                    SeatHeight = x.SeatHeight,
                                    Modular = x.Modular,
                                    ImageName = x.MainImage,
                                    WarrantyYears = x.WarrantyYears,
                                    Tag = x.Tag,
                                    Unit = x.Unit,
                                    ImageUrl = _configuration.GetValue<string>("ImgUrl") + x.ClientID + "/",
                                    ImageSrc = _configuration.GetValue<string>("ImgUrl") + x.ClientID + "/images/" + x.MainImage,                   
                                    BucketName = _configuration.GetValue<string>("AwsConfig:BuketName"),
                                    ImagePath = $"{_configuration.GetValue<string>("AwsConfig:S3Folder")}/{x.ClientID}/images/{x.MainImage}",
                                    FilePath = $"{_configuration.GetValue<string>("AwsConfig:S3Folder")}/{x.ClientID}/sustainability_files/{x.InventoryID}"
                                }).OrderByDescending(ord => ord.InventoryID).Skip(startIndex).Take(perPageRows);

                //var inventory = (from i in _dbContext.Inventories
                //                 where i.ClientID == clientId
                //                 //where ii.Building != "Donate"
                //                 select new AdminInventory
                //                 {
                //                     InventoryID = i.InventoryID,
                //                     ClientID = i.ClientID,
                //                     ItemTypeID = i.ItemTypeID,
                //                     ItemRowID = i.ItemRowID,
                //                     ItemCode = i.ItemCode,
                //                     GlobalProductCatalogID = i.GlobalProductCatalogID,
                //                     Category = _dbContext.ItemTypes.Where(it => it.ItemTypeID == i.ItemTypeID).FirstOrDefault().ItemTypeName,
                //                     Description = i.Description,
                //                     ManufacturerID = i.ManufacturerID,
                //                     ManufacturerName = i.ManufacturerName,
                //                     Fabric = i.Fabric,
                //                     Fabric2 = i.Fabric2,
                //                     Finish = i.Finish,
                //                     Finish2 = i.Finish2,
                //                     Size = i.Size,
                //                     RFIDCode = i.RFIDCode,
                //                     BarCode = i.BarCode,
                //                     QRCode = i.QRCode,
                //                     PartNumber = i.PartNumber,
                //                     Height = i.Height,
                //                     Width = i.Width,
                //                     Depth = i.Depth,
                //                     Diameter = i.Diameter,
                //                     Top = i.Top,
                //                     Edge = i.Edge,
                //                     Base = i.Base,
                //                     Frame = i.Frame,
                //                     Seat = i.Seat,
                //                     Back = i.Back,
                //                     SeatHeight = i.SeatHeight,
                //                     Modular = i.Modular,
                //                     ImageName = i.MainImage,
                //                     WarrantyYears = i.WarrantyYears,
                //                     Tag = i.Tag,
                //                     Unit = i.Unit,
                //                     ImageUrl = _configuration.GetValue<string>("ImgUrl") + i.ClientID + "/",
                //                     ImageSrc = _configuration.GetValue<string>("ImgUrl") + i.ClientID + "/" + i.MainImage,
                //                     //Files = new List<string>(),
                //                     BucketName = _configuration.GetValue<string>("AwsConfig:BuketName"),
                //                     ImagePath = $"inventory/{i.ClientID}/{i.MainImage}",
                //                     FilePath = $"inventory/{i.ClientID}/fimg/{i.InventoryID}"
                //                     // }).AsQueryable().Take(100);
                //                     //}).AsQueryable().OrderByDescending(ord=>ord.InventoryID).Skip(currentPage).Take(perPageRows);
                //                     //}).AsQueryable().OrderBy(ord => ord.Category).Skip(startIndex).Take(perPageRows);
                //                 }).AsQueryable().OrderByDescending(ord => ord.InventoryID).Skip(startIndex).Take(perPageRows);

                //var adminInventoryModelList = await inventory.ToListAsync();
                //var adminInventoryModelList =  inventory;

                var adminInventoryModelList = admininv.ToList();

                if (adminInventoryModelList.Count > 0)
                {

                    foreach (var item in adminInventoryModelList)
                    {
                        var invitementity = _dbContext.InventoryItems.Where(ii => ii.InventoryID == item.InventoryID);
                        int totqty = invitementity.Count();

                        int reservedqty = invitementity.Where(ii => ii.StatusID == (int)Enums.Status.Reserved).Count();

                        adminInventoryModelList.Where(i => i.InventoryID == item.InventoryID)
                            .Select(s => { s.TotalQty = totqty; s.ReservedQty = reservedqty; return s; }).ToList();


                    }
                }

              

                adminInventoryModel.AdminInventories = adminInventoryModelList;

            }
            catch (Exception ex)
            {
                throw;
            }

            return adminInventoryModel;

        }

        public async Task<bool> UpdateAdminInventory(int inventoryId, string column, string value, int userId)
        {
            bool result = false;
            try
            {
                column = GetMappedColumn(column);

                var entity = _dbContext.Inventories.Where(x => x.InventoryID == inventoryId).FirstOrDefault();

                if (column.Equals("Category"))
                {
                    var itemTypesEntity = _dbContext.ItemTypes.Where(it => it.ClientID == entity.ClientID && it.ItemTypeID == entity.ItemTypeID).FirstOrDefault();

                    if (itemTypesEntity != null)
                    {
                        itemTypesEntity.ItemTypeName = value;
                        itemTypesEntity.UpdateDateTime = System.DateTime.Now;
                        itemTypesEntity.UpdateID = userId;

                        _dbContext.Update(entity);
                        await _dbContext.SaveChangesAsync();

                        result = true;
                    }
                    else
                    {
                        result = false;
                    }
                }
                else if (entity != null)
                {
                    //entity._dbContext.Inventories.GetType().GetProperty(column).Name
                    PropertyInfo propInstance = entity.GetType().GetProperty(column);
                    propInstance.SetValue(entity, value);
                    entity.UpdateDateTime = System.DateTime.Now;
                    entity.UpdateID = userId;

                    _dbContext.Update(entity);
                    await _dbContext.SaveChangesAsync();

                    result = true;
                }
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        public async Task<bool> UpdateAdminInventoryItem(InventoryItemModel inventoryItemModel, string column, int value, int userId)
        {
            bool result = false;
            using (IDbContextTransaction transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    if (column.ToLower().Equals("qty"))
                    {
                        if (inventoryItemModel.Qty < Convert.ToInt32(value))
                        {
                            var inventoryItemrow = _dbContext.InventoryItems.Where(ii => ii.InventoryItemID == inventoryItemModel.InventoryItemID).FirstOrDefault();

                            InventoryItem model = new InventoryItem();
                            model.InventoryID = inventoryItemModel.InventoryID;
                            model.DisplayOnSite = true;
                            model.InventoryBuildingID = inventoryItemrow.InventoryBuildingID;
                            // model.Building = inventoryItemrow.Building;
                            model.InventoryFloorID = inventoryItemrow.InventoryFloorID;
                            // model.Floor = inventoryItemrow.Floor;
                            model.Room = inventoryItemrow.Room;
                            model.ConditionID = inventoryItemrow.ConditionID;
                            model.DamageNotes = "";
                            model.ClientID = inventoryItemrow.ClientID;
                            model.GPSLocation = "";
                            model.RFIDcode = "";
                            model.Barcode = "";
                            model.CreateDateTime = System.DateTime.Now;
                            model.CreateID = userId;
                            model.UpdateDateTime = System.DateTime.Now;
                            model.UpdateID = userId;
                            model.StatusID = (int)Enums.Status.Active;

                            await _dbContext.AddAsync(model);
                            await _dbContext.SaveChangesAsync();

                            int inventoryItemId = model.InventoryID;

                            //need to implement new History tracker 
                           // var invitemststushistory = InsertInventoryItemStatusHistory(null, inventoryItemId, (int)Enums.Status.QtyUpdate, userId);

                            result = true;

                        }
                        else if (inventoryItemModel.Qty > Convert.ToInt32(value))
                        {
                            var inventoryItems = _dbContext.InventoryItems
                                                    .Where(ii => ii.InventoryID == inventoryItemModel.InventoryID
                                                            && ii.InventoryBuildingID == inventoryItemModel.InventoryBuildingID
                                                            && ii.InventoryFloorID == inventoryItemModel.InventoryFloorID
                                                            && ii.Room == inventoryItemModel.Room
                                                            && ii.ConditionID == inventoryItemModel.ConditionId)
                                                    .AsQueryable();// LastOrDefault();

                            var entity = inventoryItems.OrderBy(ord => ord.InventoryItemID).LastOrDefault();

                            if (entity != null)
                            {
                                //need to implement new History tracker 
                                //InsertInventoryItemStatusHistory(null, entity.InventoryItemID, (int)Enums.Status.QtyUpdate, userId);

                                entity.DisplayOnSite = false;
                                entity.UpdateDateTime = System.DateTime.Now;
                                entity.UpdateID = userId;
                                //need to implement new History tracker 
                                //entity.StatusID = (int)Enums.Status.QtyUpdate;

                                _dbContext.Update(entity);
                                await _dbContext.SaveChangesAsync();

                                //var invitemststushistory = InsertInventoryItemStatusHistory(entity.InventoryItemID, entity.StatusID);


                                result = true;

                            }
                        }
                    }
                    else
                    {
                        var entity = _dbContext.InventoryItems
                                                    .Where(ii => ii.InventoryID == inventoryItemModel.InventoryID
                                                            && ii.InventoryBuildingID == inventoryItemModel.InventoryBuildingID
                                                            && ii.InventoryFloorID == inventoryItemModel.InventoryFloorID
                                                            && ii.Room == inventoryItemModel.Room
                                                            && ii.ConditionID == inventoryItemModel.ConditionId)
                                                    .ToList();

                        if (entity != null)
                        {
                            //need to implement new History tracker 
                            //InsertInventoryItemStatusHistory(entity, 0, (int)Enums.Status.ConditionUpdate, userId);

                            var replacedEntity = entity.Select(x => new InventoryItem()
                            {
                                ConditionID = value,
                                UpdateDateTime = System.DateTime.Now,
                                UpdateID = userId,
                                //need to implement new History tracker 
                                //StatusID = (int)Enums.Status.ConditionUpdate
                            }).ToList();


                            _dbContext.UpdateRange(replacedEntity);
                            await _dbContext.SaveChangesAsync();

                            result = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw;
                }


                return result;
            }
        }
        private string GetMappedColumn(string col)
        {
            try
            {
                //int value = 1;
                //string description = Enum.GetName(col);

                var enumType = typeof(ColumnMapper);
                var memberInfos = enumType.GetMember(col);
                var enumValueMemberInfo = memberInfos.FirstOrDefault(m => m.DeclaringType == enumType);
                var valueAttributes = enumValueMemberInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
                var description = ((DescriptionAttribute)valueAttributes[0]).Description;

                return description;

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //private bool InsertInventoryItemStatusHistory(List<InventoryItem> inventoryItem, int inventoryitemid, int statusid, int userId)
        //{
        //    try
        //    {
        //        //foreach (var item in model.CartItem)
        //        //{
        //        //List<InventoryItem> inventoryItemStatusHistory = new List<InventoryItem>();


        //        // var inventoryItem = _dbContext.InventoryItems.Where(ii => ii.InventoryItemID == inventoryitemid).ToList();

        //        var statusHistorylist = inventoryItem.Select(x => new InventoryItemStatusHistory()
        //        {
        //            InventoryItemID = x.InventoryItemID,
        //            InventoryID = x.InventoryID,
        //            ClientID = x.ClientID,
        //            InventoryBuildingID = x.InventoryBuildingID,
        //            InventoryFloorID = x.InventoryFloorID,
        //            Room = x.Room,
        //            ConditionID = x.ConditionID,
        //            DestInventoryBuildingID = 0,
        //            DestInventoryFloorID = 0,
        //            DestRoom = "",
        //            DepartmentCostCenter = "",
        //            StatusID = statusid,
        //            DisplayOnSite = x.DisplayOnSite,
        //            DamageNotes = x.DamageNotes,
        //            GPSLocation = x.GPSLocation,
        //            RFIDcode = x.RFIDcode,
        //            Barcode = x.Barcode,
        //            ProposalNumber = x.ProposalNumber,
        //            PoOrderNo = x.PoOrderNo,
        //            PoOrderDate = x.PoOrderDate,
        //            NonSSIPurchaseDate = x.NonSSIPurchaseDate,
        //            WarrantyRequestID = x.WarrantyRequestID,
        //            OrigInventoryItemCreateDateTime = x.CreateDateTime,
        //            CreateID = x.CreateID,
        //            CreateDateTime = System.DateTime.Now,
        //            UpdateID = userId,
        //            UpdateDateTime = System.DateTime.Now
        //        }).ToList();

        //        _dbContext.AddRange(statusHistorylist);
        //        _dbContext.SaveChanges();
        //        // }

        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}


        public async Task<bool> UploadFileToS3Bucket(IFormFile file, int clientId)
        {

            return await _aws3Repository.UploadFileAsync(file, clientId);
        }

        public async Task<string> UploadImageToS3Bucket(IFormFile file, int clientId, int inventoryId, int userId)
        {
            string imageName = "";
            using (IDbContextTransaction transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    var entity = await _dbContext.Inventories.Where(i => i.InventoryID == inventoryId).FirstOrDefaultAsync();
                    string imgName = entity.MainImage;

                    if (string.IsNullOrEmpty(imgName))
                    {
                        imgName = System.Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

                        entity.MainImage = imgName;
                        entity.UpdateID = userId;
                        entity.UpdateDateTime = System.DateTime.Now;
                        _dbContext.Update(entity);
                        await _dbContext.SaveChangesAsync();

                        var invImages = new InventoryImages();
                        invImages.InventoryID = entity.InventoryID;
                        invImages.InventoryItemID = null;
                        invImages.ConditionID = _dbContext.InventoryItems.Where(ii => ii.InventoryID == entity.InventoryID).OrderBy(m => m.InventoryItemID).First().ConditionID;
                        invImages.ImageGUID = Guid.Parse(Path.GetFileNameWithoutExtension(imgName).ToUpper());
                        invImages.ImageName = imgName;
                        invImages.ImageURL = _configuration.GetValue<string>("S3Url");
                        invImages.ClientID = clientId;
                        invImages.Width = null;
                        invImages.Height = null;
                        invImages.ItemTypeAutomationID = 0;
                        invImages.ItemTypeAutomationOptionID = 0;
                        invImages.TempPhotoName = "";
                        invImages.CreateDateTime = System.DateTime.Now;
                        invImages.CreateID = userId;
                        invImages.UpdateDateTime = null;
                        invImages.UpdateID = null;
                        invImages.SubmissionDate = System.DateTime.Now;
                        invImages.StatusID = (int)Enums.Status.Active;

                        await _dbContext.AddAsync(invImages);
                        await _dbContext.SaveChangesAsync();
                    }

                    //await _aws3Repository.UploadImageAsync(file, clientId, imgName);
                    //await _awsUploadRepository.UploadImageAsync(file, _configuration.GetValue<string>("AwsConfig:BuketName"), $"inventory/{clientId}/{imgName}");

                    await transaction.CommitAsync();

                    imageName = imgName;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
            return imageName;
        }

        public async Task<List<AdminInventoryItemTypesOptionSetModel>> GetInventoryItemTypes(int clientid, int itemtypeid)
        {
            try
            {
                List<AdminInventoryItemTypesOptionSetModel> adminInventoryItemTypesModelsList = new List<AdminInventoryItemTypesOptionSetModel>();


                var result = (from it in _dbContext.ItemTypes
                              from ito in _dbContext.ItemTypeOptions.Where(x => (x.ItemTypeAttributeID.Equals(0)
                                                                           && x.ItemTypeAttributeID.Equals(3)
                                                                           && itemtypeid.Equals(0))
                                                                           ||
                                                                           (
                                                                           (it.ClientID.Equals(1) || it.ClientID.Equals(clientid))
                                                                           && (x.ItemTypeAttributeID.Equals(0) || it.ItemTypeAttributeID.Equals(x.ItemTypeAttributeID))
                                                                           )
                                                                           ).DefaultIfEmpty()
                              from itol in _dbContext.ItemTypeOptionLines.Where(ol => ol.StatusID.Equals(5)
                                                                            && (ito.ItemTypeOptionID.Equals(ol.ItemTypeOptionID)
                                                                                // || ol.ItemTypeOptionID is null
                                                                                )
                                                                           ).DefaultIfEmpty()
                              where it.ItemTypeID == itemtypeid
                              where ito.StatusID == 5
                              select new AdminInventoryItemTypesOptionSetModel
                              {
                                  ClientID = it.ClientID,
                                  StatusID = ito.StatusID,
                                  ItemTypeID = it.ItemTypeID,
                                  ItemTypeCode = it.ItemTypeCode,
                                  ItemTypeName = it.ItemTypeName,
                                  ItemTypeOptionID = ito.ItemTypeOptionID,
                                  ItemTypeOptionName = ito.ItemTypeOptionName,
                                  ItemTypeOptionCode = ito.ItemTypeOptionCode,
                                  OrderSequence = ito.OrderSequence,
                                  FieldType = ito.FieldType,
                                  ItemTypeAttributeID = ito.ItemTypeAttributeID,
                                  //ItemTypeSupportFileID = ito.ItemTypeSupportFileID,
                                  ValType = ito.ValType,
                                  IsHide = ito.IsHide,

                                  ValTypeDesc = ito.ValType == 1 ? "text" :
                                                 ito.ValType == 2 ? "textarea" :
                                                 ito.ValType == 3 ? "mat-select" :
                                                 ito.ValType == 4 ? "Date" :
                                                 ito.ValType == 5 ? "Time" :
                                                 ito.ValType == 6 ? "mat-checkbox" :
                                                 ito.ValType == 20 ? "DataFromAPIx" :
                                                 ito.ValType == 30 ? "GPS" :
                                                 ito.ValType == 40 ? "TotalCount" :
                                                 ito.ValType == 50 ? "TextBoxPlus" :
                                                 ito.ValType == 60 ? "QuantityPlus" :
                                                 ito.ValType == 70 ? "PhotoPlus" :
                                                 ito.ValType == 80 ? "RadioPlus" :
                                                 ito.ValType == 90 ? "DropdownPlus" :
                                                 ito.ValType == 100 ? "RadioButton" : "",

                                  ItemTypeOptionLineID = string.IsNullOrEmpty(itol.ItemTypeOptionLineID.ToString()) ? 0 : itol.ItemTypeOptionLineID,
                                  ItemTypeOptionLineCode = itol.ItemTypeOptionLineCode ?? "",
                                  ItemTypeOptionLineName = itol.ItemTypeOptionLineName ?? "",
                                  InventoryUserAcceptanceRulesRequired = itol.InventoryUserAcceptanceRulesRequired ?? ""
                              }).AsQueryable().OrderBy(ord => ord.ItemTypeID).ThenBy(ord => ord.OrderSequence);
                //order by i.ItemTypeID, io.OrderSequence

                adminInventoryItemTypesModelsList = await result.ToListAsync();

                var groupData = adminInventoryItemTypesModelsList.Where(s => s.ItemTypeOptionLineCode != "" && !s.IsHide).GroupBy(grp => grp.ValType).ToList();

                //var dataGrp = newData.GroupBy(grp => grp.ValType).ToList();

                //string 
                foreach (var item in groupData)
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    int index = 1;
                    foreach (var key in item)
                    {
                        if (item.Count() == index)
                            stringBuilder.Append($"{key.ItemTypeOptionLineName}");
                        else
                        {
                            stringBuilder.Append($"{key.ItemTypeOptionLineName}_");
                        }

                        index++;
                    }

                    adminInventoryItemTypesModelsList.Where(adm => adm.ValType == item.Key).FirstOrDefault().ItemTypeOptionLineNameDisplay = stringBuilder.ToString();
                }

                return adminInventoryItemTypesModelsList;

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> EditInventory(int userid,string apicallurl, AdminInventory adminInventory)
        {
            bool result = false;
            using (IDbContextTransaction transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    var entity = await _dbContext.Inventories.Where(i => i.InventoryID == adminInventory.InventoryID).FirstOrDefaultAsync();

                    if (entity != null)
                    {
                        var batchTransactionGuid = System.Guid.NewGuid();
                        var creatBatchTransactionDate = System.DateTime.Now;
                        Dictionary<string, string> historyCols = new Dictionary<string, string>();

                        if (entity.Description != adminInventory.Description)
                            historyCols.Add("Description", entity.Description);
                        if (entity.ManufacturerID != adminInventory.ManufacturerID)
                            historyCols.Add("ManufacturerID", entity.ManufacturerID.ToString());
                        if (entity.ManufacturerName != adminInventory.ManufacturerName)
                            historyCols.Add("ManufacturerName", entity.ManufacturerName);
                        if (entity.Fabric != adminInventory.Fabric)
                            historyCols.Add("Fabric", entity.Fabric);
                        if (entity.Fabric2 != adminInventory.Fabric2)
                            historyCols.Add("Fabric2", entity.Fabric2);
                        if (entity.Finish != adminInventory.Finish)
                            historyCols.Add("Finish", entity.Finish);
                        if (entity.Finish2 != adminInventory.Finish2)
                            historyCols.Add("Finish2", entity.Finish2);
                        if (entity.Size != adminInventory.Size)
                            historyCols.Add("Size", entity.Size);
                        if (entity.RFIDCode != adminInventory.RFIDCode)
                            historyCols.Add("RFIDCode", entity.RFIDCode);
                        if (entity.BarCode != adminInventory.BarCode)
                            historyCols.Add("BarCode", entity.BarCode);
                        if (entity.QRCode != adminInventory.QRCode)
                            historyCols.Add("QRCode", entity.QRCode);
                        if (entity.PartNumber != adminInventory.PartNumber)
                            historyCols.Add("PartNumber", entity.PartNumber);
                        if (entity.Height != adminInventory.Height)
                            historyCols.Add("Height", entity.Height.ToString());
                        if (entity.Width != adminInventory.Width)
                            historyCols.Add("Width", entity.Width.ToString());
                        if (entity.Depth != adminInventory.Depth)
                            historyCols.Add("Depth", entity.Depth.ToString());
                        if (entity.Diameter != adminInventory.Diameter)
                            historyCols.Add("Diameter", entity.Diameter.ToString());
                        if (entity.Top != adminInventory.Top)
                            historyCols.Add("Top", entity.Top);
                        if (entity.Edge != adminInventory.Edge)
                            historyCols.Add("Edge", entity.Edge);
                        if (entity.Base != adminInventory.Base)
                            historyCols.Add("Base", entity.Base);
                        if (entity.Frame != adminInventory.Frame)
                            historyCols.Add("Frame", entity.Frame);
                        if (entity.Seat != adminInventory.Seat)
                            historyCols.Add("Seat", entity.Seat);
                        if (entity.Back != adminInventory.Back)
                            historyCols.Add("Back", entity.Back);
                        if (entity.SeatHeight != adminInventory.SeatHeight)
                            historyCols.Add("SeatHeight", entity.SeatHeight.ToString());
                        if (entity.Modular != adminInventory.Modular)
                            historyCols.Add("Modular", entity.Modular);
                        if (entity.WarrantyYears != adminInventory.WarrantyYears)
                            historyCols.Add("WarrantyYears", entity.WarrantyYears.ToString());
                        if (entity.Tag != adminInventory.Tag)
                            historyCols.Add("Tag", entity.Tag);
                        if (entity.Unit != adminInventory.Unit)
                            historyCols.Add("Unit", entity.Unit);


                        entity.Description = adminInventory.Description;
                        entity.ManufacturerID = adminInventory.ManufacturerID;
                        entity.ManufacturerName = adminInventory.ManufacturerName;
                        entity.Fabric = adminInventory.Fabric;
                        entity.Fabric2 = adminInventory.Fabric2;
                        entity.Finish = adminInventory.Finish;
                        entity.Finish2 = adminInventory.Finish2;
                        entity.Size = adminInventory.Size;
                        entity.RFIDCode = adminInventory.RFIDCode;
                        entity.BarCode = adminInventory.BarCode;
                        entity.QRCode = adminInventory.QRCode;
                        entity.PartNumber = adminInventory.PartNumber;
                        entity.Height = adminInventory.Height;
                        entity.Width = adminInventory.Width;
                        entity.Depth = adminInventory.Depth;
                        entity.Diameter = adminInventory.Diameter;
                        entity.Top = adminInventory.Top;
                        entity.Edge = adminInventory.Edge;
                        entity.Base = adminInventory.Base;
                        entity.Frame = adminInventory.Frame;
                        entity.Seat = adminInventory.Seat;
                        entity.Back = adminInventory.Back;
                        entity.SeatHeight = adminInventory.SeatHeight;
                        entity.Modular = adminInventory.Modular;
                        entity.WarrantyYears = adminInventory.WarrantyYears;
                        entity.Tag = adminInventory.Tag;
                        entity.Unit = adminInventory.Unit;
                        entity.UpdateDateTime = System.DateTime.Today;
                        entity.UpdateID = userid;

                        _dbContext.Update(entity);
                        await _dbContext.SaveChangesAsync();

                        ////insert InventoryHistory records belong to orderid and InventoryItemID
                        var invHistory = historyCols.Select(x => new InventoryHistory()
                        {
                            BatchTransactionGUID = batchTransactionGuid,
                            OrderID = null,
                            EntityID = entity.InventoryID,
                            ApiName = apicallurl,
                            TableName = "Inventory",
                            ColumnName = x.Key,
                            OldValue = x.Value,
                            NewValue = (
                                        x.Key.Contains("Description") ? adminInventory.Description :
                                        x.Key.Contains("ManufacturerID") ? adminInventory.ManufacturerID.ToString() :
                                        x.Key.Contains("ManufacturerName") ? adminInventory.ManufacturerName :
                                        x.Key.Contains("Fabric") ? adminInventory.Fabric :
                                        x.Key.Contains("Fabric2") ? adminInventory.Fabric2 :
                                        x.Key.Contains("Finish") ? adminInventory.Finish :
                                        x.Key.Contains("Finish2") ? adminInventory.Finish2 :
                                        x.Key.Contains("Size") ? adminInventory.Size :
                                        x.Key.Contains("RFIDCode") ? adminInventory.RFIDCode :
                                        x.Key.Contains("BarCode") ? adminInventory.BarCode :
                                        x.Key.Contains("QRCode") ? adminInventory.QRCode :
                                        x.Key.Contains("PartNumber") ? adminInventory.PartNumber :
                                        x.Key.Contains("Height") ? adminInventory.Height.ToString() :
                                        x.Key.Contains("Width") ? adminInventory.Width.ToString() :
                                        x.Key.Contains("Depth") ? adminInventory.Depth.ToString() :
                                        x.Key.Contains("Diameter ") ? adminInventory.Diameter.ToString() :
                                        x.Key.Contains("Top") ? adminInventory.Top :
                                        x.Key.Contains("Edge") ? adminInventory.Edge :
                                        x.Key.Contains("Base") ? adminInventory.Base :
                                        x.Key.Contains("Frame") ? adminInventory.Frame :
                                        x.Key.Contains("Seat") ? adminInventory.Seat :
                                        x.Key.Contains("Back") ? adminInventory.Back :
                                        x.Key.Contains("SeatHeight") ? adminInventory.SeatHeight.ToString() :
                                        x.Key.Contains("Modular") ? adminInventory.Modular :
                                        x.Key.Contains("WarrantyYears") ? adminInventory.WarrantyYears.ToString() :
                                        x.Key.Contains("Tag") ? adminInventory.Tag :
                                        x.Key.Contains("Unit") ? adminInventory.Unit : ""                                   
                                      
                                       ),
                            Description = x.Key + " Updated",
                            CreateDateTime = creatBatchTransactionDate,
                            CreateID = userid
                        });

                        await _dbContext.AddRangeAsync(invHistory);
                        await _dbContext.SaveChangesAsync();

                        await transaction.CommitAsync();
                        result = true;
                        //CreateDateTime 
                        //CreateID 
                        //ClientID 
                        //ItemTypeID 
                        //ItemRowID 
                        // ItemCode 
                        //GlobalProductCatalogID 
                        // Category 


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

        public async Task<AdminInventory> CreateInventory(int userid, int clientid, CreateInventoryModel createInventory)
        {
            // bool result = false;
            AdminInventory adminInventoryModel = new AdminInventory();
            using (IDbContextTransaction transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    Inventory inventory = new Inventory();

                    inventory.ClientID = clientid;
                    inventory.ItemTypeID = createInventory.ItemTypeId;
                    inventory.ItemRowID = 0;
                    inventory.ItemCode = createInventory.ManufacturerName ?? "";
                    inventory.Category = _dbContext.ItemTypes.FirstOrDefault(it => it.ItemTypeID == createInventory.ItemTypeId).ItemTypeName;
                    inventory.Description = createInventory.Description ?? "";
                    inventory.ManufacturerID = createInventory.ManufacturerID > 0 ? createInventory.ManufacturerID : 0;
                    inventory.ManufacturerName = createInventory.ManufacturerName ?? "";
                    inventory.Fabric = "";
                    inventory.Fabric2 = "";
                    inventory.Finish = createInventory.Finish ?? "";
                    inventory.Finish2 = "";
                    inventory.Size = "";
                    inventory.RFIDCode = "";
                    inventory.BarCode = "";
                    inventory.PartNumber = createInventory.PartNumber ?? "";
                    inventory.Height = createInventory.Height;
                    inventory.Width = createInventory.Width;
                    inventory.Depth = createInventory.Depth;
                    inventory.Diameter = createInventory.Diameter;
                    inventory.Top = createInventory.Top ?? "";
                    inventory.Edge = createInventory.Edge ?? "";
                    inventory.Base = createInventory.Base ?? "";
                    inventory.Frame = createInventory.Frame ?? "";
                    inventory.Seat = createInventory.Seat ?? "";
                    inventory.Back = createInventory.Back ?? "";
                    inventory.SeatHeight = createInventory.SeatHeight.HasValue ? createInventory.SeatHeight : 0;
                    inventory.Modular = createInventory.Modular ?? "";
                    inventory.MainImage = "";
                    inventory.WarrantyYears = 1;
                    inventory.Tag = createInventory.Tag ?? "";
                    inventory.Unit = createInventory.Unit ?? "";
                    inventory.CreateDateTime = System.DateTime.Now;
                    inventory.CreateID = userid;
                    inventory.DeviceDate = System.DateTime.Now;
                    inventory.SubmissionDate = System.DateTime.Now;
                    inventory.SubmissionID = 0;
                    inventory.StatusID = (int)Enums.Status.Active;

                    await _dbContext.AddAsync(inventory);
                    await _dbContext.SaveChangesAsync();

                    int inventoryid = inventory.InventoryID;

                    var invEntity = await _dbContext.Inventories.Where(inv => inv.InventoryID == inventoryid).SingleOrDefaultAsync();

                    invEntity.ItemCode =  $"{invEntity.ItemCode} {inventoryid}";

                    _dbContext.Update(invEntity);
                    await _dbContext.SaveChangesAsync();

                    List<InventoryItem> invitemlist = new List<InventoryItem>();

                    for (int index = 0; index < createInventory.Qty; index++)
                    {
                        InventoryItem inventoryItem = new InventoryItem();

                        inventoryItem.InventoryBuildingID = createInventory.Building;
                        inventoryItem.InventoryFloorID = createInventory.Floor;
                        inventoryItem.Room = createInventory.Room;
                        inventoryItem.ConditionID = createInventory.ConditionId;
                        inventoryItem.ClientID = clientid;
                        inventoryItem.InventoryID = inventoryid;
                        //inventoryItem.InventoryOwnerID = 0;
                        //inventoryItem.InventorySpaceTypeID = 0;
                        // inventoryItem.RFIDcode = "";
                        inventoryItem.AddedToCartItem = false;
                        inventoryItem.DisplayOnSite = true;
                        //inventoryItem.DamageNotes = "";
                        inventoryItem.Barcode = "";
                        inventoryItem.StatusID = (int)Enums.Status.Active;
                        inventoryItem.CreateDateTime = System.DateTime.Now;
                        inventoryItem.CreateID = userid;
                        inventoryItem.SubmissionID = 0;
                        inventoryItem.DamageNotes = "";
                        inventoryItem.RFIDcode = "";
                        inventoryItem.ProposalNumber = createInventory.ProposalNumber ?? "";
                        inventoryItem.PoOrderNo = !string.IsNullOrEmpty(createInventory.PoOrderNo) ? Convert.ToDecimal(createInventory.PoOrderNo) : 0;

                        await _dbContext.AddAsync(inventoryItem);
                        await _dbContext.SaveChangesAsync();

                        int invitemid = inventoryItem.InventoryItemID;

                        invitemlist.Add(inventoryItem);
                    }

                    if(createInventory.ConditionId == (int)Enums.Condition.New)
                    {
                        var ordResult = await CreateNewInstallationOrderWithItems(invitemlist, invEntity.ItemCode, invEntity.Description, userid, clientid);
                    }

                    var adminInventory = (from i in _dbContext.Inventories
                                          where i.InventoryID == inventoryid
                                          select new AdminInventory
                                          {
                                              InventoryID = i.InventoryID,
                                              ClientID = i.ClientID,
                                              ItemTypeID = i.ItemTypeID,
                                              ItemRowID = i.ItemRowID,
                                              ItemCode = i.ItemCode,
                                              GlobalProductCatalogID = i.GlobalProductCatalogID,
                                              Category = _dbContext.ItemTypes.Where(it => it.ItemTypeID == i.ItemTypeID).FirstOrDefault().ItemTypeName,
                                              Description = i.Description,
                                              ManufacturerID = i.ManufacturerID,
                                              ManufacturerName = i.ManufacturerName,
                                              Fabric = i.Fabric,
                                              Fabric2 = i.Fabric2,
                                              Finish = i.Finish,
                                              Finish2 = i.Finish2,
                                              Size = i.Size,
                                              RFIDCode = i.RFIDCode,
                                              BarCode = i.BarCode,
                                              QRCode = i.QRCode,
                                              PartNumber = i.PartNumber,
                                              Height = i.Height,
                                              Width = i.Width,
                                              Depth = i.Depth,
                                              Diameter = i.Diameter,
                                              Top = i.Top,
                                              Edge = i.Edge,
                                              Base = i.Base,
                                              Frame = i.Frame,
                                              Seat = i.Seat,
                                              Back = i.Back,
                                              SeatHeight = i.SeatHeight,
                                              Modular = i.Modular,
                                              ImageName = i.MainImage,
                                              WarrantyYears = i.WarrantyYears,
                                              Tag = i.Tag,
                                              Unit = i.Unit,
                                              ImageUrl = _configuration.GetValue<string>("ImgUrl") + i.ClientID + "/",
                                              ImageSrc = _configuration.GetValue<string>("ImgUrl") + i.ClientID + "/images/" + i.MainImage,
                                              //Files = new List<string>(),
                                              BucketName = _configuration.GetValue<string>("AwsConfig:BuketName"),
                                              ImagePath = $"{_configuration.GetValue<string>("AwsConfig:S3Folder")}/{i.ClientID}/images/{i.MainImage}",
                                              FilePath = $"{_configuration.GetValue<string>("AwsConfig:S3Folder")}/{i.ClientID}/sustainability_files/{i.InventoryID}"
                                              // }).AsQueryable().Take(100);
                                              //}).AsQueryable().OrderByDescending(ord=>ord.InventoryID).Skip(currentPage).Take(perPageRows);
                                          }).AsQueryable();

                    await transaction.CommitAsync();
                    adminInventoryModel = await adminInventory.FirstOrDefaultAsync();

                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
            return adminInventoryModel;
        }

        public async Task<List<StatusModel>> GetStatus()
        {
            List<StatusModel> status = new List<StatusModel>();

            try
            {
                status.Add(new StatusModel()
                {
                    StatusId = (int)Enums.Status.Active,
                    StatusName = "Available"
                });

                status.Add(new StatusModel()
                {
                    StatusId = (int)Enums.Status.Reserved,
                    StatusName = Enums.Status.Reserved.ToString()
                });
            }
            catch(Exception ex)
            {
                throw;
            }

            return status;
        }

        public async Task<bool> EditInventoryItem(int userid,int prevQty, string apicallurl, InventoryItemModel inventoryItemModel)
        {
            bool result = false;
            using (IDbContextTransaction transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    var entity = await _dbContext.InventoryItems.Where(i => i.InventoryItemID == inventoryItemModel.InventoryItemID).FirstOrDefaultAsync();

                    if (entity != null)
                    {
                        #region current code commented due to change the flow of history tracker and only condition change.
                        //if(inventoryItemModel.Qty > prevQty)
                        //{
                        //    //var inventoryItems = _dbContext.InventoryItems
                        //    //                       .Where(ii => ii.InventoryID == inventoryItemModel.InventoryID
                        //    //                               && ii.InventoryBuildingID == inventoryItemModel.InventoryBuildingID
                        //    //                               && ii.InventoryFloorID == inventoryItemModel.InventoryFloorID
                        //    //                               && ii.Room == inventoryItemModel.Room
                        //    //                               && ii.ConditionID == inventoryItemModel.ConditionId)
                        //    //                       .AsQueryable();// LastOrDefault();

                        //    //var entityInvItems = inventoryItems.OrderBy(ord => ord.InventoryItemID).LastOrDefault();

                        //    var invItem = new List<InventoryItem>();

                        //    for (int i = 1; i <= inventoryItemModel.Qty - prevQty; i++)
                        //    {
                        //        invItem.Add(new InventoryItem()
                        //        {
                        //            InventoryID = entity.InventoryID,
                        //            ClientID = entity.ClientID,
                        //            InventoryBuildingID = entity.InventoryBuildingID,
                        //            InventoryFloorID = entity.InventoryFloorID,
                        //            Room = entity.Room,
                        //            ConditionID = entity.ConditionID,
                        //            StatusID = (int)Enums.Status.QtyUpdate,
                        //            DisplayOnSite = true,
                        //            InventorySpaceTypeID = entity.InventorySpaceTypeID,
                        //            InventoryOwnerID = entity.InventoryOwnerID,
                        //            DamageNotes = inventoryItemModel.DamageNotes,
                        //            GPSLocation = null,
                        //            RFIDcode = "",
                        //            Barcode = "",
                        //            ProposalNumber = null,
                        //            PoOrderNo = null,
                        //            PoOrderDate = null,
                        //            NonSSIPurchaseDate = null,
                        //            WarrantyRequestID = null,
                        //            CreateID = userid,
                        //            CreateDateTime = System.DateTime.Now,
                        //            UpdateID = null,
                        //            UpdateDateTime = null,
                        //            AddedToCartItem = false,
                        //            SubmissionID = null,
                        //            SubmissionDate = null
                        //        });
                        //    }
                        //    await _dbContext.AddRangeAsync(invItem);
                        //    await _dbContext.SaveChangesAsync();

                        //    InsertInventoryItemStatusHistory(invItem, entity.InventoryItemID, (int)Enums.Status.QtyUpdate, userid);


                        //    //entity.DisplayOnSite = true;
                        //    //entity.CreateDateTime = System.DateTime.Now;
                        //    //entity.UpdateID = userid;
                        //    //entity.StatusID = (int)Enums.Status.QtyUpdate;

                        //    //_dbContext.Update(entity);
                        //    //await _dbContext.SaveChangesAsync();

                        //    //var invitemststushistory = InsertInventoryItemStatusHistory(entity.InventoryItemID, entity.StatusID);


                        //    result = true;


                        //}

                        //if (inventoryItemModel.Qty > 1)
                        //{
                        //    List<InventoryItem> inventoyItemList = new List<InventoryItem>();

                        //    for (int i=1; i < inventoryItemModel.Qty;i++)
                        //    {
                        //        var inventoyItem = new InventoryItem();

                        //        inventoyItem.InventoryID = entity.InventoryID;
                        //        inventoyItem.ClientID = entity.ClientID;
                        //        inventoyItem.DisplayOnSite = entity.DisplayOnSite;
                        //        inventoyItem.StatusID = entity.StatusID;
                        //        inventoyItem.InventoryBuildingID = entity.InventoryBuildingID;
                        //        inventoyItem.InventoryFloorID = entity.InventoryFloorID;
                        //        inventoyItem.Room = entity.Room;
                        //        inventoyItem.InventorySpaceTypeID = entity.InventorySpaceTypeID;
                        //        inventoyItem.InventoryOwnerID = entity.InventoryOwnerID;
                        //        inventoyItem.ConditionID = inventoryItemModel.ConditionId;
                        //        inventoyItem.DamageNotes = inventoryItemModel.DamageNotes;
                        //        inventoyItem.GPSLocation = entity.GPSLocation;
                        //        inventoyItem.RFIDcode = entity.RFIDcode;
                        //        inventoyItem.Barcode = entity.Barcode;
                        //        //inventoyItem.QRCode = entity.QRCode;
                        //        //inventoyItem.ProposalNumber = entity.ProposalNumber;
                        //        //inventoyItem.PoOrderNo = entity.PoOrderNo;
                        //        //inventoyItem.PoOrderDate = entity.PoOrderDate;
                        //        //inventoyItem.NonSSIPurchaseDate = entity.NonSSIPurchaseDate;
                        //        //inventoyItem.WarrantyRequestID = entity.WarrantyRequestID;
                        //        inventoyItem.AddedToCartItem = entity.AddedToCartItem;
                        //        inventoyItem.CreateDateTime = System.DateTime.Now;
                        //        inventoyItem.CreateID = userid;
                        //        inventoyItem.UpdateDateTime = entity.UpdateDateTime;
                        //        inventoyItem.UpdateID = userid;
                        //        //inventoyItem.SubmissionDate = entity.SubmissionDate;
                        //        //inventoyItem.SubmissionID = entity.SubmissionID;

                        //        inventoyItemList.Add(inventoyItem);
                        //    }


                        //    await _dbContext.AddRangeAsync(inventoyItemList);
                        //    await _dbContext.SaveChangesAsync();
                        //}
                        //else if (inventoryItemModel.Qty == 0)
                        //{
                        //    var removeentity = await _dbContext.InventoryItems.Where(i => i.InventoryID == inventoryItemModel.InventoryID && i.ConditionID == inventoryItemModel.ConditionId && i.StatusID == (int)Enums.Status.Active).LastOrDefaultAsync();

                        //    _dbContext.Remove(removeentity);
                        //    await _dbContext.SaveChangesAsync();
                        //}
                        //else
                        //{                        
                        //entity.ConditionID = inventoryItemModel.ConditionId;

                        //entity.UpdateDateTime = System.DateTime.Today;
                        //entity.UpdateID = userid;

                        //_dbContext.Update(entity);
                        //await _dbContext.SaveChangesAsync();
                        //}
                        #endregion

                        //Random rnd = new Random();
                        var batchTransactionID = System.Guid.NewGuid();
                        DateTime historyCreateDateTime = System.DateTime.Now; 
                        Dictionary<string, string> historyCols = new Dictionary<string, string>();

                        //Inventory Histroy insertion
                        if (inventoryItemModel.ConditionId != entity.ConditionID)
                            historyCols.Add("ConditionID", entity.ConditionID.ToString());
                        if (inventoryItemModel.ProposalNumber != entity.ProposalNumber)
                            historyCols.Add("ProposalNumber", entity.ProposalNumber.ToString());
                        if (Convert.ToDecimal(inventoryItemModel.PoOrderNo) != entity.PoOrderNo)
                            historyCols.Add("PoOrderNo", entity.PoOrderNo.ToString());

                        var invItemHistory = historyCols.Select(x => new InventoryHistory()
                        {
                            BatchTransactionGUID = batchTransactionID,
                            OrderID = null,
                            EntityID = entity.InventoryItemID,
                            ApiName = apicallurl,
                            TableName = "InventoryItem",
                            ColumnName = x.Key,
                            OldValue = x.Value,
                            NewValue = (
                                     x.Key.Contains("ConditionID") ? inventoryItemModel.ConditionId.ToString() :
                                     x.Key.Contains("ProposalNumber") ? inventoryItemModel.ProposalNumber.ToString() :
                                     x.Key.Contains("PoOrderNo") ? inventoryItemModel.PoOrderNo.ToString() : ""
                                    ),
                            Description = x.Key + " Updated",
                            CreateDateTime = historyCreateDateTime,
                            CreateID = userid
                        });

                        await _dbContext.AddRangeAsync(invItemHistory);
                        await _dbContext.SaveChangesAsync();

                        //var inventoryHistory = new InventoryHistory();
                        //inventoryHistory.BatchTransactionGUID = System.Guid.NewGuid();
                        //inventoryHistory.OrderID = null;
                        //inventoryHistory.EntityID = entity.InventoryItemID;
                        //inventoryHistory.ApiName = apicallurl; // "editinventoryitem";
                        //inventoryHistory.TableName = "InventoryItem";
                        //inventoryHistory.ColumnName = "ConditionID";
                        //inventoryHistory.OldValue = entity.ConditionID.ToString();
                        //inventoryHistory.NewValue = inventoryItemModel.ConditionId.ToString();
                        //inventoryHistory.Description = "Condition Changed";
                        //inventoryHistory.CreateDateTime = System.DateTime.Now;
                        //inventoryHistory.CreateID = userid;

                        //await _dbContext.AddAsync(inventoryHistory);
                        //await _dbContext.SaveChangesAsync();



                        //InventoryItem Table Update code.
                        entity.ConditionID = inventoryItemModel.ConditionId;
                        entity.ProposalNumber = inventoryItemModel.ProposalNumber;
                        entity.PoOrderNo = Convert.ToDecimal(inventoryItemModel.PoOrderNo);

                        entity.UpdateDateTime = System.DateTime.Today;
                        entity.UpdateID = userid;

                        _dbContext.Update(entity);
                        await _dbContext.SaveChangesAsync();


                        await transaction.CommitAsync();

                        result = true;
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

        public async Task<List<ItemTypesModel>> GetItemTypes(int clientid)
        {
            try
            {
                var itemTypes = await _dbContext.ItemTypes.Where(i => i.ClientID == clientid || i.ClientID == 1 && i.ItemTypeID != 0)
                                        .Select(item => new ItemTypesModel { ItemTypeID = item.ItemTypeID, ItemTypeName = item.ItemTypeName })
                                        .Distinct().OrderBy(ord => ord.ItemTypeName).ToListAsync();
                              
                return itemTypes;
                //return itemTypes.OrderBy(ord => ord.ItemTypeName).ToList();

                //return await _dbContext.Inventories.Where(i=>!string.IsNullOrEmpty(i.Category) && i.ClientID == client_id).Select(cat=>cat.Category).Distinct().ToListAsync();
                //return await _dbContext.ItemTypes.Where(i => i.ClientID == client_id || i.ClientID == 1 && i.ItemTypeID != 0).Select(item => new ItemTypesModel {ItemTypeID = item.ItemTypeID,ItemTypeName = item.ItemTypeName }).Distinct().ToListAsync();

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private async Task<bool> CreateNewInstallationOrderWithItems(List<InventoryItem> inventoryItems,string itemcode,string description,int userid,int clientid)
        {
            bool result = false;
            try
            {
                var user = await GetUserDetails(userid);

                Order orderModel = new Order();

                orderModel.Email = user.email;
                orderModel.RequestForName = $"{user.first_name} {user.last_name}";
                orderModel.ClientID = clientid;
                orderModel.CreateID = userid;
                orderModel.RequestorID = userid;
                orderModel.CreateDateTime = System.DateTime.Now;
                orderModel.StatusID = (int)Enums.Status.OrdOpen;
                orderModel.OrderTypeID = (int)Enums.OrderType.NewInstallation;

                await _dbContext.AddAsync(orderModel);
                await _dbContext.SaveChangesAsync();

                int order_id = orderModel.OrderID;

                var orderItemList = inventoryItems.Select(x => new OrderItem()
                {
                    InventoryID = x.InventoryID,
                    InventoryItemID = x.InventoryItemID,
                    ItemCode = itemcode,
                    OrderID = order_id,
                    Qty = 1,
                    ClientID = clientid,
                    CreateID = userid,
                    CreateDateTime = System.DateTime.Now,
                    DestBuildingID = x.InventoryBuildingID,
                    DestFloorID = x.InventoryFloorID,
                    DestRoom = x.Room,
                    DepartmentCostCenter = "",
                    Comments = "",
                    //InstallDate = Convert.ToDateTime(item.InstDate),
                    StatusID = (int)Enums.Status.OrdOpen
                }).ToList();

                await _dbContext.AddRangeAsync(orderItemList);
                await _dbContext.SaveChangesAsync();

                result = true;

            }
            catch (Exception ex)
            {
                throw;
            }

            return result;
        }

        private async Task<Users> GetUserDetails(int user_id)
        {
            var users = await _requestContext.Users.Where(u => u.user_id == user_id).FirstOrDefaultAsync();

            return users;
        }

    }
}
