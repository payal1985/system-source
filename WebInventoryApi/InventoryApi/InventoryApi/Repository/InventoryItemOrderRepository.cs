using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using InventoryApi.DBContext;
using InventoryApi.DBModels.InventoryDBModels;
using InventoryApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InventoryApi.Helpers;
using Microsoft.EntityFrameworkCore;
using InventoryApi.Repository.Interfaces;

namespace InventoryApi.Repository
{
    public class InventoryItemOrderRepository : IInventoryItemOrderRepository
    {
        InventoryContext _dbContext;
        SSIRequestContext _requestContext;

        IConfiguration _configuration { get; }
        IEmailNotificationRepository _emailNotificationRepository { get; }
        IHDTicketRepository _hdTicketRepository { get; }

        IAws3Repository _aws3Repository { get; }

        private readonly ILoggerManagerRepository _logger;
        public InventoryItemOrderRepository(InventoryContext dbContext, IConfiguration configuration
                                            ,IEmailNotificationRepository emailNotificationRepository
                                            ,IHDTicketRepository hdTicketRepository
                                            ,IAws3Repository aws3Repository
                                            , ILoggerManagerRepository logger
                                            , SSIRequestContext requestContext
            )
        {
            _logger = logger;
            _dbContext = dbContext;
            _configuration = configuration;
            _emailNotificationRepository = emailNotificationRepository;
            _hdTicketRepository = hdTicketRepository;
            _aws3Repository = aws3Repository;
            Common._dbContext = dbContext;
            _requestContext = requestContext;
        }

