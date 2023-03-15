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
    public class WarrantyRepository: IWarrantyRepository
    {
        InventoryContext _dbContext;

        IEmailNotificationRepository _emailNotificationRepository { get; }
        IHDRequestRepository _hdRequestRepository { get; }
        private readonly ILoggerManagerRepository _logger;
        IConfiguration _configuration { get; }

        public WarrantyRepository(InventoryContext dbContext
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
        public async Task<int> InsertWarrantyRequest(string apicallurl, List<GenericModel> listInventoryItemWarrantyModels)
        {
            int result = 0;

            using (IDbContextTransaction transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    int ordId = await InsertInventoryWarrantyOrder(listInventoryItemWarrantyModels[0].Email,
                                                                    listInventoryItemWarrantyModels[0].RequestForName,
                                                                    listInventoryItemWarrantyModels[0].ClientID,
                                                                    listInventoryItemWarrantyModels[0].RequestorId);

                    //foreach (var warrantyItem in listInventoryItemWarrantyModels)
                    //{
                    if (ordId > 0)
                    {
                        var invorderitemresult = await InsertInventoryWarrantyOrderItem(ordId, apicallurl, listInventoryItemWarrantyModels);
                        _logger.LogInfo("Inserted Order Item Successfully");

                        //var requestId = await SendWarrantyRequestEmailNotification(warrantyItem);
                        var requestId = await SendWarrantyRequestEmailNotification(listInventoryItemWarrantyModels);
                        _logger.LogInfo($"Send Order Email Notification and request id is-{requestId}");

                        bool flagresult = await UpdateInventoryItemWarrantyRequestID(requestId, ordId, listInventoryItemWarrantyModels);
                        //result = await UpdateInventoryItemWarrantyRequestID(requestId, warrantyItem.InventoryItemID);

                        await transaction.CommitAsync();
                        _logger.LogInfo($"Transaction Commited Successfully for InventoryId-> {listInventoryItemWarrantyModels[0].InventoryID}");

                        result = requestId;
                    }
                    //}
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    _logger.LogInfo($"Transaction Rollback for InventoryId-> {listInventoryItemWarrantyModels[0].InventoryID}");
                    throw;
                }

            }
            return result;
        }

        private async Task<int> InsertInventoryWarrantyOrder(string email, string reqname, int clientid, int userid)
        {

            int order_id = 0;
            try
            {
                Order orderModel = new Order();

                orderModel.Email = email;
                orderModel.RequestForName = reqname;
                orderModel.ClientID = clientid;
                orderModel.CreateID = userid;
                orderModel.RequestorID = userid;
                orderModel.CreateDateTime = System.DateTime.Now;
                orderModel.OrderTypeID = (int)Enums.OrderType.Warranty;
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

        private async Task<bool> InsertInventoryWarrantyOrderItem(int order_id, string apicallurl, List<GenericModel> itemWarrantyModel)
        {
            bool result = false;
            try
            {
                var batchTransactionID = System.Guid.NewGuid();
                DateTime historyCreateDateTime = System.DateTime.Now;

                foreach (var item in itemWarrantyModel)
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
                                                //Qty = item.Qty,
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
                    NewValue = ((int)Enums.Status.WarrantyService).ToString(),
                    Description = "Warranty Requset Created",
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
        private async Task<int> SendWarrantyRequestEmailNotification(List<GenericModel> inventoryItemWarrantyModel)
        {
            try
            {
                StringBuilder sbFullEmailBody = new StringBuilder();
                foreach (var item in inventoryItemWarrantyModel)
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

                    sbEmailBody = $"<html><pre>A new {item.ClientName} warranty request came in :\nEmail:{item.Email}\n" +
                            $"Request for :{item.RequestForName}\n" +
                            $"Comments:{item.Comment}</pre>" +
                            $"{sb}</html>";

                    sbFullEmailBody.Append(sbEmailBody);

                }




                _emailNotificationRepository.SendGenericEmail(sbFullEmailBody.ToString(), inventoryItemWarrantyModel[0].RequestForName, inventoryItemWarrantyModel[0].Email, inventoryItemWarrantyModel[0].ClientName, Enums.RequestType.Warranty.ToString());
                ////_emailNotificationRepository.SendGmailEmail(body, inventoryItemWarrantyModel.ReqName, inventoryItemWarrantyModel.Email, inventoryItemWarrantyModel.ClientName);


                //var hdticketresult = await _hdTicketRepository.CreateWarrentyRequestHDTicket(inventoryItemWarrantyModel, body);
                var hdticketresult = await _hdRequestRepository.CreateRequestHDTicket(inventoryItemWarrantyModel,Enums.RequestType.Warranty.ToString());
                return hdticketresult;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        private async Task<bool> UpdateInventoryItemWarrantyRequestID(int requestId, int orderid, List<GenericModel> inventoryItemWarrantyModels)
        {
            bool result = false;
            try
            {
                foreach (var item in inventoryItemWarrantyModels)
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
                            entity = await _dbContext.InventoryItems.Where(ii => ii.InventoryItemID == item.InventoryItemID
                                                                    //&& ii.InventoryBuildingID == item.BuildingId
                                                                    //&& ii.InventoryFloorID == item.FloorId
                                                                    //&& ii.Room == item.Room
                                                                    //&& ii.ConditionID == item.ConditionId
                                                                    )
                                                                    .ToListAsync();
                        }
                    }



                    if (entity != null && entity.Count > 0)
                    {
                        entity.ForEach(e =>
                        {
                            e.WarrantyRequestID = requestId;
                            // e.DisplayOnSite = false;
                            e.StatusID = (int)Enums.Status.WarrantyService;
                        });

                        //entity.WarrantyRequestID = requestId;
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
