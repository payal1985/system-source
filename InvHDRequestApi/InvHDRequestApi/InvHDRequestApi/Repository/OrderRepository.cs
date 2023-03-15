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
    public class OrderRepository : IOrderRepository
    {
        InventoryContext _dbContext;
        SSIDBContext _ssidbContext;

        IConfiguration _configuration { get; }
        IEmailNotificationRepository _emailNotificationRepository { get; }
        IHDRequestRepository _hdRequestRepository { get; }

      //  IAws3Repository _aws3Repository { get; }

        private readonly ILoggerManagerRepository _logger;
        public OrderRepository(InventoryContext dbContext, IConfiguration configuration
                                            , IEmailNotificationRepository emailNotificationRepository
                                            , IHDRequestRepository hdRequestRepository
                                            //, IAws3Repository aws3Repository
                                            , ILoggerManagerRepository logger
                                            , SSIDBContext ssidbContext
            )
        {
            _logger = logger;
            _dbContext = dbContext;
            _configuration = configuration;
            _emailNotificationRepository = emailNotificationRepository;
            _hdRequestRepository = hdRequestRepository;
           // _aws3Repository = aws3Repository;
           // Common._dbContext = dbContext;
           // _requestContext = requestContext;
        }

        public string GetConnectionString()
        {
            string connString = _configuration.GetConnectionString("SSIDB");
            return connString;
        }

        public async Task<bool> CreateOrderRequest(string apicallurl, OrderModel model)
        {
            bool result = false;
            using (IDbContextTransaction transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    Order orderModel = new Order();

                    orderModel.Email = model.RequestorEmail;
                    orderModel.RequestForName = model.RequestorForName;                    
                    orderModel.ClientID = model.CartItem[0].ClientID;
                    orderModel.CreateID = model.CartItem[0].RequestorId;
                    orderModel.RequestorID = model.CartItem[0].RequestorId;
                    orderModel.CreateDateTime = System.DateTime.Now;
                    orderModel.StatusID = (int)Enums.Status.OrdOpen;
                    orderModel.OrderTypeID = (int)Enums.OrderType.Relocate;
                    orderModel.ClientNote = model.ClientNote;

                    await _dbContext.AddAsync(orderModel);
                    await _dbContext.SaveChangesAsync();

                    int order_id = orderModel.OrderID;

                    var orderItemModel = model.CartItem;

                    var invorderitemresult = await InsertInventoryOrderItem(order_id, apicallurl, orderItemModel);
                    _logger.LogInfo("Inserted Order Item Successfully");

                    //new Histroy Tracker flow introduced on Oct 25,2022 . so, commented thease 2 lines of code...
                    //var flagrl = await InsertInventoryItemStatusHistory(model);
                    //_logger.LogInfo("Inserted Inventory Item Status History");

                    var flaginvitem = await UpdateInventoryItem(model, orderItemModel);
                    _logger.LogInfo($"Updated Inventory Item result is->{flaginvitem}");

                    if (flaginvitem)
                    {
                        int requestId = await SendOrderEmailNotification(model);
                        _logger.LogInfo($"Send Order Email Notification and request id is-{requestId}");

                        bool updateOrder = await MapRequestIdToOrder(order_id, requestId);

                        await transaction.CommitAsync();
                        _logger.LogInfo($"Transaction Commited Successfully for InventoryId-> {model.CartItem[0].InventoryID}");

                        result = true;
                    }
                    else
                    {
                        await transaction.RollbackAsync();
                        _logger.LogInfo($"Transaction Rollback for InventoryId-> {model.CartItem[0].InventoryID}");
                        result = false;
                    }

                }
                catch (Exception ex)
                {
                    _logger.LogError($"Exception occured due to call InsertInventoryOrder Method=>{ex.Message} \n {ex.StackTrace}");

                    await transaction.RollbackAsync();
                    throw;
                }
            }

            return result;
        }

        private async Task<bool> InsertInventoryOrderItem(int order_id,string apicallurl, List<Cart> cart)
        {
            bool result = false;
            try
            {
                var batchTransactionID = System.Guid.NewGuid();
                DateTime historyCreateDateTime = System.DateTime.Now;

                foreach (var item in cart)
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
                            DestBuildingID = item.DestInventoryBuildingID,
                            DestFloorID = item.DestInventoryFloorID,
                            DestRoom = item.DestRoom,
                            DepartmentCostCenter = item.DepartmentCostCenter,
                            Comments = item.Comments,
                            InstallDate = Convert.ToDateTime(item.InstDate),
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
                                                                                 && ii.AddedToCartItem
                                                                                 && ii.InventoryBuildingID == item.BuildingId
                                                                                 && ii.InventoryFloorID == item.FloorId
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
                                                 DestBuildingID = item.DestInventoryBuildingID,
                                                 DestFloorID = item.DestInventoryFloorID,
                                                 DestRoom = item.DestRoom,
                                                 DepartmentCostCenter = item.DepartmentCostCenter,
                                                 Comments = item.Comments,
                                                 InstallDate = Convert.ToDateTime(item.InstDate),
                                                 StatusID = (int)Enums.Status.OrdOpen
                                             }).ToListAsync();

                            await _dbContext.AddRangeAsync(orderItemList);
                            await _dbContext.SaveChangesAsync();

                            var hisResult = await InsertInventoryHistory(orderItemList, batchTransactionID, historyCreateDateTime, apicallurl);
                        }
                        else
                        {
                            orderItemList = await _dbContext.InventoryItems.Where(ii => ii.InventoryItemID == item.InventoryItemID && ii.AddedToCartItem && ii.ConditionID == item.ConditionId)
                                            .Take(item.PullQty)
                                            .Select(x => new OrderItem()
                                            {
                                                InventoryID = x.InventoryID,
                                                InventoryItemID = x.InventoryItemID,
                                                ItemCode = item.ItemCode,
                                                OrderID = order_id,
                                                //Qty = item.PullQty,
                                                Qty = item.PullQty > 0 ? item.PullQty : 1,
                                                ClientID = item.ClientID,
                                                CreateID = item.RequestorId,
                                                CreateDateTime = System.DateTime.Now,
                                                DestBuildingID = item.DestInventoryBuildingID,
                                                DestFloorID = item.DestInventoryFloorID,
                                                DestRoom = item.DestRoom,
                                                DepartmentCostCenter = item.DepartmentCostCenter,
                                                Comments = item.Comments,
                                                InstallDate = Convert.ToDateTime(item.InstDate),
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
        private async Task<bool> UpdateInventoryItem(OrderModel ordModel, List<Cart> cart)
        {
            try
            {

                foreach (var item in cart)
                {
                    if (item.ChildInventoryItemModels != null && item.ChildInventoryItemModels.Count > 0)
                    {
                        var cartentity = item.ChildInventoryItemModels.Where(ch => ch.IsSelected == true).ToList();

                        if (cartentity != null)
                        {
                            List<InventoryItem> entity = new List<InventoryItem>();

                            foreach (var invitem in cartentity)
                            {
                                var invitementity = await _dbContext.InventoryItems.Where(ii => ii.InventoryItemID == invitem.InventoryItemID && ii.AddedToCartItem).FirstOrDefaultAsync();

                                entity.Add(invitementity);
                            }

                            if (entity != null && entity.Count > 0 && entity.Count.Equals(item.PullQty))
                            {

                                entity.ForEach(x =>
                                {
                                    //x.InventoryBuildingID = item.DestInventoryBuildingID;
                                    //x.InventoryFloorID = item.DestInventoryFloorID;
                                    //x.Room = item.DestRoom;
                                    x.UpdateID = ordModel.CartItem.FirstOrDefault().RequestorId;
                                    x.UpdateDateTime = System.DateTime.Now;
                                    x.StatusID = (int)Enums.Status.Reserved;
                                    x.AddedToCartItem = false;
                                });

                                _dbContext.UpdateRange(entity);
                                await _dbContext.SaveChangesAsync();

                                _logger.LogInfo($"entity is updated with required columns and changed flag to false in AddedToCartItem column");

                            }
                            else if (!entity.Count.Equals(item.PullQty))
                            {
                                List<InventoryItem> reverseEntity = new List<InventoryItem>();
                                reverseEntity = entity;


                                _logger.LogInfo($"reverseEntity count is->{reverseEntity.Count} and pullqty is->{item.PullQty}");

                                if (reverseEntity != null && reverseEntity.Count > 0)
                                {
                                    reverseEntity.ForEach(x =>
                                    {
                                        x.AddedToCartItem = true;

                                    });
                                    _dbContext.UpdateRange(reverseEntity);
                                    await _dbContext.SaveChangesAsync();

                                    _logger.LogInfo($"reverseEntity is updated with flag to true in AddedToCartItem column");
                                }
                                return false;
                            }
                        }
                    }
                    else
                    {
                        List<InventoryItem> entity = new List<InventoryItem>();
                        //entity = await _dbContext.InventoryItems.Where(ii => ii.InventoryItemID == item.InventoryItemID && ii.AddedToCartItem).ToListAsync();

                        if (item.PullQty > 1)
                        {
                            entity = await _dbContext.InventoryItems.Where(ii => ii.InventoryID == item.InventoryID && ii.InventoryItemID >= item.InventoryItemID
                                                                            && ii.AddedToCartItem
                                                                            && ii.InventoryBuildingID == item.BuildingId
                                                                            && ii.InventoryFloorID == item.FloorId
                                                                            && ii.Room == item.Room
                                                                            && ii.ConditionID == item.ConditionId)
                                                                     .Take(item.PullQty).ToListAsync();


                        }
                        else
                            entity = await _dbContext.InventoryItems.Where(ii => ii.InventoryID == item.InventoryID && ii.ConditionID == item.ConditionId && ii.InventoryItemID == item.InventoryItemID && ii.AddedToCartItem).Take(item.PullQty).ToListAsync();

                        _logger.LogInfo($"entity count is->{entity.Count} and pullqty is->{item.PullQty}");

                        if (entity != null && entity.Count > 0 && entity.Count.Equals(item.PullQty))
                        {

                            entity.ForEach(x =>
                            {
                                x.UpdateID = ordModel.CartItem.FirstOrDefault().RequestorId;
                                x.UpdateDateTime = System.DateTime.Now;
                                x.StatusID = (int)Enums.Status.Reserved;
                                x.AddedToCartItem = false;
                            });

                            _dbContext.UpdateRange(entity);
                            await _dbContext.SaveChangesAsync();

                            _logger.LogInfo($"entity is updated with required columns and changed flag to false in AddedToCartItem column");

                        }
                        else if (!entity.Count.Equals(item.PullQty))
                        {
                            List<InventoryItem> reverseEntity = new List<InventoryItem>();
                            //reverseEntity = await _dbContext.InventoryItems.Where(ii => ii.InventoryItemID == item.InventoryItemID).Take(item.PullQty).ToListAsync();

                            if (item.PullQty > 1)
                            {
                                reverseEntity = await _dbContext.InventoryItems.Where(ii => ii.InventoryID == item.InventoryID
                                                                                    && ii.InventoryItemID >= item.InventoryItemID
                                                                                    && ii.InventoryBuildingID == item.BuildingId
                                                                                    && ii.InventoryFloorID == item.FloorId
                                                                                    && ii.Room == item.Room
                                                                                    && ii.ConditionID == item.ConditionId)
                                                                                    .Take(item.PullQty).ToListAsync();

                            }
                            else
                                reverseEntity = await _dbContext.InventoryItems.Where(ii => ii.InventoryID == item.InventoryID && ii.ConditionID == item.ConditionId && ii.InventoryItemID == item.InventoryItemID).Take(item.PullQty).ToListAsync();

                            _logger.LogInfo($"reverseEntity count is->{reverseEntity.Count} and pullqty is->{item.PullQty}");

                            if (reverseEntity != null && reverseEntity.Count > 0)
                            {
                                reverseEntity.ForEach(x =>
                                {
                                    x.AddedToCartItem = true;

                                });
                                _dbContext.UpdateRange(reverseEntity);
                                await _dbContext.SaveChangesAsync();

                                _logger.LogInfo($"reverseEntity is updated with flag to true in AddedToCartItem column");

                            }
                            return false;
                        }
                    }

                }
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception occured due to call UpdateInventoryItem Method=>{ex.Message} \n {ex.StackTrace}");
                throw;
            }
        }

        private async Task<int> SendOrderEmailNotification(OrderModel model)
        {
            int result;
            try
            {
                StringBuilder sbFullEmailBody = new StringBuilder();

                //StringBuilder sb = new StringBuilder();
                //StringBuilder sbEmailBody = new StringBuilder();

                //StringBuilder sbDestBuilding = new StringBuilder();
                //StringBuilder sbDestFloor = new StringBuilder();
                //StringBuilder sbDestRoom = new StringBuilder();
                //StringBuilder sbInstDate = new StringBuilder();
                //StringBuilder sbComment = new StringBuilder();

                //int index = 1;
                foreach (var item in model.CartItem)
                {
                    StringBuilder sb = new StringBuilder();
                    string sbEmailBody = "";

                    //sb.Append("<table style='cellspacing:0; cellpadding:5; border:1px solid gray;'>");
                    sb.Append("<table cellspacing=0 cellpadding=5 border=1>");

                    //sb.Append("<tr style='border:1px solid gray;'>");
                    sb.Append("<tr>");

                    ////sb.Append("<td><img src='" + imgpath + item.inv_image_name + "' height='100px' width='100px'/></td>");
                    ////sb.Append("<td style='border:1px solid gray;'><img src='http://dev.systemsource.com/static/RL/MicrosoftImageTest.pl?p_image_ID=2' height='100px' width='100px'/></td>");
                    //sb.Append("<td style='border:1px solid gray;'><img src='"+item.inv_image_url+"' height='100px' width='100px'/></td>");

                    //sb.Append("<td style='border:1px solid gray;'>" + item.pullqty + "</td>");
                    //sb.Append("<td style='border:1px solid gray;'>" + item.item_code + "</td>");
                    //sb.Append("<td style='border:1px solid gray;'>" + item.description + "</td>");
                    // sb.Append("<td><img src='http://dev.systemsource.com/static/RL/MicrosoftImageTest.pl?p_image_ID=2' height='100px' width='100px'/></td>");
                    sb.Append("<td><img src='" + _configuration.GetValue<string>("EmailConfigs:EmailImgUrl") + item.ClientID + "/images/" + item.InventoryImageName + "' height='100px' width='100px'/></td>");

                    sb.Append("<td>" + item.PullQty + "</td>");
                    sb.Append("<td>" + item.ItemCode + "</td>");
                    sb.Append("<td>" + item.Description + "</td>");
                    sb.Append("</tr></table>");

                    sbEmailBody = $"<html><pre>A new {item.ClientName} order came in :\nEmail:{model.RequestorEmail}\n" +
                                 $"Request for :{model.RequestorForName}\nDestination Building:{item.DestBuilding}\n" +
                                 $"Destination Floor :{item.DestFloor}\n" +
                                 $"Room / Floor / Area :{item.DestRoom}\n" +
                                 $"Requested Installation Date:{TimeZoneInfo.ConvertTimeToUtc(DateTime.SpecifyKind(Convert.ToDateTime(item.InstDate), DateTimeKind.Unspecified), TimeZoneInfo.Local)}" +
                                 $"\nComments:{item.Comments}</pre>" +
                                 $"{sb}</html>";

                    sbFullEmailBody.Append(sbEmailBody);
                }

                _emailNotificationRepository.SendEmail(sbFullEmailBody.ToString(), model.RequestorForName, model.RequestorEmail, model.CartItem[0].ClientName);
                //_emailNotificationRepository.SendEmail(body, model.RequestorProjectName, model.RequestorEmail, model.CartItem[0].ClientName);
                //_emailNotificationRepository.SendGmailEmail(body, model.RequestorProjectName, model.RequestorEmail, model.CartItem[0].ClientName);

                //result = await _hdTicketRepository.CreateHDTicket(model, body);
                result = await _hdRequestRepository.CreateOrderReqHDTicket(model, sbFullEmailBody.ToString());
            }
            catch (Exception ex)
            {
                result = 0;
                _logger.LogError($"Exception occured due to call SendOrderEmailNotification Method=>{ex.Message} \n {ex.StackTrace}");
                throw;
            }

            return result;
        }

        private async Task<bool> MapRequestIdToOrder(int orderid, int requestid)
        {
            bool result = false;
            try
            {
                var entity = await _dbContext.Orders.Where(ord => ord.OrderID == orderid).FirstOrDefaultAsync();

                if (entity != null)
                {
                    entity.RequestID = requestid;

                    _dbContext.Update(entity);
                    await _dbContext.SaveChangesAsync();

                    result = true;
                }
            }
            catch (Exception ex)
            {
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
                    NewValue = ((int)Enums.Status.Reserved).ToString(),
                    Description = "Order Requset Created",
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

    }
}