        public async Task<bool> InsertInventoryOrder(InventoryOrderModel model)
        {
            bool result = false;
            using (IDbContextTransaction transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                { 
                    Order orderModel = new Order();

                    orderModel.Email = model.RequestorEmail;
                    orderModel.RequestForName = model.RequestorProjectName;
                   // orderModel.Location = "";
                   // orderModel.Room = model.DestRoom;
                   // orderModel.DepartmentCostCenter = model.DepartmentCostCenter;
                   //// ordeModel.instdate = TimeZoneInfo.ConvertTimeFromUtc(model.requested_inst_date.Date,TimeZoneInfo.Local).Date;                    
                   // //orderModel.instdate = TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(model.requested_inst_date),TimeZoneInfo.Local);                    
                   // orderModel.InstallDate = Convert.ToDateTime(model.InstDate);                    
                   // //orderModel.instdate = System.DateTime.Now;                    
                   // orderModel.Comments = model.Comments;
                   // orderModel.DestBuildingID = model.DestInventoryBuildingID;
                   // orderModel.DestFloorID = model.DestInventoryFloorID;
                   // orderModel.InstallDateReturn = System.DateTime.Now;
                    orderModel.ClientID = model.CartItem[0].ClientID;
                    orderModel.CreateID = model.CartItem[0].UserId;
                    orderModel.CreateDateTime = System.DateTime.Now;
                    orderModel.StatusID = (int)Enums.Status.OrdOpen;
                    orderModel.OrderTypeID = (int)Enums.OrderType.Relocate;

                    await _dbContext.AddAsync(orderModel);
                    await _dbContext.SaveChangesAsync();

                    int order_id = orderModel.OrderID;

                    var orderItemModel = model.CartItem;

                    var invorderitemresult = await InsertInventoryOrderItem(order_id, orderItemModel);
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

        private async Task<bool> InsertInventoryOrderItem(int order_id, List<Cart> cart)
        {
            bool result = false;
            try
            {
                foreach(var item in cart)
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
                            CreateID = item.UserId,
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
                    }
                    else
                    {
                        if(item.PullQty > 1)
                        {
                           orderItemList = await _dbContext.InventoryItems.Where(ii => ii.InventoryItemID >= item.InventoryItemID 
                                                                                && ii.AddedToCartItem 
                                                                                && ii.InventoryBuildingID == item.BuildingId
                                                                                && ii.InventoryFloorID == item.FloorId
                                                                                && ii.Room == item.Room
                                                                                && ii.ConditionID == item.ConditionId)
                                            .Take(item.PullQty)
                                            .Select(x=> new OrderItem()
                                            {
                                                InventoryID = x.InventoryID,
                                                InventoryItemID = x.InventoryItemID,
                                                ItemCode = item.ItemCode,
                                                OrderID = order_id,
                                                Qty = 1,
                                                ClientID = item.ClientID,
                                                CreateID = item.UserId,
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
                                                Qty = item.PullQty,
                                                ClientID = item.ClientID,
                                                CreateID = item.UserId,
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
                        }

                    }
                }

                

                //var orderItemList = cart.Select(x => new OrderItem()
                //{
                //    InventoryID = x.InventoryID,
                //    InventoryItemID = x.InventoryItemID,
                //    ItemCode  = x.ItemCode,
                //    OrderID = order_id,
                //    Qty = x.PullQty,
                //    ClientID = x.ClientID,
                //    CreateID = x.UserId,
                //    CreateDateTime = System.DateTime.Now
                //}).ToList();

                //await _dbContext.AddRangeAsync(orderItemList);
                //await _dbContext.SaveChangesAsync();

                result = true;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception occured due to call InsertInventoryOrderItem Method=>{ex.Message} \n {ex.StackTrace}");
                throw;
            }
            return result;
        }
        private async Task<bool> UpdateInventoryItem(InventoryOrderModel ordModel , List<Cart> cart)
        {
            try
            {

                foreach (var item in cart)
                {
                    if(item.ChildInventoryItemModels != null && item.ChildInventoryItemModels.Count > 0)
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
                                    x.UpdateID = ordModel.CartItem.FirstOrDefault().UserId;
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
                                //x.InventoryBuildingID = item.DestInventoryBuildingID;
                                //x.InventoryFloorID = item.DestInventoryFloorID;
                                //x.Room = item.DestRoom;
                                x.UpdateID = ordModel.CartItem.FirstOrDefault().UserId;
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

                #region old working code
                //foreach (var item in cart)
                //{
                //    List<InventoryItem> entity = new List<InventoryItem>();
                //    //entity = await _dbContext.InventoryItems.Where(ii => ii.InventoryItemID == item.InventoryItemID && ii.AddedToCartItem).ToListAsync();

                //    if (item.PullQty > 1)
                //        entity = await _dbContext.InventoryItems.Where(ii => ii.InventoryID == item.InventoryID && ii.ConditionID == item.ConditionId && ii.InventoryItemID >= item.InventoryItemID && ii.AddedToCartItem).Take(item.PullQty).ToListAsync();
                //    else
                //        entity = await _dbContext.InventoryItems.Where(ii => ii.InventoryID == item.InventoryID && ii.ConditionID == item.ConditionId && ii.InventoryItemID == item.InventoryItemID && ii.AddedToCartItem).Take(item.PullQty).ToListAsync();

                //    _logger.LogInfo($"entity count is->{entity.Count} and pullqty is->{item.PullQty}");

                //    //foreach(var e in entity)
                //    //{
                //    //    e.Building = ordModel.DestBuilding;
                //    //    e.Floor = ordModel.DestFloor;
                //    //    e.Room = ordModel.DestRoom;
                //    //    e.InventoryBuildingID = ordModel.DestInventoryBuildingID;
                //    //    e.InventoryFloorID = ordModel.DestInventoryFloorID;

                //    //    _dbContext.Update(e);
                //    //    _dbContext.SaveChanges();
                //    //}                  
                //    if (entity != null && entity.Count > 0 && entity.Count.Equals(item.PullQty))
                //    {

                //        entity.ForEach(x =>
                //        {
                //            x.InventoryBuildingID = ordModel.DestInventoryBuildingID;
                //            x.InventoryFloorID = ordModel.DestInventoryFloorID;
                //            x.Room = ordModel.DestRoom;
                //            x.UpdateID = ordModel.CartItem.FirstOrDefault().UserId;
                //            x.UpdateDateTime = System.DateTime.Now;
                //            x.StatusID = (int)Enums.Status.Reserved; 
                //            x.AddedToCartItem = false;
                //        });

                //        _dbContext.UpdateRange(entity);
                //        await _dbContext.SaveChangesAsync();

                //        _logger.LogInfo($"entity is updated with required columns and changed flag to false in AddedToCartItem column");

                //        //foreach (var e in entity)
                //        //{
                //        //    e.InventoryBuildingID = ordModel.DestInventoryBuildingID;
                //        //    //e.Building = ordModel.DestBuilding;
                //        //    //e.Floor = ordModel.DestFloor;
                //        //    e.InventoryFloorID = ordModel.DestInventoryFloorID;
                //        //    e.Room = ordModel.DestRoom;
                //        //    e.UpdateID = ordModel.CartItem.FirstOrDefault().UserId;
                //        //    e.UpdateDateTime = System.DateTime.Now;
                //        //    e.StatusID = (int)Enums.Status.Reserved;
                //        //    e.AddedToCartItem = false;

                //        //    _dbContext.Update(e);
                //        //    _dbContext.SaveChanges();
                //        //}
                //    }
                //    else if(!entity.Count.Equals(item.PullQty))
                //    {
                //        List<InventoryItem> reverseEntity = new List<InventoryItem>();
                //        //reverseEntity = await _dbContext.InventoryItems.Where(ii => ii.InventoryItemID == item.InventoryItemID).Take(item.PullQty).ToListAsync();

                //        if (item.PullQty > 1)
                //            reverseEntity = await _dbContext.InventoryItems.Where(ii => ii.InventoryID == item.InventoryID && ii.ConditionID == item.ConditionId && ii.InventoryItemID >= item.InventoryItemID).Take(item.PullQty).ToListAsync();
                //        else
                //            reverseEntity = await _dbContext.InventoryItems.Where(ii => ii.InventoryID == item.InventoryID && ii.ConditionID == item.ConditionId && ii.InventoryItemID == item.InventoryItemID).Take(item.PullQty).ToListAsync();

                //        _logger.LogInfo($"reverseEntity count is->{reverseEntity.Count} and pullqty is->{item.PullQty}");

                //        if (reverseEntity != null && reverseEntity.Count > 0)
                //        {
                //            reverseEntity.ForEach(x =>
                //            {
                //                x.AddedToCartItem = true;

                //            });
                //            _dbContext.UpdateRange(reverseEntity);
                //            await _dbContext.SaveChangesAsync();

                //            _logger.LogInfo($"reverseEntity is updated with flag to true in AddedToCartItem column");

                //            //foreach (var e in reverseEntity)
                //            //{
                //            //    e.AddedToCartItem = true;

                //            //    _dbContext.Update(e);
                //            //    _dbContext.SaveChanges();
                //            //}
                //        }
                //        return false;
                //    }               

                //}
                #endregion

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception occured due to call UpdateInventoryItem Method=>{ex.Message} \n {ex.StackTrace}");
                throw;
            }
        }

        //private async Task<bool> InsertInventoryItemStatusHistory(InventoryOrderModel model)
        //{
        //    try
        //    {

        //        foreach (var item in model.CartItem)
        //        {
        //            if (item.ChildInventoryItemModels != null && item.ChildInventoryItemModels.Count > 0)
        //            {
        //                var cartentity = item.ChildInventoryItemModels.Where(ch => ch.IsSelected == true).ToList();

        //                if (cartentity != null)
        //                {
        //                    List<InventoryItem> entity = new List<InventoryItem>();

        //                    foreach (var invitem in cartentity)
        //                    {
        //                        var invitementity = await _dbContext.InventoryItems.Where(ii => ii.InventoryItemID == invitem.InventoryItemID && ii.AddedToCartItem).FirstOrDefaultAsync();

        //                        entity.Add(invitementity);
        //                    }

        //                    var statusHistorylist = entity.Select(x => new InventoryItemStatusHistory()
        //                    {
        //                        InventoryItemID = x.InventoryItemID,
        //                        InventoryID = x.InventoryID,
        //                        //LocationID = x.LocationID,
        //                        ClientID = x.ClientID,
        //                        InventoryBuildingID = x.InventoryBuildingID,
        //                        InventoryFloorID = x.InventoryFloorID,
        //                        Room = x.Room,
        //                        ConditionID = x.ConditionID,
        //                        DestInventoryBuildingID = item.DestInventoryBuildingID,
        //                        DestInventoryFloorID = item.DestInventoryFloorID,
        //                        DestRoom = item.DestRoom,
        //                        //DestCondition = "",
        //                        DepartmentCostCenter = item.DepartmentCostCenter,
        //                        StatusID = (int)Enums.Status.MovingLocation,
        //                        DisplayOnSite = x.DisplayOnSite,
        //                        DamageNotes = x.DamageNotes,
        //                        GPSLocation = x.GPSLocation,
        //                        RFIDcode = x.RFIDcode,
        //                        Barcode = x.Barcode,
        //                        QRCode = x.QRCode,
        //                        ProposalNumber = x.ProposalNumber,
        //                        PoOrderNo = x.PoOrderNo,
        //                        PoOrderDate = x.PoOrderDate,
        //                        NonSSIPurchaseDate = x.NonSSIPurchaseDate,
        //                        WarrantyRequestID = x.WarrantyRequestID,
        //                        OrigInventoryItemCreateDateTime = x.CreateDateTime,
        //                        CreateID = x.CreateID,
        //                        CreateDateTime = System.DateTime.Now,
        //                        UpdateID = x.UpdateID,
        //                        UpdateDateTime = x.UpdateDateTime,
        //                        SubmissionDate = x.SubmissionDate,
        //                        SubmissionID = x.SubmissionID,
        //                        AddedToCartItem = x.AddedToCartItem
        //                        //ExternalID = x.ExternalID,

        //                    }).ToList();

        //                    await _dbContext.AddRangeAsync(statusHistorylist);
        //                    await _dbContext.SaveChangesAsync();

        //                }
        //            }
        //            else
        //            {
        //                List<InventoryItem> inventoryItem = new List<InventoryItem>();

        //                if (item.PullQty > 1)
        //                    inventoryItem = await _dbContext.InventoryItems.Where(ii => ii.InventoryID == item.InventoryID && ii.ConditionID == item.ConditionId && ii.InventoryItemID >= item.InventoryItemID).Take(item.PullQty).ToListAsync();
        //                else
        //                    inventoryItem = await _dbContext.InventoryItems.Where(ii => ii.InventoryID == item.InventoryID && ii.ConditionID == item.ConditionId && ii.InventoryItemID == item.InventoryItemID).Take(item.PullQty).ToListAsync();


        //                var statusHistorylist = inventoryItem.Select(x => new InventoryItemStatusHistory()
        //                {
        //                    InventoryItemID = x.InventoryItemID,
        //                    InventoryID = x.InventoryID,
        //                    //LocationID = x.LocationID,
        //                    ClientID = x.ClientID,
        //                    InventoryBuildingID = x.InventoryBuildingID,
        //                    InventoryFloorID = x.InventoryFloorID,
        //                    Room = x.Room,
        //                    ConditionID = x.ConditionID,
        //                    DestInventoryBuildingID = item.DestInventoryBuildingID,
        //                    DestInventoryFloorID = item.DestInventoryFloorID,
        //                    DestRoom = item.DestRoom,
        //                    //DestCondition = "",
        //                    DepartmentCostCenter = item.DepartmentCostCenter,
        //                    StatusID = (int)Enums.Status.MovingLocation,
        //                    DisplayOnSite = x.DisplayOnSite,
        //                    DamageNotes = x.DamageNotes,
        //                    GPSLocation = x.GPSLocation,
        //                    RFIDcode = x.RFIDcode,
        //                    Barcode = x.Barcode,
        //                    QRCode = x.QRCode,
        //                    ProposalNumber = x.ProposalNumber,
        //                    PoOrderNo = x.PoOrderNo,
        //                    PoOrderDate = x.PoOrderDate,
        //                    NonSSIPurchaseDate = x.NonSSIPurchaseDate,
        //                    WarrantyRequestID = x.WarrantyRequestID,
        //                    OrigInventoryItemCreateDateTime = x.CreateDateTime,
        //                    CreateID = x.CreateID,
        //                    CreateDateTime = System.DateTime.Now,
        //                    UpdateID = x.UpdateID,
        //                    UpdateDateTime = x.UpdateDateTime,
        //                    SubmissionDate = x.SubmissionDate,
        //                    SubmissionID = x.SubmissionID,
        //                    AddedToCartItem = x.AddedToCartItem
        //                    //ExternalID = x.ExternalID,

        //                }).ToList();

        //                await _dbContext.AddRangeAsync(statusHistorylist);
        //                await _dbContext.SaveChangesAsync();

        //            }

        //        }

        //        #region old working code
        //        //foreach (var item in model.CartItem)
        //        //{
        //        //    //List<InventoryItem> inventoryItemStatusHistory = new List<InventoryItem>();
        //        //    List<InventoryItem> inventoryItem = new List<InventoryItem>();
        //        //    //inventoryItem = await _dbContext.InventoryItems.Where(ii => ii.InventoryItemID == item.InventoryItemID).ToListAsync();

        //        //    if (item.PullQty > 1)
        //        //        inventoryItem = await _dbContext.InventoryItems.Where(ii => ii.InventoryID == item.InventoryID && ii.ConditionID == item.ConditionId && ii.InventoryItemID >= item.InventoryItemID).Take(item.PullQty).ToListAsync();
        //        //    else
        //        //        inventoryItem = await _dbContext.InventoryItems.Where(ii => ii.InventoryID == item.InventoryID && ii.ConditionID == item.ConditionId && ii.InventoryItemID == item.InventoryItemID).Take(item.PullQty).ToListAsync();


        //        //    var statusHistorylist = inventoryItem.Select(x => new InventoryItemStatusHistory()
        //        //    {
        //        //        InventoryItemID = x.InventoryItemID,
        //        //        InventoryID = x.InventoryID,
        //        //        //LocationID = x.LocationID,
        //        //        ClientID = x.ClientID,
        //        //        InventoryBuildingID = x.InventoryBuildingID,
        //        //        InventoryFloorID = x.InventoryFloorID,
        //        //        Room = x.Room,
        //        //        ConditionID = x.ConditionID,
        //        //        DestInventoryBuildingID = model.DestInventoryBuildingID,
        //        //        DestInventoryFloorID = model.DestInventoryFloorID,
        //        //        DestRoom = model.DestRoom,
        //        //        //DestCondition = "",
        //        //        DepartmentCostCenter = model.DepartmentCostCenter,
        //        //        StatusID = (int)Enums.Status.MovingLocation,
        //        //        DisplayOnSite = x.DisplayOnSite,
        //        //        DamageNotes = x.DamageNotes,
        //        //        GPSLocation = x.GPSLocation,
        //        //        RFIDcode = x.RFIDcode,
        //        //        Barcode = x.Barcode,
        //        //        QRCode = x.QRCode,
        //        //        ProposalNumber = x.ProposalNumber,
        //        //        PoOrderNo = x.PoOrderNo,
        //        //        PoOrderDate = x.PoOrderDate,
        //        //        NonSSIPurchaseDate = x.NonSSIPurchaseDate,
        //        //        WarrantyRequestID = x.WarrantyRequestID,
        //        //        OrigInventoryItemCreateDateTime = x.CreateDateTime,
        //        //        CreateID = x.CreateID,
        //        //        CreateDateTime=System.DateTime.Now,
        //        //        UpdateID = x.UpdateID,
        //        //        UpdateDateTime = x.UpdateDateTime,
        //        //        SubmissionDate = x.SubmissionDate,
        //        //        SubmissionID = x.SubmissionID,
        //        //        AddedToCartItem = x.AddedToCartItem
        //        //        //ExternalID = x.ExternalID,

        //        //    }).ToList();

        //        //    await _dbContext.AddRangeAsync(statusHistorylist);
        //        //    await _dbContext.SaveChangesAsync();
        //        //}
        //        #endregion

        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"Exception occured due to call InsertInventoryItemStatusHistory Method=>{ex.Message} \n {ex.StackTrace}");
        //        throw;
        //    }
        //}

        private async Task<int> SendOrderEmailNotification(InventoryOrderModel model)
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
                    sb.Append("<td><img src='" + item.InventoryImageUrl + item.InventoryImageName + "' height='100px' width='100px'/></td>");

                    sb.Append("<td>" + item.PullQty + "</td>");
                    sb.Append("<td>" + item.ItemCode + "</td>");
                    sb.Append("<td>" + item.Description + "</td>");
                    sb.Append("</tr></table>");

                    sbEmailBody = $"<html><pre>A new {item.ClientName} order came in :\nEmail:{model.RequestorEmail}\n" +
                                 $"Request for :{model.RequestorProjectName}\nDestination Building:{item.DestBuilding}\n" +
                                 $"Destination Floor :{item.DestFloor}\n" +
                                 $"Room / Floor / Area :{item.DestRoom}\n" +
                                 $"Requested Installation Date:{TimeZoneInfo.ConvertTimeToUtc(DateTime.SpecifyKind(Convert.ToDateTime(item.InstDate), DateTimeKind.Unspecified), TimeZoneInfo.Local)}" +
                                 $"\nComments:{item.Comments}</pre>" +
                                 $"{sb}</html>";

                    sbFullEmailBody.Append(sbEmailBody);
                    //sbEmailBody.Append($"<html><pre>A new {item.ClientName} order came in :\nEmail:{model.RequestorEmail}\n" +
                    //             $"Request for :{model.RequestorProjectName}\nDestination Building:{item.DestBuilding}\n" +
                    //             $"Destination Floor :{item.DestFloor}\n" +
                    //             $"Room / Floor / Area :{item.DestRoom}\n" +
                    //             $"Requested Installation Date:{TimeZoneInfo.ConvertTimeToUtc(DateTime.SpecifyKind(Convert.ToDateTime(item.InstDate), DateTimeKind.Unspecified), TimeZoneInfo.Local)}" +
                    //             $"\nComments:{item.Comments}</pre>" +
                    //             $"{sb}</html>");

                    //if(index == model.CartItem.Count)
                    //{
                    //    sbDestBuilding.Append($"{item.DestBuilding}");
                    //    sbDestFloor.Append($"{item.DestFloor}");
                    //    sbDestRoom.Append($"{item.DestRoom}");
                    //    sbInstDate.Append($"{Convert.ToDateTime(item.InstDate).Date}");
                    //    sbComment.Append($"{item.Comments}");
                    //}
                    //else
                    //{
                    //    sbDestBuilding.Append($"{item.DestBuilding} , ");
                    //    sbDestFloor.Append($"{item.DestFloor} , ");
                    //    sbDestRoom.Append($"{item.DestRoom} , ");
                    //    sbInstDate.Append($"{Convert.ToDateTime(item.InstDate).Date} , ");
                    //    sbComment.Append($"{item.Comments} , ");
                    //}
                    //index++;
                }

                //string body = $"<html><pre>A new {model.CartItem[0].ClientName} order came in :\nEmail:{model.RequestorEmail}\n" +
                //                 $"Request for :{model.RequestorProjectName}\nDestination Building:{model.DestBuilding}\nDestination Floor :{model.DestFloor}\n" +
                //                 $"Room / Floor / Area :{model.DestRoom}\n" +
                //                 $"Requested Installation Date:{TimeZoneInfo.ConvertTimeToUtc(DateTime.SpecifyKind(Convert.ToDateTime(model.InstDate), DateTimeKind.Unspecified), TimeZoneInfo.Local)}" +
                //                 $"\nComments:{model.Comments}</pre>" +
                //                 $"{sb}</html>";

                //string instDate = String.Join(",", model.CartItem.Distinct().Select(d => TimeZoneInfo.ConvertTimeToUtc(DateTime.SpecifyKind(Convert.ToDateTime(d.InstDate), DateTimeKind.Unspecified), TimeZoneInfo.Local)).ToArray());
                //string destRoom = String.Join(",", model.CartItem.Distinct().Select(d => d.DestRoom).ToArray());
                //string destFloor = String.Join(",", model.CartItem.Distinct().Select(d => d.DestFloor).ToArray());
                //string destBuilding = String.Join(",", model.CartItem.Distinct().Select(d => d.DestBuilding).ToArray());
                //string comments = String.Join(",", model.CartItem.Distinct().Select(d => d.Comments).ToArray());

                //string body = $"<html><pre>A new {model.CartItem[0].ClientName} order came in :\nEmail:{model.RequestorEmail}\n" +
                //                 $"Request for :{model.RequestorProjectName}" +
                //                 $"\nDestination Building:{destBuilding}" +
                //                 $"\nDestination Floor :{destFloor}\n" +
                //                 $"Room / Floor / Area :{destRoom}\n" +
                //                 $"Requested Installation Date:{instDate}" +
                //                 $"\nComments:{comments}</pre>" +
                //                 $"{sb}</html>";


                _emailNotificationRepository.SendEmail(sbFullEmailBody.ToString(), model.RequestorProjectName, model.RequestorEmail, model.CartItem[0].ClientName);
                //_emailNotificationRepository.SendEmail(body, model.RequestorProjectName, model.RequestorEmail, model.CartItem[0].ClientName);
                //_emailNotificationRepository.SendGmailEmail(body, model.RequestorProjectName, model.RequestorEmail, model.CartItem[0].ClientName);

                //result = await _hdTicketRepository.CreateHDTicket(model, body);
                result = await _hdTicketRepository.CreateHDTicket(model, sbFullEmailBody.ToString());
            }
            catch(Exception ex)
            {
                result = 0;
                _logger.LogError($"Exception occured due to call SendOrderEmailNotification Method=>{ex.Message} \n {ex.StackTrace}");
                throw;
            }

            return result;
        }

