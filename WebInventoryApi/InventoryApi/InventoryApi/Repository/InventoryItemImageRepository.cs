using InventoryApi.DBContext;
using InventoryApi.DBModels;
using InventoryApi.Models;
using InventoryApi.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryApi.Repository
{
    public class InventoryItemImageRepository : IInventoryItemImageRepository
    {
        InventoryContext _dbContext;
        IConfiguration _configuration { get; }
        IAwsDownloadRepository _awsDownloadRepository { get; }

        public InventoryItemImageRepository(InventoryContext dbContext, IConfiguration configuration, IAwsDownloadRepository awsDownloadRepository)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            _awsDownloadRepository = awsDownloadRepository;
        }

        public async Task<List<InventoryItemImagesModel>> GetInventoryItemImages(int inv_id)
        {
            try
            {
                //var invMainImg = _dbContext.Inventories.Where(i => i.InventoryID == inv_id).
                //    Select(x => new InventoryItemImagesModel()
                //    {
                //        InventoryItemImageID = 0,
                //        InventoryItemID = 0,
                //        InventoryItemImageGuid = new Guid(),
                //        ImageName = x.MainImage,
                //        ImageUrl = _configuration.GetValue<string>("ImgUrl") + x.ClientID + "/",
                //        ClientID = 0
                //    }).ToList(); 

                //var invitemimg =  _dbContext.InventoryImages.Where(iii => _dbContext.InventoryItems.Where(x=>x.InventoryID == inv_id).Select(y=>y.InventoryItemID).Contains(iii.InventoryItemID)).ToList();
                var invitemimg = await _dbContext.InventoryImages.Where(iii => iii.InventoryID == inv_id).ToListAsync();

                List<InventoryItemImagesModel> inventoryItemImagesModel = new List<InventoryItemImagesModel>();

                //inventoryItemImagesModel = invMainImg.Select(x => new InventoryItemImagesModel()
                //{
                //    InventoryItemImageID = x.InventoryItemImageID,
                //    InventoryItemID = x.InventoryItemID,
                //    InventoryItemImageGuid = x.InventoryItemImageGuid,
                //    ImageName = x.ImageName,
                //    ImageUrl = x.ImageUrl,
                //    ClientID = x.ClientID
                //}).ToList();

                if (invitemimg.Count > 0)
                {
                    inventoryItemImagesModel = invitemimg.Select(x => new InventoryItemImagesModel()
                    {
                        InventoryItemImageID = x.InventoryImageID,
                        InventoryItemID = x.InventoryItemID,
                        InventoryItemImageGuid = x.ImageGUID,
                        ImageName = x.ImageName,
                        ImageUrl = x.ImageURL + "/" + x.ClientID + "/",
                        ClientID = x.ClientID,
                        BucketName = _configuration.GetValue<string>("AwsConfig:BuketName"),
                        ImagePath = $"{_configuration.GetValue<string>("AwsConfig:S3Folder")}/{x.ClientID}/images/{x.ImageName}"
                    }).ToList();
                }


                //foreach(var item in inventoryItemImagesModel)
                //{
                //    var imgBase64 = await _awsDownloadRepository.DownloadFileAsync(item.BucketName, item.ImagePath);
                //    inventoryItemImagesModel.Where(img => img.InventoryItemImageID == item.InventoryItemImageID).Select(s => s.ImageBase64 = imgBase64).ToList();
                //}

                //var invitemimg =  _dbContext.InventoryItemImages.Where(iii => _dbContext.InventoryItems.Where(x=>x.InventoryID == inv_id).Select(y=>y.InventoryItemID).Contains(iii.InventoryItemID)).ToList();

                //var invitem = _dbContext.InventoryItems.Where(ii => ii.InventoryID == inv_id).ToList();

                //var grpInvItem = invitem.GroupBy(x => new { x.Building, x.Floor, x.Room, x.Condition }).Select(g => new { g.Key.Building, g.Key.Floor, g.Key.Room, g.Key.Condition, qtyCount = g.Count() }).ToList();

                //List<InventoryItemImagesModel> inventoryItemImagesModel = new List<InventoryItemImagesModel>();

                //inventoryItemImagesModel = invitemimg.Select(x => new InventoryItemImagesModel()
                //{
                //    InventoryItemImageID = x.InventoryItemImageID,
                //    InventoryItemID = x.InventoryItemID,
                //    InventoryItemImageGuid = x.InventoryItemImageGuid,
                //    ImageName = x.ImageName,
                //    ImageUrl = x.ImageUrl,
                //    ClientID = x.ClientID

                //    //inv_item_img_id = x.InventoryItemImageID,
                //    //inv_item_id = x.InventoryItemID,
                //    //inv_item_img_guid = x.InventoryItemImageGuid,
                //    //image_name = x.ImageName,
                //    //image_url = x.ImageUrl,
                //    //client_id = x.ClientID
                //}).ToList();



                //foreach (var item in grpInvItem)
                //{
                //    int extInvItemId = invitem.Where(x => x.Building == item.Building && x.Floor == item.Floor && x.Room == item.Room && x.Condition == item.Condition).FirstOrDefault().InventoryItemID;
                //    string strLocation = $"{item.Building} : {item.Floor} : Loc {item.Room} : Cond {item.Condition} : Qty {item.qtyCount}";

                //    inventoryItemImagesModel.Where(x => x.InventoryItemID == extInvItemId).Select(s => { s.Location = strLocation; return s; }).ToList();
                //}


                return inventoryItemImagesModel;

                #region oldcode
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
                #endregion
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<InventoryItemImagesModel>> GetInventoryItemImagesForCondition(int inv_item_id, int conditionid, int inventoryid)
        {
            try
            {
                var invitemimgentity = await _dbContext.InventoryImages
                                .Where(iii => iii.InventoryItemID == inv_item_id && iii.ConditionID == conditionid)
                                .ToListAsync();

                if(invitemimgentity == null || invitemimgentity.Count == 0)
                {
                    invitemimgentity = await _dbContext.InventoryImages
                                        .Where(img => img.InventoryID == inventoryid && img.ConditionID == conditionid)
                                        .ToListAsync();

                    //need to confirm with team this block of code is required or not.
                    if (invitemimgentity == null || invitemimgentity.Count == 0)
                    {
                        invitemimgentity = await _dbContext.InventoryImages
                                        .Where(img => img.InventoryID == inventoryid && img.InventoryItemID == null)
                                        .ToListAsync();
                    }

                }

                List<InventoryItemImagesModel> inventoryItemImagesModel = new List<InventoryItemImagesModel>();

                inventoryItemImagesModel = invitemimgentity.Select(x => new InventoryItemImagesModel()
                {
                    InventoryItemImageID = x.InventoryImageID,
                    InventoryItemID = x.InventoryItemID,
                    InventoryItemImageGuid = x.ImageGUID,
                    ImageName = x.ImageName,
                    ImageUrl = x.ImageURL + "/" + x.ClientID + "/",
                    ClientID = x.ClientID,
                    BucketName = _configuration.GetValue<string>("AwsConfig:BuketName"),
                    ImagePath = $"{_configuration.GetValue<string>("AwsConfig:S3Folder")}/{x.ClientID}/images/{x.ImageName}"

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
