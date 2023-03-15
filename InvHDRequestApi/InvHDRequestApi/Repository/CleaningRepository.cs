using InvHDRequestApi.DBContext;
using InvHDRequestApi.DBModels.InventoryDBModels;
using InvHDRequestApi.Helpers;
using InvHDRequestApi.Models;
using InvHDRequestApi.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvHDRequestApi.Repository
{
    public class CleaningRepository : ICleaningRepository
    {
        InventoryContext _dbContext;

        IEmailNotificationRepository _emailNotificationRepository { get; }
        IHDRequestRepository _hdRequestRepository { get; }
        private readonly ILoggerManagerRepository _logger;
        IConfiguration _configuration { get; }

        public CleaningRepository(InventoryContext dbContext
                                      , IHDRequestRepository hdRequestRepository
                                      , IEmailNotificationRepository emailNotificationRepository
                                      , ILoggerManagerRepository logger
                                      , IConfiguration configuration
                                    )
        {
            _dbContext = dbContext;
            _hdRequestRepository = hdRequestRepository;
            _emailNotificationRepository = emailNotificationRepository;
            _logger = logger;
            _configuration = configuration;
        }
        public async Task<int> InsertCleaningRequest(string apicallurl, List<GenericModel> listInventoryItemCleaningModels)
        {
            int result = 0;

            using (IDbContextTransaction transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    int ordId = await InsertInventoryCleaningOrder(listInventoryItemCleaningModels[0].Email,
                                                                    listInventoryItemCleaningModels[0].RequestForName,
                                                                    listInventoryItemCleaningModels[0].ClientID,
                                                                    listInventoryItemCleaningModels[0].RequestorId);

                    //foreach (var CleaningItem in listInventoryItemCleaningModels)
                    //{
                    if (ordId > 0)
                    {
                        var invorderitemresult = await InsertInventoryCleaningOrderItem(ordId, apicallurl, listInventoryItemCleaningModels);
                        _logger.LogInfo("Inserted Cleaning Request Item Successfully");

                        //var requestId = await SendCleaningRequestEmailNotification(CleaningItem);
                        var requestId = await SendCleaningRequestEmailNotification(listInventoryItemCleaningModels);
                        _logger.LogInfo($"Send Cleaning Request Email Notification and request id is-{requestId}");

                        bool flagresult = await UpdateInventoryItemCleaningRequestID(requestId, ordId, listInventoryItemCleaningModels);
                        //result = await UpdateInventoryItemCleaningRequestID(requestId, CleaningItem.InventoryItemID);

                        await transaction.CommitAsync();
                        _logger.LogInfo($"Transaction Commited Successfully for InventoryId-> {listInventoryItemCleaningModels[0].InventoryID}");

                        result = requestId;
                    }
                    //}
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    _logger.LogInfo($"Transaction Rollback for InventoryId-> {listInventoryItemCleaningModels[0].InventoryID}");
                    throw;
                }

            }
            return result;
        }

        private async Task<int> InsertInventoryCleaningOrder(string email, string reqname, int clientid, int userid)
        {

            int order_id = 0;
            try
            {
                Order orderModel = new Order();

                orderModel.Email = email;
                orderModel.RequestForName = reqname;
                orderModel.ClientID = clientid;
                orderModel.RequestorID = userid;
                orderModel.CreateID = userid;
                orderModel.CreateDateTime = System.DateTime.Now;
                orderModel.OrderTypeID = (int)Enums.OrderType.Clean;
                orderModel.StatusID = (int)Enums.Status.OrdOpen;

                await _dbContext.AddAsync(orderModel);
                await _dbContext.SaveChangesAsync();

                order_id = orderModel.OrderID;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception occured due to call InsertInventoryOrder Method=>{ex.Message} \n {ex.StackTrace}");

                throw;
            }


            return order_id;

        }

        private async Task<bool> InsertInventoryCleaningOrderItem(int order_id, string apicallurl, List<GenericModel> itemCleaningModel)
        {
            bool result = false;
            try
            {
                var batchTransactionID = System.Guid.NewGuid();
                DateTime historyCreateDateTime = System.DateTime.Now;

                foreach (var item in itemCleaningModel)
                {
                    List<OrderItem> orderItemList = new List<OrderItem>();

                    if (item.ChildInventoryItemModels != null && item.ChildInventoryItemModels.Count > 0)
                    {
                        orderItemList = item.ChildInventoryItemModels.Where(ch => ch.IsSelected == true).Select(x => new OrderItem()
                        {
                            InventoryID = x.InventoryID,
                            InventoryItemID = x.InventoryItemID,
                            ItemCode = item.ItemCode,
                            OrderID = order_id,
                            Qty = x.Qty,
                            ClientID = item.ClientID,
                            CreateID = item.RequestorId,
                            CreateDateTime = System.DateTime.Now,
                            DestBuildingID = null,
                            DestFloorID = 0,
                            DestRoom = "",
                            DepartmentCostCenter = "",
                            Comments = item.Comment,
                            InstallDate = System.DateTime.Now,
                            StatusID = (int)Enums.Status.OrdOpen
                        }).ToList();

                        await _dbContext.AddRangeAsync(orderItemList);
                        await _dbContext.SaveChangesAsync();

                        var hisResult = await InsertInventoryHistory(orderItemList, batchTransactionID, historyCreateDateTime, apicallurl);
                    }
                    else
                    {
                        if (item.PullQty > 1)
                        {
                            orderItemList = await _dbContext.InventoryItems.Where(ii => ii.InventoryItemID >= item.InventoryItemID
                                                                            //&& ii.AddedToCartItem
                                                                            && ii.InventoryBuildingID == item.InventoryBuildingID
                                                                            && ii.InventoryFloorID == item.InventoryFloorID
                                                                            && ii.Room == item.Room
                                                                            && ii.ConditionID == item.ConditionId)
                                                                         .Take(item.PullQty)
                                                                         .Select(x => new OrderItem()
                                                                         {
                                                                             InventoryID = x.InventoryID,
                                                                             InventoryItemID = x.InventoryItemID,
                                                                             ItemCode = item.ItemCode,
                                                                             OrderID = order_id,
                                                                             Qty = 1,
                                                                             ClientID = item.ClientID,
                                                                             CreateID = item.RequestorId,
                                                                             CreateDateTime = System.DateTime.Now,
                                                                             DestBuildingID = null,
                                                                             DestFloorID = 0,
                                                                             DestRoom = "",
                                                                             DepartmentCostCenter = "",
                                                                             Comments = item.Comment,
                                                                             InstallDate = System.DateTime.Now,
                                                                             StatusID = (int)Enums.Status.OrdOpen
                                                                         }).ToListAsync();

                            await _dbContext.AddRangeAsync(orderItemList);
                            await _dbContext.SaveChangesAsync();

                            var hisResult = await InsertInventoryHistory(orderItemList, batchTransactionID, historyCreateDateTime, apicallurl);

                        }
                        else
                        {
                            orderItemList = await _dbContext.InventoryItems.Where(ii => ii.InventoryItemID == item.InventoryItemID
                                                                            ////&& ii.AddedToCartItem
                                                                            //&& ii.InventoryBuildingID == item.InventoryBuildingID
                                                                            //&& ii.InventoryFloorID == item.InventoryFloorID
                                                                            //&& ii.Room == item.Room
                                                                            //&& ii.ConditionID == item.ConditionId
                                                                            )
                                            .Select(x => new OrderItem()
                                            {
                                                InventoryID = x.InventoryID,
                                                InventoryItemID = x.InventoryItemID,
                                                ItemCode = item.ItemCode,
                                                OrderID = order_id,
                                                Qty = item.PullQty > 0 ? item.PullQty : 1,
                                                ClientID = item.ClientID,
                                                CreateID = item.RequestorId,
                                                CreateDateTime = System.DateTime.Now,
                                                DestBuildingID = null,
                                                DestFloorID = 0,
                                                DestRoom = "",
                                                DepartmentCostCenter = "",
                                                Comments = item.Comment,
                                                InstallDate = System.DateTime.Now,
                                                StatusID = (int)Enums.Status.OrdOpen
                                            }).ToListAsync();

                            await _dbContext.AddRangeAsync(orderItemList);
                            await _dbContext.SaveChangesAsync();

                            var hisResult = await InsertInventoryHistory(orderItemList, batchTransactionID, historyCreateDateTime, apicallurl);

                        }

                    }
                }
                result = true;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception occured due to call InsertInventoryOrderItem Method=>{ex.Message} \n {ex.StackTrace}");
                throw;
            }
            return result;
        }

        private async Task<bool> InsertInventoryHistory(List<OrderItem> orderItemList, Guid batchtransid, DateTime batchtransdate, string apicallurl)
        {
            bool result = false;
            try
            {
                ////insert InventoryHistory records belong to orderid and InventoryItemID
                var invHistory = orderItemList.Select(x => new InventoryHistory()
                {
                    BatchTransactionGUID = batchtransid,
                    OrderID = x.OrderID,
                    EntityID = x.InventoryItemID,
                    ApiName = apicallurl,
                    TableName = "InventoryItem",
                    ColumnName = "SatusID",
                    OldValue = "5",
                    NewValue = ((int)Enums.Status.Maintenance).ToString(),
                    Description = "Cleaning Requset Created",
                    CreateDateTime = batchtransdate,
                    CreateID = x.CreateID
                });

                await _dbContext.AddRangeAsync(invHistory);
                await _dbContext.SaveChangesAsync();

                result = true;
            }
            catch (Exception ex)
            {
                throw;
            }

            return result;
        }
        private async Task<int> SendCleaningRequestEmailNotification(List<GenericModel> inventoryItemCleaningModel)
        {
            try
            {
                StringBuilder sbFullEmailBody = new StringBuilder();
                foreach (var item in inventoryItemCleaningModel)
                {
                    StringBuilder sb = new StringBuilder();
                    string sbEmailBody = "";

                    sb.Append("<table cellspacing=0 cellpadding=5 border=1>");

                    //sb.Append("<tr style='border:1px solid gray;'>");
                    sb.Append("<tr>");

                    sb.Append("<td><img src='" + _configuration.GetValue<string>("EmailConfigs:EmailImgUrl") + item.ClientID + "/images/" + item.InventoryImageName + "' height='80px' width='80px'/></td>");

                    sb.Append("<td>" + item.Qty + "</td>");
                    sb.Append("<td>" + item.ItemCode + "</td>");
                    sb.Append("<td>" + item.Description + "</td>");
                    sb.Append("</tr></table>");

                    sbEmailBody = $"<html><pre>A new {item.ClientName} Cleaning request came in :\nEmail:{item.Email}\n" +
                            $"Request for :{item.RequestForName}\n" +
                            $"Comments:{item.Comment}</pre>" +
                            $"{sb}</html>";

                    sbFullEmailBody.Append(sbEmailBody);

                }

                _emailNotificationRepository.SendGenericEmail(sbFullEmailBody.ToString(), inventoryItemCleaningModel[0].RequestForName, inventoryItemCleaningModel[0].Email, inventoryItemCleaningModel[0].ClientName, Enums.RequestType.Cleaning.ToString());

                var hdticketresult = await _hdRequestRepository.CreateRequestHDTicket(inventoryItemCleaningModel,Enums.RequestType.Cleaning.ToString());
                return hdticketresult;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        private async Task<bool> UpdateInventoryItemCleaningRequestID(int requestId, int orderid, List<GenericModel> inventoryItemCleaningModels)
        {
            bool result = false;
            try
            {
                foreach (var item in inventoryItemCleaningModels)
                {

                    List<InventoryItem> entity = new List<InventoryItem>();

                    if (item.ChildInventoryItemModels != null && item.ChildInventoryItemModels.Count > 0)
                    {
                        var childInvItems = item.ChildInventoryItemModels.Where(ch => ch.IsSelected == true).Select(s => s.InventoryItemID).ToList();

                        foreach (int invitemid in childInvItems)
                        {
                            var invitem = await _dbContext.InventoryItems.Where(ii => ii.InventoryItemID == invitemid).FirstOrDefaultAsync();
                            entity.Add(invitem);
                        }
                    }
                    else
                    {
                        if (item.PullQty > 1)
                        {
                            entity = await _dbContext.InventoryItems.Where(ii => ii.InventoryItemID >= item.InventoryItemID
                                                                    && ii.InventoryBuildingID == item.InventoryBuildingID
                                                                    && ii.InventoryFloorID == item.InventoryFloorID
                                                                    && ii.Room == item.Room
                                                                    && ii.ConditionID == item.ConditionId)
                                                                    .Take(item.PullQty)
                                                                    .ToListAsync();
                        }
                        else
                        {
                            entity = await _dbContext.InventoryItems.Where(ii => ii.InventoryItemID == item.InventoryItemID).ToListAsync();
                        }
                    }



                    if (entity != null && entity.Count > 0)
                    {
                        entity.ForEach(e =>
                        {
                            //e.CleaningRequestID = requestId;
                            // e.DisplayOnSite = false;
                            e.StatusID = (int)Enums.Status.Maintenance;
                        });

                        //entity.CleaningRequestID = requestId;
                        //entity.DisplayOnSite = false;
                        //entity.StatusID = (int)Enums.Status.Inactive;

                        //_dbContext.Update(entity);
                        _dbContext.UpdateRange(entity);
                        await _dbContext.SaveChangesAsync();
                    }
                }

                var orderentity = await _dbContext.Orders.Where(o => o.OrderID == orderid).FirstOrDefaultAsync();

                if (orderentity != null)
                {
                    orderentity.RequestID = requestId;

                    _dbContext.Update(orderentity);
                    await _dbContext.SaveChangesAsync();
                }

                result = true;

            }
            catch (Exception ex)
            {
                throw;
            }

            return result;
        }

    }
}