        private async Task<bool> MapRequestIdToOrder(int orderid,int requestid)
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
            catch(Exception ex)
            {
                throw;

            }
            return result;
        }

        public async Task<List<InventoryOrderItemModel>> GetInventoryOrderItems(int clientid,int ordertypeid, int statusid)
        {
            List<InventoryOrderItemModel> result = new List<InventoryOrderItemModel>();
            try
            {
                #region old code
                //result = (from i in _dbContext.Inventories
                //          join ii in _dbContext.InventoryItems on i.InventoryID equals ii.InventoryID
                //          join iish in _dbContext.InventoryItemStatusHistory on ii.InventoryItemID equals iish.InventoryItemID
                //          join oi in _dbContext.OrderItems on ii.InventoryItemID equals oi.InventoryItemID
                //          join o in _dbContext.Orders on oi.OrderID equals o.OrderID
                //          //where o.Complete == false
                //          where oi.Delivered == false
                //          select new InventoryOrderItemModel()
                //          {
                //              OrderID = o.OrderID,
                //              InventoryID = oi.InventoryID,
                //              InventoryItemID = oi.InventoryItemID,
                //              Email = o.Email,
                //              Project = o.Project,
                //              InstDate = oi.InstallDate,
                //              DestBuildingId = oi.DestBuildingID,
                //              DestFloorId = oi.DestFloorID,
                //              DestBuilding = _requestContext.ClientLocations.Where(ib => ib.InventoryBuildingID == oi.DestBuildingID).FirstOrDefault().InventoryBuildingName,
                //              DestFloor = _dbContext.InventoryFloors.Where(f => f.InventoryFloorID == oi.DestFloorID).FirstOrDefault().InventoryFloorName,
                //              DestRoom = oi.Room,
                //              Qty = oi.Qty,
                //             // Category = i.Category,
                //              Category = _dbContext.ItemTypes.Where(it=>it.ItemTypeID == i.ItemTypeID).FirstOrDefault().ItemTypeName,
                //              ItemCode = i.ItemCode,
                //              Description = i.Description,
                //              Building = _requestContext.ClientLocations.Where(ib => ib.InventoryBuildingID == iish.InventoryBuildingID).FirstOrDefault().InventoryBuildingName,
                //              Floor = _dbContext.InventoryFloors.Where(f => f.InventoryFloorID == iish.InventoryFloorID).FirstOrDefault().InventoryFloorName,
                //              Room = iish.Room,
                //              ClientID = o.ClientID,
                //              ImageUrl = _configuration.GetValue<string>("ImgUrl") + o.ClientID + "/",
                //              Condition = _dbContext.InventoryItemConditions.Where(c=>c.InventoryItemConditionID == iish.ConditionID).FirstOrDefault().ConditionName,
                //              ImageName = i.MainImage,
                //              OrderItemId = oi.OrderItemID,
                //             ConditionId = iish.ConditionID,
                //              BucketName = _configuration.GetValue<string>("AwsConfig:BuketName"),
                //              ImagePath = $"inventory/{i.ClientID}/{i.MainImage}",
                //              RequestId=o.RequestID

                //          }).AsQueryable().ToList();

                //var orders = (from i in _dbContext.Inventories
                //              join ii in _dbContext.InventoryItems on i.InventoryID equals ii.InventoryID
                //              join iish in _dbContext.InventoryItemStatusHistory on ii.InventoryItemID equals iish.InventoryItemID
                //              join oi in _dbContext.OrderItems on ii.InventoryItemID equals oi.InventoryItemID
                //              join o in _dbContext.Orders on oi.OrderID equals o.OrderID
                //              where oi.Completed == false
                //              where oi.Delivered == false
                //              group oi by new 
                //              {
                //                  oi.DestBuildingID,
                //                  oi.DestFloorID,
                //                  oi.Room,
                //                  oi.OrderID,
                //                  o.Email,
                //                  o.Project,
                //                  o.RequestID,
                //                 // i.ItemTypeID,
                //                 // i.Description,
                //                 // i.MainImage,
                //                 // //iish.InventoryBuildingID,
                //                 // //iish.InventoryFloorID,
                //                 //// iish.Room,
                //                 // //iish.ConditionID
                //              } 
                //              into grp
                //              select new InventoryOrderItemModel()
                //              {
                //                  OrderID = grp.Key.OrderID,
                //                  InventoryID = grp.Select(g=>g.InventoryID).FirstOrDefault(),
                //                  InventoryItemID = grp.Select(g => g.InventoryItemID).FirstOrDefault(),
                //                  Email = grp.Key.Email,
                //                  Project = grp.Key.Project,
                //                  InstDate = grp.Select(g => g.InstallDate).FirstOrDefault(),
                //                  DestBuildingId = grp.Key.DestBuildingID,
                //                  DestFloorId = grp.Key.DestFloorID,
                //                  DestBuilding = _requestContext.ClientLocations.Where(ib => ib.InventoryBuildingID == grp.Key.DestBuildingID).FirstOrDefault().InventoryBuildingName,
                //                  DestFloor = _dbContext.InventoryFloors.Where(f => f.InventoryFloorID == grp.Key.DestFloorID).FirstOrDefault().InventoryFloorName,
                //                  DestRoom = grp.Key.Room,
                //                  Qty = grp.Count(),
                //                  //// Category = i.Category,
                //                  //Category = _dbContext.ItemTypes.Where(it => it.ItemTypeID == grp.Key.ItemTypeID).FirstOrDefault().ItemTypeName,
                //                  //ItemCode = grp.Select(g => g.ItemCode).FirstOrDefault(),
                //                  //Description = grp.Key.Description,
                //                  ////Building = _requestContext.ClientLocations.Where(ib => ib.InventoryBuildingID == iish.InventoryBuildingID).FirstOrDefault().InventoryBuildingName,
                //                  ////Floor = _dbContext.InventoryFloors.Where(f => f.InventoryFloorID == iish.InventoryFloorID).FirstOrDefault().InventoryFloorName,
                //                  //Room = grp.Key. .Room,
                //                  //ClientID = grp.Select(g => g.ClientID).FirstOrDefault(),
                //                  //ImageUrl = _configuration.GetValue<string>("ImgUrl") + grp.Select(g => g.ClientID).FirstOrDefault() + "/",
                //                  ////Condition = _dbContext.InventoryItemConditions.Where(c => c.InventoryItemConditionID == iish.ConditionID).FirstOrDefault().ConditionName,
                //                  //ImageName = grp.Key.MainImage,
                //                  //OrderItemId = grp.Select(g => g.OrderItemID).FirstOrDefault(),
                //                  ////ConditionId = iish.ConditionID,
                //                  //BucketName = _configuration.GetValue<string>("AwsConfig:BuketName"),
                //                  //ImagePath = $"inventory/{grp.Select(g => g.ClientID).FirstOrDefault()}/{grp.Key.MainImage}",
                //                  RequestId = grp.Key.RequestID
                //              }).AsQueryable();

                // var gropinglocation = item.InventoryItemModels.GroupBy(x => new { x.Building, x.Floor, x.Room, x.Condition })
                //                    .Select(g => new { g.Key.Building, g.Key.Floor, g.Key.Room, g.Key.Condition, qtyCount = g.Count() })
                //                    .ToList();
                #endregion

                //if (ordertypeid == (int)Enums.OrderType.NewInstallation)
                //{
                //    result = await GetNewInstallationInventoryItems(clientid, (int)Enums.Condition.New);
                //}
                //else
                //{
                    var grpOrders = from o in _dbContext.Orders
                                    join oi in _dbContext.OrderItems on o.OrderID equals oi.OrderID
                                    //where oi.Completed == false
                                    //where oi.Delivered == false
                                    where oi.ClientID == clientid
                                    where o.StatusID == statusid
                                    where (ordertypeid > 0 ? o.OrderTypeID.Equals(ordertypeid) : o.OrderTypeID >= ordertypeid)
                                    //where oi.StatusID == statusid
                                    group oi by new
                                    {
                                        oi.DestBuildingID,
                                        oi.DestFloorID,
                                        oi.DestRoom,
                                        oi.OrderID,
                                        o.Email,
                                        o.RequestForName,
                                        o.RequestID,
                                        o.StatusID,
                                        o.ClientNote,
                                        o.ActionNote,
                                        o.InstallerInstruction,
                                        o.OrderTypeID,
                                        oi.InventoryID,
                                        oi.InstallDate
                                    }
                                   into grp
                                    select new InventoryOrderItemModel
                                    {
                                        OrderID = grp.Key.OrderID,
                                        InventoryID = grp.Key.InventoryID,
                                        Email = grp.Key.Email,
                                        Project = grp.Key.RequestForName,
                                        InstDate = grp.Key.InstallDate,
                                        DestBuildingId = grp.Key.DestBuildingID,
                                        DestFloorId = grp.Key.DestFloorID,
                                        //DestBuilding = (grp.Key.DestBuildingID == 0 ? "" : _requestContext.ClientLocations.Where(ib => ib.location_id == grp.Key.DestBuildingID).FirstOrDefault().location_name),
                                        DestFloor = _dbContext.InventoryFloors.Where(f => f.InventoryFloorID == grp.Key.DestFloorID).FirstOrDefault().InventoryFloorName,
                                        DestRoom = grp.Key.DestRoom,
                                        Qty = grp.Count(),
                                        RequestId = grp.Key.RequestID,
                                        StatusId = grp.Key.StatusID,
                                        ActionNote = grp.Key.ActionNote,
                                        ClientNote = grp.Key.ClientNote,
                                        InstallerInstruction = grp.Key.InstallerInstruction,
                                        OrderTypeId = grp.Key.OrderTypeID
                                    };

                    result = await grpOrders.OrderBy(ord => ord.OrderID).ThenBy(ord => ord.InstDate).ToListAsync();

                    foreach (var item in result)
                    {
                        item.CompleteBtnFlag = _dbContext.OrderItems.Where(oi => oi.OrderID == item.OrderID).All(rec => rec.StatusID == (int)Enums.Status.OrdComplete) ? true : false;
                        item.IsCompletedRow = item.StatusId == (int)Enums.Status.OrdComplete ? true : false;
                        item.IsClosedRow = item.StatusId == (int)Enums.Status.OrdClosed ? true : false;

                        // var nameOfEnums = Enum.GetNames(typeof(Enums.Status));
                        item.Status = Common.GetDisplayName((Enums.Status)item.StatusId);

                        if (item.DestBuildingId < 0)
                            item.DestBuilding = Common.GetDisplayName((Enums.DestBuilding)item.DestBuildingId);
                        else
                            item.DestBuilding = (item.DestBuildingId == 0 || item.DestBuildingId == null ? "" : _requestContext.ClientLocations.Where(ib => ib.location_id == item.DestBuildingId).FirstOrDefault().location_name);

                        var inventory = await _dbContext.Inventories.Where(i => i.InventoryID == item.InventoryID).FirstOrDefaultAsync();

                        if (inventory != null)
                        {
                            item.Category = _dbContext.ItemTypes.Where(it => it.ItemTypeID == inventory.ItemTypeID).FirstOrDefault().ItemTypeName;
                            item.ItemCode = inventory.ItemCode;
                            item.Description = inventory.Description;
                            item.ClientID = inventory.ClientID;
                            item.ImageUrl = _configuration.GetValue<string>("ImgUrl") + inventory.ClientID + "/";
                            item.ImageName = inventory.MainImage;
                            item.BucketName = _configuration.GetValue<string>("AwsConfig:BuketName");
                            item.ImagePath = $"{_configuration.GetValue<string>("AwsConfig:S3Folder")}/{inventory.ClientID}/images/{inventory.MainImage}";
                        }
                        var inventoryId = _dbContext.OrderItems.Where(oi => oi.DestBuildingID == item.DestBuildingId
                                                                          && oi.DestFloorID == item.DestFloorId
                                                                          && oi.DestRoom == item.DestRoom).FirstOrDefault().InventoryItemID;

                        var inventoryitem = (inventoryId > 0 ? await _dbContext.InventoryItems.Where(ii => ii.InventoryItemID == inventoryId).FirstOrDefaultAsync() : null);

                        if (inventoryitem != null)
                        {
                            item.Building = (inventoryitem.InventoryBuildingID < 0 ? Common.GetDisplayName((Enums.DestBuilding)inventoryitem.InventoryBuildingID) : inventoryitem.InventoryBuildingID == 0 ? "" : _requestContext.ClientLocations.Where(ib => ib.location_id == inventoryitem.InventoryBuildingID).FirstOrDefault().location_name);
                            item.Floor = _dbContext.InventoryFloors.Where(f => f.InventoryFloorID == inventoryitem.InventoryFloorID).FirstOrDefault().InventoryFloorName;
                            item.Room = inventoryitem.Room;
                            item.Condition = _dbContext.InventoryItemConditions.Where(c => c.InventoryItemConditionID == inventoryitem.ConditionID).FirstOrDefault().ConditionName;
                            item.ProposalNumber = inventoryitem.ProposalNumber;
                            item.PoOrderNo = inventoryitem.PoOrderNo.ToString();
                        }

                        //var invHistory = await _dbContext.InventoryItemStatusHistory
                        //                .Where(iish => iish.InventoryID == item.InventoryID
                        //                       && iish.DestInventoryBuildingID == item.DestBuildingId
                        //                       && iish.DestInventoryFloorID == item.DestFloorId
                        //                       && iish.DestRoom == item.DestRoom).FirstOrDefaultAsync();

                        //if (invHistory != null)
                        //{
                        //    item.Building = (invHistory.InventoryBuildingID == 0 ? "" : _requestContext.ClientLocations.Where(ib => ib.location_id == invHistory.InventoryBuildingID).FirstOrDefault().location_name);
                        //    item.Floor = _dbContext.InventoryFloors.Where(f => f.InventoryFloorID == invHistory.InventoryFloorID).FirstOrDefault().InventoryFloorName;
                        //    item.Room = invHistory.Room;
                        //    item.Condition = _dbContext.InventoryItemConditions.Where(c => c.InventoryItemConditionID == invHistory.ConditionID).FirstOrDefault().ConditionName;

                        //}

                        //item.OrderItemId = _dbContext.OrderItems.Where(oi => oi.OrderID == item.OrderID
                        //                                                    && oi.DestBuildingID == item.DestBuildingId
                        //                                                    && oi.DestFloorID == item.DestFloorId
                        //                                                    && oi.Room == item.DestRoom
                        //                                                    && oi.InventoryID == item.InventoryID
                        //                                                    && oi.InstallDate == item.InstDate).FirstOrDefault().OrderItemID;

                    }

                    //var grpOrders =  result.GroupBy(x => new { x.DestBuildingId, x.DestFloorId, x.DestRoom,x.DestBuilding,x.DestFloor })
                    //                 .Select(g => 
                    //                 g.Select(k=>
                    //                 new InventoryOrderItemModel
                    //                 { 
                    //                     DestBuildingId = g.Key.DestBuildingId,
                    //                     DestFloorId = g.Key.DestFloorId,
                    //                     DestRoom = g.Key.DestRoom,
                    //                     Qty = g.Count(),

                    //                     OrderID = k.OrderID,
                    //                     InventoryID = k.InventoryID,
                    //                     InventoryItemID = k.InventoryItemID,
                    //                     Email = k.Email,
                    //                     Project = k.Project,
                    //                     InstDate = k.InstDate,

                    //                     DestBuilding = g.Key.DestBuilding,
                    //                     DestFloor = g.Key.DestFloor,

                    //                     Category = k.Category,
                    //                     ItemCode = k.ItemCode,
                    //                     Description = k.Description,
                    //                     Building = k.Building,
                    //                     Floor = k.Floor,
                    //                     Room = k.Room,
                    //                     ClientID = k.ClientID,
                    //                     ImageUrl = k.ImageUrl,
                    //                     Condition = k.Condition,
                    //                     ImageName = k.ImageName,
                    //                     OrderItemId = k.OrderItemId,
                    //                     ConditionId = k.ConditionId,
                    //                     BucketName = k.BucketName,
                    //                     ImagePath = k.ImagePath,
                    //                     RequestId = k.RequestId
                    //                 }).ToList()
                    //                 ).ToList();






                    //foreach (var item in result)
                    //{
                    //    item.ImageName = Common.GetCartImages(item.InventoryItemID, item.InventoryID, item.ConditionId) ?? item.ImageName;


                    //    // var entity = _dbContext.InventoryImages.Where(iii => iii.InventoryItemID == item.InventoryItemID).ToList();
                    //    //// var imgName = _dbContext.InventoryItemImages.Where(iii => iii.InventoryItemID == item.InventoryItemID).FirstOrDefault().ImageName;
                    //    //if(entity != null && entity.Count > 0)
                    //    // {
                    //    //     var imgName = entity.FirstOrDefault().ImageName;
                    //    //     if (!string.IsNullOrEmpty(imgName))
                    //    //         item.ImageName = imgName;
                    //    // }


                    //    var base64String = await ConvertImageToBase64String(item.ClientID, _configuration.GetValue<string>("ImgUrl") + item.ClientID + "/" + item.ImageName);
                    //    //var base64String = await ConvertImageToBase64String(item.ClientID , item.ItemCode + ".jpg");
                    //    if(!string.IsNullOrEmpty(base64String))
                    //    {
                    //        base64String = "data:image/jpeg;base64," + base64String;
                    //        result.Where(r => r.ItemCode == item.ItemCode).Select(s => { s.ImageBase64 = base64String; return s; }).ToList();
                    //    }
                    //}

               // } //newInstallation if condition end bracket

                return result; // Task.Run(() => result);
                //return result;

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<InventoryOrderItemModel>> ExpandInventoryOrderItems(int orderid,int destbuildingid, int destfloorid, string destroom)
        {
            List<InventoryOrderItemModel> result = new List<InventoryOrderItemModel>();
            try
            {
                var grpOrders = (from oi in _dbContext.OrderItems
                                 join ii in _dbContext.InventoryItems on oi.InventoryItemID equals ii.InventoryItemID
                                 //where oi.Delivered == false
                                 where oi.OrderID == orderid
                                 //where oi.StatusID == (int)Enums.Status.OrdOpen
                                 //where oi.DestBuildingID == destbuildingid
                                 where oi.DestBuildingID == (destbuildingid > 0 ? destbuildingid : null)
                                 //&& (!parameter.HasValue && s.id_se == null) ^ (parameter.HasValue && s.id_se == parameter)
                                 where oi.DestFloorID == destfloorid
                                 where oi.DestRoom == (!string.IsNullOrEmpty(destroom) ? destroom : "")
                                 select new InventoryOrderItemModel()
                                 {
                                     //OrderID = o.OrderID,
                                     //InventoryID = oi.InventoryID,
                                     InventoryItemID = oi.InventoryItemID,
                                     //Email = o.Email,
                                     //Project = o.Project,
                                     //InstDate = oi.InstallDate,
                                     DestBuildingId = oi.DestBuildingID,
                                     DestFloorId = oi.DestFloorID,
                                     //DestBuilding = (oi.DestBuildingID == 0 ? "" : _requestContext.ClientLocations.Where(ib => ib.location_id == oi.DestBuildingID).FirstOrDefault().location_name),
                                     DestFloor = _dbContext.InventoryFloors.Where(f => f.InventoryFloorID == oi.DestFloorID).FirstOrDefault().InventoryFloorName,
                                     DestRoom = oi.DestRoom,
                                     Qty = oi.Qty,
                                     // Category = i.Category,
                                     //Category = _dbContext.ItemTypes.Where(it => it.ItemTypeID == i.ItemTypeID).FirstOrDefault().ItemTypeName,
                                     //ItemCode = i.ItemCode,
                                     //Description = i.Description,
                                     //Building = (iish.InventoryBuildingID == 0 ? "" : _requestContext.ClientLocations.Where(ib => ib.location_id == iish.InventoryBuildingID).FirstOrDefault().location_name),
                                     BuildingId = ii.InventoryBuildingID,
                                     Floor = _dbContext.InventoryFloors.Where(f => f.InventoryFloorID == ii.InventoryFloorID).FirstOrDefault().InventoryFloorName,
                                     Room = ii.Room,
                                     ClientID = oi.ClientID,
                                     //ImageUrl = _configuration.GetValue<string>("ImgUrl") + o.ClientID + "/",
                                     Condition = _dbContext.InventoryItemConditions.Where(c => c.InventoryItemConditionID == ii.ConditionID).FirstOrDefault().ConditionName,
                                     //ImageName = i.MainImage,
                                     OrderItemId = oi.OrderItemID,
                                     ConditionId = ii.ConditionID,
                                     //BucketName = _configuration.GetValue<string>("AwsConfig:BuketName"),
                                     //ImagePath = $"inventory/{i.ClientID}/{i.MainImage}",
                                     //RequestId = o.RequestID
                                     StatusId = oi.StatusID,
                                     ActionNote = oi.ActionNote,
                                     InstDate = oi.InstallDate,
                                     Comment = oi.Comments,
                                     IsCompletedRowItem = oi.StatusID == (int)Enums.Status.OrdComplete ? true : false
                                 }).AsQueryable();

                result = await grpOrders.ToListAsync();
                result.ForEach(rs =>
                {
                    rs.Status = Common.GetDisplayName((Enums.Status)rs.StatusId);
                    rs.DestBuilding = rs.DestBuildingId < 0 ? Common.GetDisplayName((Enums.DestBuilding)rs.DestBuildingId) : (rs.DestBuildingId == 0 || rs.DestBuildingId == null) ? "" : _requestContext.ClientLocations.Where(ib => ib.location_id == rs.DestBuildingId).FirstOrDefault().location_name;
                    //rs.DestBuilding = (rs.DestBuildingId == 0 ? "" : _requestContext.ClientLocations.Where(ib => ib.location_id == rs.DestBuildingId).FirstOrDefault().location_name);
                    rs.Building = (rs.BuildingId < 0 ? Common.GetDisplayName((Enums.DestBuilding)rs.BuildingId) : rs.BuildingId == 0 ? "" : _requestContext.ClientLocations.Where(ib => ib.location_id == rs.BuildingId).FirstOrDefault().location_name);
                });
                return result; // Task.Run(() => result);
                //return result;

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private async Task<string> ConvertImageToBase64String(int clientId,string imgurl)
        {
            using (var client = new System.Net.Http.HttpClient())
            {

                //if (File.Exists(imgurl)) //local folder file check condition
                if (await _aws3Repository.IsFileExists(Path.GetFileName(imgurl), clientId,""))
                    {
                    //var bytes = await File.ReadAllBytesAsync(imgurl); //local folder/server folder image convert to byte so, base on need do uncomment and comment
                    ////byte[] imageBytes = Convert.FromBase64String(imgurl); 
                    var bytes = await client.GetByteArrayAsync(imgurl); //http url image convert to byte so, base on need do uncomment and comment
                    var base64String = Convert.ToBase64String(bytes);
                    return base64String;
                }
                else
                    return null;
        }
                
        }

        //public async Task<bool> CompleteOrder(int orderid, int destbuildingid, int destfloorid, string destroom, int userid, string apicallurl,string actionnote)
        //{
        //    bool result = false;
        //    using (IDbContextTransaction transaction = await _dbContext.Database.BeginTransactionAsync())
        //    {
        //        try
        //        {
        //            var ordEntity = await _dbContext.OrderItems.Where(ord => ord.OrderID == orderid 
        //                                                        && ord.DestBuildingID == destbuildingid 
        //                                                        && ord.DestFloorID == destfloorid
        //                                                        && ord.DestRoom == destroom
        //                                                        && ord.StatusID != (int)Enums.Status.OrdComplete).ToListAsync();
                    
        //            var batchTransactionID = System.Guid.NewGuid();
        //            DateTime historyCreateDateTime = System.DateTime.Now;

        //            if (ordEntity != null && ordEntity.Count > 0)
        //            {
        //                foreach (var oe in ordEntity)
        //                {

        //                    var invItem = await _dbContext.InventoryItems.Where(ii => ii.InventoryItemID == oe.InventoryItemID
        //                                   && ii.StatusID == (int)Enums.Status.Reserved).FirstOrDefaultAsync();

        //                    Dictionary<string, string> historyCols = new Dictionary<string, string>();

        //                    if (invItem != null)
        //                    {
        //                        historyCols.Add("InventoryBuildingID", invItem.InventoryBuildingID.ToString());
        //                        historyCols.Add("InventoryFloorID", invItem.InventoryFloorID.ToString());
        //                        historyCols.Add("Room", invItem.Room);
        //                        historyCols.Add("StatusID", invItem.StatusID.ToString());

        //                        invItem.InventoryBuildingID = (int)oe.DestBuildingID;
        //                        invItem.InventoryFloorID = oe.DestFloorID;
        //                        invItem.Room = oe.DestRoom;
        //                        invItem.StatusID = (int)Enums.Status.Active;
        //                        invItem.UpdateID = userid;
        //                        invItem.UpdateDateTime = System.DateTime.Now;

        //                        _dbContext.Update(invItem);
        //                        await _dbContext.SaveChangesAsync();
        //                    }



        //                    ////insert InventoryHistory records belong to orderid and InventoryItemID for InventoryItem Table
        //                    var invHistory = historyCols.Select(x => new InventoryHistory()
        //                    {
        //                        BatchTransactionGUID = batchTransactionID,
        //                        OrderID = orderid,
        //                        EntityID = oe.InventoryItemID,
        //                        ApiName = apicallurl,
        //                        TableName = "InventoryItem",
        //                        ColumnName = x.Key,
        //                        OldValue = x.Value,
        //                        NewValue = (
        //                                    x.Key.Contains("BuildingID") ? oe.DestBuildingID.ToString() :
        //                                    x.Key.Contains("FloorID") ? oe.DestFloorID.ToString() :
        //                                    x.Key.Contains("Room") ? oe.DestRoom : 
        //                                    x.Key.Contains("StatusID") ? ((int)Enums.Status.Active).ToString() : ""
        //                                   ),
        //                        Description = x.Key + " Updated",
        //                        CreateDateTime = historyCreateDateTime,
        //                        CreateID = userid
        //                    });

        //                    await _dbContext.AddRangeAsync(invHistory);
        //                    await _dbContext.SaveChangesAsync();

        //                    ////insert InventoryHistory records belong to orderid for OrderItem table update
        //                    Dictionary<string, string> historyOrdItemCols = new Dictionary<string, string>();
        //                    //historyOrdItemCols.Add("Completed", oe.Completed.ToString());
        //                    historyOrdItemCols.Add("Delivered", oe.Delivered.ToString());
        //                    historyOrdItemCols.Add("StatusID", oe.StatusID.ToString());

        //                    var invHistoryOrdItem = historyOrdItemCols.Select(x => new InventoryHistory()
        //                    {
        //                        BatchTransactionGUID = batchTransactionID,
        //                        OrderID = orderid,
        //                        EntityID = oe.OrderItemID,
        //                        ApiName = apicallurl,
        //                        TableName = "OrderItem",
        //                        ColumnName = x.Key,
        //                        OldValue = x.Value,
        //                        NewValue = (
        //                                         //x.Key.Contains("Completed") ? "1" :
        //                                         x.Key.Contains("Delivered") ? "1" :
        //                                         x.Key.Contains("StatusID") ? ((int)Enums.Status.OrdComplete).ToString() : ""
        //                                    ),
        //                        Description = x.Key + " Updated",
        //                        CreateDateTime = historyCreateDateTime,
        //                        CreateID = userid
        //                    });

        //                    await _dbContext.AddRangeAsync(invHistoryOrdItem);
        //                    await _dbContext.SaveChangesAsync();

        //                    //oe.Completed = true;
        //                    oe.Delivered = true;
        //                    oe.UpdateDateTime = System.DateTime.Now;
        //                    oe.UpdateID = userid;
        //                    oe.StatusID = (int)Enums.Status.OrdComplete;

        //                    _dbContext.Update(oe);
        //                    await _dbContext.SaveChangesAsync();
        //                }

        //                var order = _dbContext.Orders.Where(ord => ord.OrderID == orderid).FirstOrDefault();

        //                if(order != null)
        //                {
        //                    ////insert InventoryHistory records belong to orderid for Order table update                            
        //                    Dictionary<string, string> historyOrdCols = new Dictionary<string, string>();                            
        //                    historyOrdCols.Add("ActionNote", order.ActionNote);
        //                    historyOrdCols.Add("StatusID", order.StatusID.ToString());

        //                    var invHistoryOrd = historyOrdCols.Select(x => new InventoryHistory()
        //                    {
        //                        BatchTransactionGUID = batchTransactionID,
        //                        OrderID = orderid,
        //                        EntityID = order.OrderID,
        //                        ApiName = apicallurl,
        //                        TableName = "Order",
        //                        ColumnName = x.Key,
        //                        OldValue = x.Value,
        //                        NewValue = (                                                
        //                                         x.Key.Contains("ActionNote") ? actionnote :
        //                                         x.Key.Contains("StatusID") ? ((int)Enums.Status.OrdComplete).ToString() : ""
        //                                    ),
        //                        Description = x.Key + " Updated",
        //                        CreateDateTime = historyCreateDateTime,
        //                        CreateID = userid
        //                    });         

        //                    await _dbContext.AddRangeAsync(invHistoryOrd);
        //                    await _dbContext.SaveChangesAsync();

        //                order.StatusID = (int)Enums.Status.OrdComplete;
        //                order.UpdateID = userid;
        //                order.ActionNote = actionnote;
        //                order.UpdateDateTime = System.DateTime.Now;

        //                _dbContext.Update(order);
        //                await _dbContext.SaveChangesAsync();
        //            }
                       

        //                //ordEntity.ForEach(oe =>
        //                //{
        //                //    oe.Completed = true;
        //                //    oe.Delivered = true;

        //                //    var invItem =  _dbContext.InventoryItems.Where(ii => ii.InventoryItemID == oe.InventoryItemID 
        //                //                    && oe.StatusID == (int)Enums.Status.Reserved).FirstOrDefault();

        //                //    if(invItem != null)
        //                //    {
        //                //        invItem.StatusID = (int)Enums.Status.Active;

        //                //        _dbContext.Update(invItem);
        //                //        await _dbContext.SaveChangesAsync();
        //                //    }
        //                //});

        //                // _dbContext.UpdateRange(ordEntity);
        //                // await _dbContext.SaveChangesAsync();



        //            }
        //            await transaction.CommitAsync();
        //            result = true;


        //            //var entity = _dbContext.OrderItems.Where(ord => ord.OrderItemID == orderid).FirstOrDefault();

        //            //if(entity != null)
        //            //{
        //            //    entity.Delivered = true;
        //            //    entity.Completed = true;

        //            //    _dbContext.Update(entity);
        //            //    await _dbContext.SaveChangesAsync();

        //            //    result = true;
        //            //}
        //        }
        //        catch (Exception ex)
        //        {
        //            await transaction.RollbackAsync();
        //            throw;
        //        }
        //    }
        //    return result;
        //}

        public async Task<bool> CompleteOrder(int orderid,int userid, string apicallurl, string actionnote)
        {
            bool result = false;
            using (IDbContextTransaction transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    var orditemEntity = await _dbContext.OrderItems.Where(ord => ord.OrderID == orderid).ToListAsync();

                    var batchTransactionID = System.Guid.NewGuid();
                    DateTime historyCreateDateTime = System.DateTime.Now;

                    if (orditemEntity != null && orditemEntity.Count > 0)
                    {
                        foreach (var oe in orditemEntity)
                        {

                            var invItem = await _dbContext.InventoryItems.Where(ii => ii.InventoryItemID == oe.InventoryItemID
                                           && ii.StatusID == (int)Enums.Status.Reserved).FirstOrDefaultAsync();

                            Dictionary<string, string> historyCols = new Dictionary<string, string>();

                            if (invItem != null)
                            {
                                historyCols.Add("InventoryBuildingID", invItem.InventoryBuildingID.ToString());
                                historyCols.Add("InventoryFloorID", invItem.InventoryFloorID.ToString());
                                historyCols.Add("Room", invItem.Room);
                                historyCols.Add("StatusID", invItem.StatusID.ToString());

                                invItem.InventoryBuildingID = (int)oe.DestBuildingID;
                                invItem.InventoryFloorID = oe.DestFloorID;
                                invItem.Room = oe.DestRoom;
                                invItem.StatusID = (oe.DestBuildingID < 0 ? (int)Enums.Status.Inactive : (int)Enums.Status.Active);
                                invItem.UpdateID = userid;
                                invItem.UpdateDateTime = System.DateTime.Now;

                                _dbContext.Update(invItem);
                                await _dbContext.SaveChangesAsync();


                                ////insert InventoryHistory records belong to orderid and InventoryItemID for InventoryItem Table
                                var invitemHistory = historyCols.Select(x => new InventoryHistory()
                                {
                                    BatchTransactionGUID = batchTransactionID,
                                    OrderID = orderid,
                                    EntityID = oe.InventoryItemID,
                                    ApiName = apicallurl,
                                    TableName = "InventoryItem",
                                    ColumnName = x.Key,
                                    OldValue = x.Value,
                                    NewValue = (
                                                x.Key.Contains("BuildingID") ? oe.DestBuildingID.ToString() :
                                                x.Key.Contains("FloorID") ? oe.DestFloorID.ToString() :
                                                x.Key.Contains("Room") ? oe.DestRoom :
                                                x.Key.Contains("StatusID") ? invItem.StatusID.ToString() : ""
                                               ),
                                    Description = x.Key + " Updated",
                                    CreateDateTime = historyCreateDateTime,
                                    CreateID = userid
                                });

                                await _dbContext.AddRangeAsync(invitemHistory);
                                await _dbContext.SaveChangesAsync();
                            }

                        }

                        var order = _dbContext.Orders.Where(ord => ord.OrderID == orderid).FirstOrDefault();

                        if (order != null)
                        {
                            ////insert InventoryHistory records belong to orderid for Order table update                            
                            Dictionary<string, string> historyOrdCols = new Dictionary<string, string>();
                            historyOrdCols.Add("ActionNote", order.ActionNote);
                            historyOrdCols.Add("StatusID", order.StatusID.ToString());

                            var invHistoryOrd = historyOrdCols.Select(x => new InventoryHistory()
                            {
                                BatchTransactionGUID = batchTransactionID,
                                OrderID = orderid,
                                EntityID = order.OrderID,
                                ApiName = apicallurl,
                                TableName = "Order",
                                ColumnName = x.Key,
                                OldValue = x.Value,
                                NewValue = (
                                                 x.Key.Contains("ActionNote") ? actionnote :
                                                 x.Key.Contains("StatusID") ? ((int)Enums.Status.OrdComplete).ToString() : ""
                                            ),
                                Description = x.Key + " Updated",
                                CreateDateTime = historyCreateDateTime,
                                CreateID = userid
                            });

                            await _dbContext.AddRangeAsync(invHistoryOrd);
                            await _dbContext.SaveChangesAsync();

                            order.StatusID = (int)Enums.Status.OrdComplete;
                            order.UpdateID = userid;
                            order.ActionNote = actionnote;
                            order.UpdateDateTime = System.DateTime.Now;

                            _dbContext.Update(order);
                            await _dbContext.SaveChangesAsync();
                        }


                        //ordEntity.ForEach(oe =>
                        //{
                        //    oe.Completed = true;
                        //    oe.Delivered = true;

                        //    var invItem =  _dbContext.InventoryItems.Where(ii => ii.InventoryItemID == oe.InventoryItemID 
                        //                    && oe.StatusID == (int)Enums.Status.Reserved).FirstOrDefault();

                        //    if(invItem != null)
                        //    {
                        //        invItem.StatusID = (int)Enums.Status.Active;

                        //        _dbContext.Update(invItem);
                        //        await _dbContext.SaveChangesAsync();
                        //    }
                        //});

                        // _dbContext.UpdateRange(ordEntity);
                        // await _dbContext.SaveChangesAsync();



                    }
                    await transaction.CommitAsync();
                    result = true;


                    //var entity = _dbContext.OrderItems.Where(ord => ord.OrderItemID == orderid).FirstOrDefault();

                    //if(entity != null)
                    //{
                    //    entity.Delivered = true;
                    //    entity.Completed = true;

                    //    _dbContext.Update(entity);
                    //    await _dbContext.SaveChangesAsync();

                    //    result = true;
                    //}
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
            return result;
        }

        public async Task<bool> CompleteOrderItem(int orderitemid, int userid, string apicallurl, string actionnote)
        {
            bool result = false;
            using (IDbContextTransaction transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    var orditemEntity = await _dbContext.OrderItems.Where(ord => ord.OrderItemID == orderitemid).SingleOrDefaultAsync();

                    var batchTransactionID = System.Guid.NewGuid();
                    DateTime historyCreateDateTime = System.DateTime.Now;

                    if (orditemEntity != null)
                    {                      

                        ////insert InventoryHistory records belong to orderid for OrderItem table update
                        Dictionary<string, string> historyOrdItemCols = new Dictionary<string, string>();
                        //historyOrdItemCols.Add("Completed", orditemEntity.Completed.ToString());
                       // historyOrdItemCols.Add("Delivered", oe.Delivered.ToString());
                        historyOrdItemCols.Add("StatusID", orditemEntity.StatusID.ToString());
                        historyOrdItemCols.Add("ActionNote", orditemEntity.ActionNote);

                        var invHistoryOrdItem = historyOrdItemCols.Select(x => new InventoryHistory()
                        {
                            BatchTransactionGUID = batchTransactionID,
                            OrderID = orditemEntity.OrderID,
                            EntityID = orderitemid,
                            ApiName = apicallurl,
                            TableName = "OrderItem",
                            ColumnName = x.Key,
                            OldValue = x.Value,
                            NewValue = (
                                            // x.Key.Contains("Completed") ? "1" :
                                             //x.Key.Contains("Delivered") ? "1" :
                                             x.Key.Contains("StatusID") ? ((int)Enums.Status.OrdComplete).ToString() : 
                                             x.Key.Contains("ActionNote") ?  actionnote : ""
                                        ),
                            Description = x.Key + " Updated",
                            CreateDateTime = historyCreateDateTime,
                            CreateID = userid
                        });

                        await _dbContext.AddRangeAsync(invHistoryOrdItem);
                        await _dbContext.SaveChangesAsync();

                        //orditemEntity.Completed = true;
                        //orditemEntity.Delivered = true;
                        orditemEntity.UpdateDateTime = System.DateTime.Now;
                        orditemEntity.UpdateID = userid;
                        orditemEntity.StatusID = (int)Enums.Status.OrdComplete;
                        orditemEntity.ActionNote = actionnote;

                        _dbContext.Update(orditemEntity);
                        await _dbContext.SaveChangesAsync();

                    }
                    await transaction.CommitAsync();
                    result = true;

                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
            return result;
        }

        public async Task<bool> RemoveCartItem(int inventoryitemid)
        {
            bool result = false;
            try
            {
                var entity = _dbContext.InventoryItems.Where(item => item.InventoryItemID == inventoryitemid).FirstOrDefault();

                if (entity != null)
                {
                    entity.AddedToCartItem = false;

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

        public async Task<bool> CancelOrder(int orderid,int destbuildingid, int destfloorid, string destroom, int userid,string apicallurl)
        {
            bool result = false;
            using (IDbContextTransaction transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    var batchTransactionGuid = System.Guid.NewGuid();
                    var creatBatchTransactionDate = System.DateTime.Now;

                    var ordItemEntity = await _dbContext.OrderItems.Where(oi => oi.OrderID == orderid
                                                                          && oi.DestBuildingID == destbuildingid
                                                                          && oi.DestFloorID == destfloorid
                                                                          && oi.DestRoom == destroom
                                                                          && oi.StatusID != (int)Enums.Status.OrdCancelled).ToListAsync();

                    if(ordItemEntity != null && ordItemEntity.Count > 0)
                    {
                        foreach(var oi in ordItemEntity)                       
                        { 
                            var invHistory = new InventoryHistory()
                            {
                                BatchTransactionGUID = batchTransactionGuid,
                                OrderID = orderid,
                                EntityID = orderid,
                                ApiName = apicallurl,
                                TableName = "Order",
                                ColumnName = "StatusID",
                                OldValue = oi.StatusID.ToString(),
                                NewValue = ((int)Enums.Status.OrdCancelled).ToString(),
                                Description = "Status Changed due to cancel the order",
                                CreateDateTime = creatBatchTransactionDate,
                                CreateID = userid
                            };

                            await _dbContext.AddAsync(invHistory);
                            await _dbContext.SaveChangesAsync();

                            oi.StatusID = (int)Enums.Status.OrdCancelled;
                            oi.UpdateID = userid;
                            oi.UpdateDateTime = System.DateTime.Now;

                            _dbContext.Update(oi);
                            await _dbContext.SaveChangesAsync();
                        }

                        
                    }

                    var existOrdItem = await _dbContext.OrderItems.Where(oi => oi.OrderID == orderid && oi.StatusID != (int)Enums.Status.OrdCancelled).ToListAsync();

                    if(existOrdItem == null || existOrdItem.Count <= 0)
                    {
                        var ordEntity = await _dbContext.Orders.Where(o => o.OrderID == orderid).FirstOrDefaultAsync();

                        if (ordEntity != null)
                        {        
                            var invHistory = new InventoryHistory()
                            {
                                BatchTransactionGUID = batchTransactionGuid,
                                OrderID = orderid,
                                EntityID = orderid,
                                ApiName = apicallurl,
                                TableName = "Order",
                                ColumnName = "StatusID",
                                OldValue = ordEntity.StatusID.ToString(),
                                NewValue = ((int)Enums.Status.OrdCancelled).ToString(),
                                Description = "Status Changed due to cancel the order",
                                CreateDateTime = creatBatchTransactionDate,
                                CreateID = userid
                            };

                            await _dbContext.AddAsync(invHistory);
                            await _dbContext.SaveChangesAsync();

                            ordEntity.StatusID = (int)Enums.Status.OrdCancelled;
                            ordEntity.UpdateID = userid;
                            ordEntity.UpdateDateTime = System.DateTime.Now;

                            _dbContext.Update(ordEntity);
                            await _dbContext.SaveChangesAsync();
                        }
                    }


                    //Random rnd = new Random();

                    await transaction.CommitAsync();
                    result = true;
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }

            }

            return result;
        }

        public async Task<bool> CancelOrderItem(int orderitemid, int inventoryitemid, int userid, string apicallurl)
        {
            bool result = false;
            using (IDbContextTransaction transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    var ordItemEntity = await _dbContext.OrderItems.Where(oi => oi.OrderItemID == orderitemid).FirstOrDefaultAsync();
                    if (ordItemEntity != null)
                    {             
                        //history record insertion
                        //Random rnd = new Random();

                        var invHistory = new InventoryHistory()
                        {
                            BatchTransactionGUID = System.Guid.NewGuid(),
                            OrderID = ordItemEntity.OrderID,
                            EntityID = orderitemid,
                            ApiName = apicallurl,
                            TableName = "OrderItem",
                            ColumnName = "StatusID",
                            OldValue = ordItemEntity.StatusID.ToString(),
                            NewValue = ((int)Enums.Status.OrdCancelled).ToString(),
                            Description = "Status Changed for cancel the order item",
                            CreateDateTime = System.DateTime.Now,
                            CreateID = userid
                        };

                        await _dbContext.AddAsync(invHistory);
                        await _dbContext.SaveChangesAsync();


                        ordItemEntity.StatusID = (int)Enums.Status.OrdCancelled;
                        ordItemEntity.UpdateID = userid;
                        ordItemEntity.UpdateDateTime = System.DateTime.Now;

                        _dbContext.Update(ordItemEntity);
                        await _dbContext.SaveChangesAsync();

                        await transaction.CommitAsync();
                        result = true;
                    }

                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }

            }

            return result;
        }
    
        public async Task<bool> UpdateOrderLocation(int userid, int destbuildingid, int destfloorid, string destroom, string apicallurl, InventoryOrderItemModel model)
        {
            bool result = false;
            using (IDbContextTransaction transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    var ordItemEntity = await _dbContext.OrderItems.Where(oi => oi.OrderID == model.OrderID 
                                                                        && oi.DestBuildingID == destbuildingid
                                                                        && oi.DestFloorID == destfloorid
                                                                        && oi.DestRoom.Equals(destroom)
                                                                        ).ToListAsync();
                                      
                    if (ordItemEntity != null && ordItemEntity.Count > 0)
                    {
                        //Dictionary<string, string> historyCols = new Dictionary<string, string>();

                        //Random rnd = new Random();
                        var batchTransactionID = System.Guid.NewGuid();
                        DateTime historyCreateDateTime = System.DateTime.Now;

                        foreach (var oi in ordItemEntity)
                        {
                            Dictionary<string, string> historyCols = new Dictionary<string, string>();

                            if (model.DestBuildingId != destbuildingid)
                                historyCols.Add("DestBuildingID", oi.DestBuildingID.ToString());
                            if(model.DestFloorId != destfloorid)
                                historyCols.Add("DestFloorID", oi.DestFloorID.ToString());
                            if(model.DestRoom != destroom)
                                historyCols.Add("Room", oi.DestRoom);

                            oi.DestBuildingID = model.DestBuildingId;
                            oi.DestFloorID = model.DestFloorId;
                            oi.DestRoom = model.DestRoom;
                            oi.UpdateID = userid;
                            oi.UpdateDateTime = System.DateTime.Now;

                            _dbContext.Update(oi);
                            await _dbContext.SaveChangesAsync();

                            ////insert InventoryHistory records belong to orderid and InventoryItemID
                            var invHistory = historyCols.Select(x => new InventoryHistory()
                            {
                                BatchTransactionGUID = batchTransactionID,
                                OrderID = model.OrderID,
                                EntityID = oi.OrderItemID,
                                ApiName = apicallurl,
                                TableName = "OrderItem",
                                ColumnName = x.Key,
                                OldValue = x.Value,
                                NewValue = (
                                            x.Key.Contains("BuildingID") ? oi.DestBuildingID.ToString() :
                                            x.Key.Contains("FloorID") ? oi.DestFloorID.ToString() :
                                            x.Key.Contains("Room") ? oi.DestRoom : ""
                                           ),
                                Description = x.Key + " Updated",
                                CreateDateTime = historyCreateDateTime,
                                CreateID = userid
                            });

                            await _dbContext.AddRangeAsync(invHistory);
                            await _dbContext.SaveChangesAsync();

                        }

                    }

                   

                    await transaction.CommitAsync();
                    result = true;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw;
                }

            }

            return result;
        }

        public async Task<bool> UpdateOrderItemLocation(int userid, string apicallurl, InventoryOrderItemModel model)
        {
            bool result = false;
            using (IDbContextTransaction transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    var ordItemEntity = await _dbContext.OrderItems.Where(oi => oi.OrderItemID == model.OrderItemId                                                                      
                                                                        ).FirstOrDefaultAsync();

                    if (ordItemEntity != null)
                    {
                        Dictionary<string, string> historyCols = new Dictionary<string, string>();

                        //Random rnd = new Random();
                        var batchTransactionID = System.Guid.NewGuid();
                        DateTime historyCreateDateTime = System.DateTime.Now;

                        //foreach (var oi in ordItemEntity)
                        //{
                            if (ordItemEntity.DestBuildingID != model.DestBuildingId)
                                historyCols.Add("DestBuildingID", ordItemEntity.DestBuildingID.ToString());
                            if (ordItemEntity.DestFloorID != model.DestFloorId)
                                historyCols.Add("DestFloorID", ordItemEntity.DestFloorID.ToString());
                            if (ordItemEntity.DestRoom != model.DestRoom)
                                historyCols.Add("Room", ordItemEntity.DestRoom);

                            ordItemEntity.DestBuildingID = model.DestBuildingId;
                            ordItemEntity.DestFloorID = model.DestFloorId;
                            ordItemEntity.DestRoom = model.DestRoom;
                            ordItemEntity.UpdateID = userid;
                            ordItemEntity.UpdateDateTime = System.DateTime.Now;

                            _dbContext.Update(ordItemEntity);
                            await _dbContext.SaveChangesAsync();

                            ////insert InventoryHistory records belong to orderid and InventoryItemID
                            var invHistory = historyCols.Select(x => new InventoryHistory()
                            {
                                BatchTransactionGUID = batchTransactionID,
                                OrderID = ordItemEntity.OrderID,
                                EntityID = ordItemEntity.OrderItemID,
                                ApiName = apicallurl,
                                TableName = "OrderItem",
                                ColumnName = x.Key,
                                OldValue = x.Value,
                                NewValue = (
                                            x.Key.Contains("BuildingID") ? ordItemEntity.DestBuildingID.ToString() :
                                            x.Key.Contains("FloorID") ? ordItemEntity.DestFloorID.ToString() :
                                            x.Key.Contains("Room") ? ordItemEntity.DestRoom : ""
                                           ),
                                Description = x.Key + " Updated",
                                CreateDateTime = historyCreateDateTime,
                                CreateID = userid
                            });

                            await _dbContext.AddRangeAsync(invHistory);
                            await _dbContext.SaveChangesAsync();

                       // }

                    }

                    await transaction.CommitAsync();
                    result = true;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw;
                }

            }

            return result;
        }

        public async Task<bool> OrderAssignTo(string apicallurl, AssignToModel model)
        {
            using (IDbContextTransaction transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    //Random rnd = new Random();
                    var batchTransactionID = System.Guid.NewGuid();
                    DateTime historyCreateDateTime = System.DateTime.Now;

                    var orderitementity = await _dbContext.OrderItems.
                                            Where(orditem => orditem.OrderID == model.OrderId
                                                  //&& orditem.DestBuildingID == model.DestBuildingId
                                                  //&& orditem.DestFloorID == model.DestFloorId
                                                  //&& orditem.DestRoom == model.DestRoom
                                                   ).
                                            ToListAsync();

                    if(orderitementity != null && orderitementity.Count > 0)
                    {
                        foreach(var item in orderitementity)
                        {
                            var orditemHistory = new InventoryHistory();

                            orditemHistory.BatchTransactionGUID = batchTransactionID;
                            orditemHistory.OrderID = item.OrderID;
                            orditemHistory.EntityID = item.OrderItemID;
                            orditemHistory.ApiName = apicallurl;
                            orditemHistory.TableName = "OrderItem";
                            orditemHistory.ColumnName = "StatusID";
                            orditemHistory.OldValue = item.StatusID.ToString();
                            orditemHistory.NewValue = ((int)Enums.Status.OrdAssigned).ToString();                           
                            orditemHistory.Description = "StatusID Updated";
                            orditemHistory.CreateDateTime = historyCreateDateTime;
                            orditemHistory.CreateID = model.UserId;
                            
                            await _dbContext.AddAsync(orditemHistory);
                            await _dbContext.SaveChangesAsync();

                            item.StatusID = (int)Enums.Status.OrdAssigned;

                            _dbContext.Update(item);
                            await _dbContext.SaveChangesAsync();
                        }

                    }

                    var orderentity = await _dbContext.Orders.Where(ord => ord.OrderID == model.OrderId).FirstOrDefaultAsync();

                    if (orderentity != null)
                    {
                        Dictionary<string, string> historyCols = new Dictionary<string, string>();
                        

                        //foreach (var oi in ordItemEntity)
                        //{
                        if (orderentity.AssignToCompanyID != model.AssignToCompanyId)
                            historyCols.Add("AssignToCompanyID", orderentity.AssignToCompanyID.ToString());
                        if (orderentity.AssignedToID != model.AssignedToId)
                            historyCols.Add("AssignedToID", orderentity.AssignedToID.ToString());
                        if (orderentity.InstallerInstruction != model.InstallerInstruction)
                            historyCols.Add("InstallerInstruction", orderentity.InstallerInstruction);
                        if (orderentity.StatusID != (int)Enums.Status.OrdAssigned)
                            historyCols.Add("StatusID", orderentity.StatusID.ToString());

                        ////insert InventoryHistory records belong to orderid and InventoryItemID
                        var ordHistory = historyCols.Select(x => new InventoryHistory()
                        {
                            BatchTransactionGUID = batchTransactionID,
                            OrderID = orderentity.OrderID,
                            EntityID = orderentity.OrderID,
                            ApiName = apicallurl,
                            TableName = "Order",
                            ColumnName = x.Key,
                            OldValue = x.Value,
                            NewValue = (
                                        x.Key.Contains("AssignToCompanyID") ? model.AssignToCompanyId.ToString() :
                                        x.Key.Contains("AssignedToID") ? model.AssignedToId.ToString() :
                                        x.Key.Contains("InstallerInstruction") ? model.InstallerInstruction.ToString() :
                                        x.Key.Contains("StatusID") ? ((int)Enums.Status.OrdAssigned).ToString() : ""
                                       ),
                            Description = x.Key + " Updated",
                            CreateDateTime = historyCreateDateTime,
                            CreateID = model.UserId
                        });

                        await _dbContext.AddRangeAsync(ordHistory);
                        await _dbContext.SaveChangesAsync();


                        orderentity.AssignToCompanyID = model.AssignToCompanyId;
                        orderentity.AssignedToID = model.AssignedToId;
                        orderentity.StatusID = (int)Enums.Status.OrdAssigned;
                        orderentity.InstallerInstruction = model.InstallerInstruction;

                        _dbContext.Update(orderentity);
                        await _dbContext.SaveChangesAsync();
                    }

                    await transaction.CommitAsync();
                    return true;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        public async Task<List<OrderTypeModel>> GetOrderTypes()
        {
            try
            {
                return await _dbContext.OrderTypes.Select(x => new OrderTypeModel()
                            { 
                                OrderTypeId = x.OrderTypeID, 
                                OrderTypeName = x.OrderTypeName, 
                                OrderTypeDesc = x.OrderTypeDesc 
                            }
                            ).ToListAsync();
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        public async Task<List<StatusModel>> GetStatus()
        {
            try
            {
                return await _dbContext.Statuses.Where(st=>st.StatusType.Equals("Order Status")).Select(x => new StatusModel()
                {
                    StatusId = x.StatusID,
                    StatusName = x.StatusName                   
                } ).ToListAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> CloseOrder(int orderid, int userid, string apicallurl)
        {
            bool result = false;
            using (IDbContextTransaction transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {

                    var batchTransactionID = System.Guid.NewGuid();
                    DateTime historyCreateDateTime = System.DateTime.Now;

                    var ordItem = await _dbContext.OrderItems.Where(oi => oi.OrderID == orderid).ToListAsync();
                    if(ordItem != null && ordItem.Count > 0)
                    {
                        foreach(var oe in ordItem)
                        {
                            ////insert InventoryHistory records belong to orderid for Orderitem table update                            
                            Dictionary<string, string> historyOrdItemCols = new Dictionary<string, string>();
                            //historyOrdCols.Add("ActionNote", order.ActionNote);
                            historyOrdItemCols.Add("StatusID", oe.StatusID.ToString());

                            var invHistoryOrdItem = historyOrdItemCols.Select(x => new InventoryHistory()
                            {
                                BatchTransactionGUID = batchTransactionID,
                                OrderID = orderid,
                                EntityID = oe.OrderItemID,
                                ApiName = apicallurl,
                                TableName = "OrderItem",
                                ColumnName = x.Key,
                                OldValue = x.Value,
                                NewValue = (
                                                 // x.Key.Contains("ActionNote") ? actionnote :
                                                 x.Key.Contains("StatusID") ? ((int)Enums.Status.OrdClosed).ToString() : ""
                                            ),
                                Description = x.Key + " Updated",
                                CreateDateTime = historyCreateDateTime,
                                CreateID = userid
                            });

                            await _dbContext.AddRangeAsync(invHistoryOrdItem);
                            await _dbContext.SaveChangesAsync();

                            oe.StatusID = (int)Enums.Status.OrdClosed;                         

                            _dbContext.Update(oe);
                            await _dbContext.SaveChangesAsync();
                        }

                        
                    }
                    var order = _dbContext.Orders.Where(ord => ord.OrderID == orderid).FirstOrDefault();

                    if (order != null)
                    {
                        ////insert InventoryHistory records belong to orderid for Order table update                            
                        Dictionary<string, string> historyOrdCols = new Dictionary<string, string>();
                        //historyOrdCols.Add("ActionNote", order.ActionNote);
                        historyOrdCols.Add("StatusID", order.StatusID.ToString());

                        var invHistoryOrd = historyOrdCols.Select(x => new InventoryHistory()
                        {
                            BatchTransactionGUID = batchTransactionID,
                            OrderID = orderid,
                            EntityID = order.OrderID,
                            ApiName = apicallurl,
                            TableName = "Order",
                            ColumnName = x.Key,
                            OldValue = x.Value,
                            NewValue = (
                                            // x.Key.Contains("ActionNote") ? actionnote :
                                             x.Key.Contains("StatusID") ? ((int)Enums.Status.OrdClosed).ToString() : ""
                                        ),
                            Description = x.Key + " Updated",
                            CreateDateTime = historyCreateDateTime,
                            CreateID = userid
                        });

                        await _dbContext.AddRangeAsync(invHistoryOrd);
                        await _dbContext.SaveChangesAsync();

                        order.StatusID = (int)Enums.Status.OrdClosed;
                       

                        _dbContext.Update(order);
                        await _dbContext.SaveChangesAsync();
                    }

                    await transaction.CommitAsync();
                    result = true;


                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
            return result;
        }

        private async Task<List<InventoryOrderItemModel>> GetNewInstallationInventoryItems(int clientid,int conditionid)
        {
            List<InventoryOrderItemModel> invOrderItemModel = new List<InventoryOrderItemModel>();
            try
            {
                var entity = await _dbContext.InventoryItems.Where(ii => ii.ClientID == clientid && ii.ConditionID == conditionid).ToListAsync();
                invOrderItemModel = entity.Select(x => new InventoryOrderItemModel()
                {
                    InventoryItemID = x.InventoryItemID,
                    InventoryID = x.InventoryID,
                    BuildingId = x.InventoryBuildingID,
                    Building = _requestContext.ClientLocations.Where(ib => ib.location_id == x.InventoryBuildingID).FirstOrDefault().location_name,
                    Floor = _dbContext.InventoryFloors.Where(f => f.InventoryFloorID == x.InventoryFloorID).FirstOrDefault().InventoryFloorName,
                    Qty = 1,
                    Room = x.Room,
                    ConditionId = x.ConditionID,
                    Condition = _dbContext.InventoryItemConditions.Where(c => c.InventoryItemConditionID == x.ConditionID).FirstOrDefault().ConditionName,
                    ClientID = x.ClientID,
                    ImageUrl = _configuration.GetValue<string>("ImgUrl") + x.ClientID + "/",
                    BucketName = _configuration.GetValue<string>("AwsConfig:BuketName"),
                    ProposalNumber = x.ProposalNumber,
                    PoOrderNo = x.PoOrderNo.ToString()                    
                }).ToList();

                foreach (var item in invOrderItemModel)
                {
                    var inventory = await _dbContext.Inventories.Where(i => i.InventoryID == item.InventoryID).FirstOrDefaultAsync();
                    if (inventory != null)
                    {
                        item.Category = _dbContext.ItemTypes.Where(it => it.ItemTypeID == inventory.ItemTypeID).FirstOrDefault().ItemTypeName;
                        item.ItemCode = inventory.ItemCode;
                        item.Description = inventory.Description;
                        item.ImageName = inventory.MainImage;
                        item.ImagePath = $"{_configuration.GetValue<string>("AwsConfig:S3Folder")}/{inventory.ClientID}/images/{inventory.MainImage}";
                    }
                }
            }
            catch(Exception ex)
            {
                throw;
            }
       
            
            return invOrderItemModel;
        }
    }
}
