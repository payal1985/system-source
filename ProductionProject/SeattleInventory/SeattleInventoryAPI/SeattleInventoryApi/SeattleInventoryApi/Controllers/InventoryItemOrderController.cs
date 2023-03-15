using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NLog;
using SeattleInventoryApi.Models;
using SeattleInventoryApi.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SeattleInventoryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryItemOrderController : ControllerBase
    {
        private readonly IInventoryItemOrderRepository _inventoryItemOrderRepository;
        //private readonly IEmailNotificationRepository _emailNotificationRepository;
        //private readonly IHDTicketRepository _hDTicketRepository;
        private readonly ILoggerManagerRepository _logger;


        public InventoryItemOrderController(IInventoryItemOrderRepository inventoryItemOrderRepository, ILoggerManagerRepository logger
                                            //,IEmailNotificationRepository emailNotificationRepository
                                            //,IHDTicketRepository hDTicketRepository
                                            )
        {
            //logger = LogManager.GetCurrentClassLogger();
            _logger = logger;
            _inventoryItemOrderRepository = inventoryItemOrderRepository;
            //_emailNotificationRepository = emailNotificationRepository;
            //_hDTicketRepository = hDTicketRepository;

            _logger.LogInfo("Initialize the Inventory Item Order Controller");

        }



        [HttpGet("getinventoryorderitem")]
        public async Task<IActionResult> GetInventoryorderItem()
        {
            try
            {
                var result = await _inventoryItemOrderRepository.GetInventoryOrderItems();
                _logger.LogInfo($"GetInventoryorderItem  Request in InventoryItemOrder Controller return result with inventory orders successfully...");

                return Ok(result);
            }
            catch(Exception ex)
            {
                string errMsg = $"Exception occured due to call GetInventoryorderItem Request=>{ex.Message} \n {ex.StackTrace} \n {ex.InnerException}";
                _logger.LogError($"GetInventoryorderItem  Request in InventoryItemOrder Controller=>{errMsg}");
                //throw new Exception($"Exception occured to load inventory orders \n {errMsg}");

                //var errMsg = $"Exception Occured-> {ex.Message}\n{ex.InnerException}\n{ex.StackTrace}";
                return BadRequest(errMsg);
            }
        }


        [HttpPost("postinventoryitemorder")]
        public IActionResult PostInventoryItemOrder([FromBody] InventoryOrderModel model)
        {
            try
            {
                var result = _inventoryItemOrderRepository.InsertInventoryOrder(model);
                _logger.LogInfo($"PostInventoryItemOrder  Request in InventoryItemOrder Controller return result successfully...");

                #region oldcode
                //if (result)
                //{
                //   // string imgpath = @"C:/Users/Payal/source/repos/ProductionProject/SeattleInventory/SeattleInventoryPortal/seattle-inventory-portal/src/assets/images/inventoryimg/";
                //    StringBuilder sb = new StringBuilder();
                //    foreach (var item in model.cart_item)
                //    {
                //        //sb.Append("<table style='cellspacing:0; cellpadding:5; border:1px solid gray;'>");
                //        sb.Append("<table cellspacing=0 cellpadding=5 border=1>");

                //        //sb.Append("<tr style='border:1px solid gray;'>");
                //        sb.Append("<tr>");

                //        ////sb.Append("<td><img src='" + imgpath + item.inv_image_name + "' height='100px' width='100px'/></td>");
                //        ////sb.Append("<td style='border:1px solid gray;'><img src='http://dev.systemsource.com/static/RL/MicrosoftImageTest.pl?p_image_ID=2' height='100px' width='100px'/></td>");
                //        //sb.Append("<td style='border:1px solid gray;'><img src='"+item.inv_image_url+"' height='100px' width='100px'/></td>");

                //        //sb.Append("<td style='border:1px solid gray;'>" + item.pullqty + "</td>");
                //        //sb.Append("<td style='border:1px solid gray;'>" + item.item_code + "</td>");
                //        //sb.Append("<td style='border:1px solid gray;'>" + item.description + "</td>");
                //        sb.Append("<td><img src='http://dev.systemsource.com/static/RL/MicrosoftImageTest.pl?p_image_ID=2' height='100px' width='100px'/></td>");
                //        //sb.Append("<td><img src='" + item.inv_image_url + "' height='100px' width='100px'/></td>");

                //        sb.Append("<td>" + item.pullqty + "</td>");
                //        sb.Append("<td>" + item.item_code + "</td>");
                //        sb.Append("<td>" + item.description + "</td>");
                //        sb.Append("</tr></table>");
                //    }


                //    string body = $"<html><pre>A new Seattle order came in :\nEmail:{model.requestoremail}\n" +
                //                     $"Request for :{model.request_individual_project}\nDestination Building:{model.destination_building}\nDestination Floor :{model.destination_floor}\n" +
                //                     $"Room / Floor / Area :{model.destination_location}\nRequested Installation Date:{TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(model.requested_inst_date), TimeZoneInfo.Local)}\nComments:{model.comments}</pre>" +
                //                     $"{sb}</html>";

                //    //_emailNotificationRepository.SendEmail(body, model.request_individual_project, model.requestoremail);
                //     _emailNotificationRepository.SendGmailEmail(body, model.request_individual_project);

                //    var hdticketresult = _hDTicketRepository.CreateHDTicket(model,body);

                //}
                #endregion

                return Ok(result);
            }
            catch(Exception ex)
            {
                string errMsg = $"Exception occured due to call PostInventoryItemOrder in InventoryItemOrder Controller Post Request=>{ex.Message} \n {ex.StackTrace} \n {ex.InnerException}";
                _logger.LogError($"PostInventoryItemOrder  Request in InventoryItemOrder Controller=>{errMsg}");
                // var strErrMsg = $"Exception while send order {ex.Message} \n {ex.InnerException} \n {ex.StackTrace}";
                //return BadRequest(errMsg);
                throw new Exception($"Exception occured to post order \n {errMsg}");

            }

        }

        
        [HttpPost("saveinventoryitemorder")]
        public IActionResult SaveInventoryItemOrder([FromBody] List<InventoryCartOrderItemModel> list)
        {
            bool result = false;
            try
            {
                var grpResult = list.GroupBy
                    (x => new { x.email, x.reqname, x.destbuilding, x.destfloor, x.destroom,x.destdepcostcenter,
                        x.inst_date, x.comment })
                    .Select(key =>
                        new InventoryOrderModel
                        {
                            requestoremail = key.Key.email,
                            //request_individual_project = key.Key.username,
                            request_individual_project = key.Key.reqname,
                            destination_building = key.Key.destbuilding,
                            destination_floor = key.Key.destfloor,
                            destination_room = key.Key.destroom,
                            requested_inst_date = key.Key.inst_date,
                            comments = key.Key.comment,
                            department_cost_center = key.Key.destdepcostcenter,
                            cart_item = key.Select(k=>new Cart{
                               inv_item_id = k.inv_item_id,
                                building = k.building,
                                floor = k.floor,
                                mploc = k.mploc ,
                                cond = k.cond ,
                                qty = k.qty ,
                                inventory_id = k.inventory_id ,
                                inv_image_name =  k.inv_image_name ,
                                inv_image_url =  k.inv_image_url,
                                item_code =  k.item_code,
                                description =  k.description,
                                pullqty =  k.pullqty,
                                client_id =  k.client_id,
                                client_name = k.client_name
                            }).ToList()
                        }).ToList();

                foreach (var model in grpResult)
                {
                    //var newdate = Convert.ToDateTime(model.requested_inst_date);
                    //var pstdate = DateTime.SpecifyKind(newdate, DateTimeKind.Local);

                    //TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById("Pacific SA Standard Time");
                    //DateTime kstTime = TimeZoneInfo.ConvertTimeFromUtc(newdate, timeZone);

                    //                    DateTime kstTime =
                    //TimeZoneInfo.ConvertTimeToUtc(DateTime.SpecifyKind(model.requested_inst_date, DateTimeKind.Unspecified), TimeZoneInfo.Local);

                    // TimeZoneInfo.ConvertF(Convert.ToDateTime(model.requested_inst_date), TimeZoneInfo.Local)
                    result = _inventoryItemOrderRepository.InsertInventoryOrder(model);
                }
                //result = true;
                _logger.LogInfo($"SaveInventoryItemOrder  Request in InventoryItemOrder Controller return result successfully...");


                return Ok(result);
            }
            catch (Exception ex)
            {
                string errMsg = $"Exception occured due to call SaveInventoryItemOrder in InventoryItemOrder Controller Post Request=>{ex.Message} \n {ex.StackTrace} \n {ex.InnerException}";
                _logger.LogError($"SaveInventoryItemOrder  Request in InventoryItemOrder Controller=>{errMsg}");
                // var strErrMsg = $"Exception while send order {ex.Message} \n {ex.InnerException} \n {ex.StackTrace}";
                //return BadRequest(errMsg);
                throw new Exception($"Exception occured to post order \n {errMsg}");

            }

        }

        [HttpPost("savewarrantyrequest")]
        public IActionResult SaveWarrantyRequest([FromBody] List<InventoryCartOrderItemModel> list)
        {
            bool result = false;
            try
            {
                var grpResult = list.GroupBy
                    (x => new {
                        x.email,
                        x.reqname,                        
                        x.comment
                    })
                    .Select(key =>
                        new InventoryOrderModel
                        {
                            requestoremail = key.Key.email,
                            //request_individual_project = key.Key.username,
                            request_individual_project = key.Key.reqname,                   
                            comments = key.Key.comment,                           
                            cart_item = key.Select(k => new Cart
                            {
                                inv_item_id = k.inv_item_id,
                                building = k.building,
                                floor = k.floor,
                                mploc = k.mploc,
                                cond = k.cond,
                                qty = k.qty,
                                inventory_id = k.inventory_id,
                                inv_image_name = k.inv_image_name,
                                inv_image_url = k.inv_image_url,
                                item_code = k.item_code,
                                description = k.description,
                                pullqty = k.pullqty,
                                client_id = k.client_id,
                                client_name = k.client_name
                            }).ToList()
                        }).ToList();

                foreach (var model in grpResult)
                {
                    result = _inventoryItemOrderRepository.InsertWarrantyRequest(model);
                }
                //result = true;
                _logger.LogInfo($"SaveWarrantyRequest  Request in InventoryItemOrder Controller return result successfully...");


                return Ok(result);
            }
            catch (Exception ex)
            {
                string errMsg = $"Exception occured due to call SaveWarrantyRequest in InventoryItemOrder Controller Post Request=>{ex.Message} \n {ex.StackTrace} \n {ex.InnerException}";
                _logger.LogError($"SaveWarrantyRequest  Request in InventoryItemOrder Controller=>{errMsg}");
                // var strErrMsg = $"Exception while send order {ex.Message} \n {ex.InnerException} \n {ex.StackTrace}";
                //return BadRequest(errMsg);
                throw new Exception($"Exception occured to post order \n {errMsg}");

            }
        }
    }
}
