using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using SeattleInventoryApi.DBContext;
using SeattleInventoryApi.DBModels;
using SeattleInventoryApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeattleInventoryApi.Repository
{
    public class InventoryItemOrderRepository : IInventoryItemOrderRepository
    {
        InventoryContext _dbContext;
        IConfiguration _configuration { get; }
        IEmailNotificationRepository _emailNotificationRepository { get; }
        IHDTicketRepository _hdTicketRepository { get; }

        public InventoryItemOrderRepository(InventoryContext dbContext, IConfiguration configuration
                                            ,IEmailNotificationRepository emailNotificationRepository
                                            ,IHDTicketRepository hdTicketRepository
                                            )
        {
            _dbContext = dbContext;
            _configuration = configuration;
            _emailNotificationRepository = emailNotificationRepository;
            _hdTicketRepository = hdTicketRepository;
        }

        public bool InsertInventoryOrder(InventoryOrderModel model)
        {
            bool result = false;
            using (IDbContextTransaction transaction = _dbContext.Database.BeginTransaction())
            {
                try
                { 
                    Order orderModel = new Order();

                    orderModel.email = model.requestoremail;
                    orderModel.project = model.request_individual_project;
                    orderModel.location = "";
                    orderModel.room = model.destination_room;
                    orderModel.depcostcenter = model.department_cost_center;
                   // ordeModel.instdate = TimeZoneInfo.ConvertTimeFromUtc(model.requested_inst_date.Date,TimeZoneInfo.Local).Date;                    
                    //orderModel.instdate = TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(model.requested_inst_date),TimeZoneInfo.Local);                    
                    orderModel.instdate = Convert.ToDateTime(model.requested_inst_date);                    
                    //orderModel.instdate = System.DateTime.Now;                    
                    orderModel.comments = model.comments;
                    orderModel.destb = model.destination_building;
                    orderModel.destf = model.destination_floor;
                    orderModel.instdater = System.DateTime.Now;
                    
                    _dbContext.Add(orderModel);
                    _dbContext.SaveChanges();

                    int order_id = orderModel.order_id;

                    var orderItemModel = model.cart_item;

                    InsertInventoryOrderItem(order_id, orderItemModel);

                    var flagrl = InsertInventoryItemRivisonLog(model);

                    var flaginvitem = UpdateInventoryItem(orderModel, orderItemModel);

                    //if(order_id > 0 && flagrl && flaginvitem)
                    //{

                    //}

                    SendOrderEmailNotification(model); 

                    transaction.Commit();

                    result = true;

                }
                catch (Exception ex)
                {
                    transaction.RollbackAsync();
                    throw;
                }
            }

            return result;

        }

        private void InsertInventoryOrderItem(int order_id, List<Cart> cart)
        {
            try
            {
                var orderItemList = cart.Select(x => new OrderItem()
                {
                    inv_id = x.inventory_id,
                    inv_item_id = x.inv_item_id,
                    ic = x.item_code,
                    order_id = order_id,
                    qty = x.pullqty
                }).ToList();

                _dbContext.AddRange(orderItemList);
                _dbContext.SaveChanges();

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private bool UpdateInventoryItem(Order ordModel , List<Cart> cart)
        {
            try
            {
                foreach (var item in cart)
                {
                    List<InventoryItem> entity = new List<InventoryItem>();

                    if (item.pullqty > 1)
                      entity = _dbContext.InventoryItems.Where(ii => ii.inventory_id == item.inventory_id && ii.cond == item.cond && ii.inv_item_id >= item.inv_item_id).Take(item.pullqty).ToList();
                    else
                      entity = _dbContext.InventoryItems.Where(ii => ii.inventory_id == item.inventory_id && ii.cond == item.cond && ii.inv_item_id == item.inv_item_id).Take(item.pullqty).ToList();

                    foreach(var e in entity)
                    {
                        e.building = ordModel.destb;
                        e.floor = ordModel.destf;
                        e.mploc = ordModel.room;

                        _dbContext.Update(e);
                        _dbContext.SaveChanges();
                    }                  
                }
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private bool InsertInventoryItemRivisonLog(InventoryOrderModel model)
        {
            try
            {
                foreach (var item in model.cart_item)
                {
                    List<InventoryItem> inv_item_rv_log = new List<InventoryItem>();

                    if (item.pullqty > 1)
                        inv_item_rv_log = _dbContext.InventoryItems.Where(ii => ii.inventory_id == item.inventory_id && ii.cond == item.cond && ii.inv_item_id >= item.inv_item_id).Take(item.pullqty).ToList();
                    else
                        inv_item_rv_log = _dbContext.InventoryItems.Where(ii => ii.inventory_id == item.inventory_id && ii.cond == item.cond && ii.inv_item_id == item.inv_item_id).Take(item.pullqty).ToList();

                    //var inv_item_rv_log = _dbContext.InventoryItems.Where(ii => ii.inventory_id == item.inventory_id && ii.cond == item.cond).Take(item.pullqty).ToList();

                    var original_createdate = _dbContext.Inventories.Where(i => i.inventory_id == item.inventory_id).FirstOrDefault().createdate;

                    var rv_log_list = inv_item_rv_log.Select(x => new InventoryItemRevisionLog()
                    {
                        barcode = x.barcode,
                        building = x.building,
                        client_id = x.client_id,
                        cond = x.cond,
                        display_on_site = x.display_on_site,
                        external_id = x.external_id,
                        floor = x.floor,
                        gps_location = x.gps_location,
                        inventory_id = x.inventory_id,
                        inv_item_id = x.inv_item_id,
                        location_id = x.location_id,
                        mploc = x.mploc,
                        notes = x.notes,
                        rfid = x.rfid,
                        dest_building = model.destination_building,
                        dest_floor = model.destination_floor,
                        dest_mploc=model.destination_room,
                        original_create_date = original_createdate,
                        status="Moving Location",
                        dep_cost_center = model.department_cost_center
                    }).ToList();

                    _dbContext.AddRange(rv_log_list);
                    _dbContext.SaveChanges();
                }

                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void SendOrderEmailNotification(InventoryOrderModel model)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                foreach (var item in model.cart_item)
                {
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
                    sb.Append("<td><img src='" + item.inv_image_url + "' height='80px' width='80px'/></td>");

                    sb.Append("<td>" + item.pullqty + "</td>");
                    sb.Append("<td>" + item.item_code + "</td>");
                    sb.Append("<td>" + item.description + "</td>");
                    sb.Append("</tr></table>");
                }

                string body = $"<html><pre>A new {model.cart_item[0].client_name} order came in :\nEmail:{model.requestoremail}\n" +
                                 $"Request for :{model.request_individual_project}\nDestination Building:{model.destination_building}\nDestination Floor :{model.destination_floor}\n" +
                                 $"Room / Floor / Area :{model.destination_room}\n" +
                                 $"Requested Installation Date:{TimeZoneInfo.ConvertTimeToUtc(DateTime.SpecifyKind(Convert.ToDateTime(model.requested_inst_date), DateTimeKind.Unspecified), TimeZoneInfo.Local)}" +
                                 $"\nComments:{model.comments}</pre>" +
                                 $"{sb}</html>";

                _emailNotificationRepository.SendEmail(body, model.request_individual_project, model.requestoremail, model.cart_item[0].client_name);
                //_emailNotificationRepository.SendGmailEmail(body, model.request_individual_project);

                var hdticketresult = _hdTicketRepository.CreateHDTicket(model, body);
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        public async Task<List<InventoryOrderItemModel>> GetInventoryOrderItems()
        {
            List<InventoryOrderItemModel> result = new List<InventoryOrderItemModel>();
            try
            {
                result = (from i in _dbContext.Inventories
                          join ii in _dbContext.InventoryItems on i.inventory_id equals ii.inventory_id
                          join oi in _dbContext.OrderItems on ii.inv_item_id equals oi.inv_item_id
                          join o in _dbContext.Orders on oi.order_id equals o.order_id
                          where oi.completed == false
                          select new InventoryOrderItemModel()
                          {
                              order_id = o.order_id,
                              inv_id = oi.inv_id,
                              inv_item_id = oi.inv_item_id,
                              email = o.email,
                              project = o.project,
                              instdate = o.instdate,
                              destb = o.destb,
                              destf = o.destf,
                              room = o.room,
                              qty = oi.qty,
                              category = i.category,
                              item_code = i.item_code,
                              description = i.description,
                              building = ii.building,
                              floor = ii.floor,
                              mploc = ii.mploc
                          }).AsQueryable().ToList();


                foreach (var item in result)
                {
                    var base64String = await ConvertImageToBase64String(_configuration.GetValue<string>("ImgUrl") + item.item_code + ".jpg");
                    if(!string.IsNullOrEmpty(base64String))
                    {
                        base64String = "data:image/jpeg;base64," + base64String;
                        result.Where(r => r.item_code == item.item_code).Select(s => { s.imgBase64 = base64String; return s; }).ToList();
                    }
                }

                return await Task.Run(() => result);
                //return result;

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private async Task<string> ConvertImageToBase64String(string imgurl)
        {
            using (var client = new System.Net.Http.HttpClient())
            {
                if (File.Exists(imgurl))
                {
                    var bytes = await File.ReadAllBytesAsync(imgurl); //local folder/server folder image convert to byte so, base on need do uncomment and comment
                    ////byte[] imageBytes = Convert.FromBase64String(imgurl); 
                    //var bytes = await client.GetByteArrayAsync(imgurl); //http url image convert to byte so, base on need do uncomment and comment
                    var base64String = Convert.ToBase64String(bytes);
                    return base64String;
                }
                else
                    return null;
            }
                
        }

        public bool InsertWarrantyRequest(InventoryOrderModel model)
        {
            bool result = SendWarrantyRequestEmailNotification(model);
            return result;
        }

        private bool SendWarrantyRequestEmailNotification(InventoryOrderModel model)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                foreach (var item in model.cart_item)
                {
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
                    sb.Append("<td><img src='" + item.inv_image_url + "' height='80px' width='80px'/></td>");

                    sb.Append("<td>" + item.pullqty + "</td>");
                    sb.Append("<td>" + item.item_code + "</td>");
                    sb.Append("<td>" + item.description + "</td>");
                    sb.Append("</tr></table>");
                }

                string body = $"<html><pre>A new {model.cart_item[0].client_name} warranty request came in :\nEmail:{model.requestoremail}\n" +
                                 $"Request for :{model.request_individual_project}\n" +
                                 $"\nComments:{model.comments}</pre>" +
                                 $"{sb}</html>";

                //_emailNotificationRepository.SendEmail(body, model.request_individual_project, model.requestoremail, model.cart_item[0].client_name);
                //_emailNotificationRepository.SendGmailEmail(body, model.request_individual_project);

                var hdticketresult = _hdTicketRepository.CreateWarrentyRequestHDTicket(model, body);
                return hdticketresult;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
