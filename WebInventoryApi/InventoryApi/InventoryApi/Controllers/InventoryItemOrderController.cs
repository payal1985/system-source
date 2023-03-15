using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NLog;
using InventoryApi.Models;
using InventoryApi.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InventoryApi.Repository.Interfaces;
using Newtonsoft.Json;
using System.Net;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace InventoryApi.Controllers
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



        [HttpGet("getinventoryorderitem/{clientid}/{ordertypeid}/{statusid}")]
        public async Task<IActionResult> GetInventoryorderItem(int clientid,int ordertypeid,int statusid)
        {
            try
            {
                var result = await _inventoryItemOrderRepository.GetInventoryOrderItems(clientid, ordertypeid,statusid);
                _logger.LogInfo($"GetInventoryorderItem  Request in InventoryItemOrder Controller return result with inventory orders successfully...");

                return Ok(result);
            }
            catch(Exception ex)
            {
                string errMsg = $"Exception occured due to call GetInventoryorderItem Request=>{ex.Message} \n {ex.StackTrace} \n {ex.InnerException}";
                _logger.LogError($"GetInventoryorderItem  Request in InventoryItemOrder Controller=>{errMsg}");
                return Problem(errMsg, statusCode: (int)HttpStatusCode.BadRequest);
            }
        }

        //[HttpGet("expandinventoryorderitem/{orderid}/{destbuildingid}/{destfloorid}/{destroom}")]
        [HttpGet("expandinventoryorderitem/{orderid}/{destbuildingid}/{destfloorid}")]
        public async Task<IActionResult> ExpandInventoryorderItem(int orderid,int destbuildingid,int destfloorid,string? destroom)
        {
            try
            {
                var result = await _inventoryItemOrderRepository.ExpandInventoryOrderItems(orderid, destbuildingid, destfloorid, destroom);
                _logger.LogInfo($"ExpandInventoryorderItem  Request in InventoryItemOrder Controller return result with inventory orders successfully...");

                return Ok(result);
            }
            catch (Exception ex)
            {
                string errMsg = $"Exception occured due to call ExpandInventoryorderItem Request=>{ex.Message} \n {ex.StackTrace} \n {ex.InnerException}";
                _logger.LogError($"ExpandInventoryorderItem  Request in InventoryItemOrder Controller=>{errMsg}");
                return Problem(errMsg, statusCode: (int)HttpStatusCode.BadRequest);
            }
        }

        [HttpGet("getordertypes")]
        public async Task<IActionResult> GetOrderTypes()
        {
            try
            {
                var result = await _inventoryItemOrderRepository.GetOrderTypes();
                _logger.LogInfo($"GetOrderTypes  Request in InventoryItemOrder Controller return result with inventory orders successfully...");

                return Ok(result);
            }
            catch (Exception ex)
            {
                string errMsg = $"Exception occured due to call GetOrderTypes Request=>{ex.Message} \n {ex.StackTrace} \n {ex.InnerException}";
                _logger.LogError($"GetOrderTypes  Request in InventoryItemOrder Controller=>{errMsg}");
                return Problem(errMsg, statusCode: (int)HttpStatusCode.BadRequest);
            }
        }

        [HttpGet("getstatus")]
        public async Task<IActionResult> GetStatus()
        {
            try
            {
                var result = await _inventoryItemOrderRepository.GetStatus();
                _logger.LogInfo($"GetStatus  Request in InventoryItemOrder Controller return result with inventory orders successfully...");

                return Ok(result);
            }
            catch (Exception ex)
            {
                string errMsg = $"Exception occured due to call GetStatus Request=>{ex.Message} \n {ex.StackTrace} \n {ex.InnerException}";
                _logger.LogError($"GetStatus  Request in InventoryItemOrder Controller=>{errMsg}");
                return Problem(errMsg, statusCode: (int)HttpStatusCode.BadRequest);
            }
        }
        //[HttpPost("postinventoryitemorder")]
        //public IActionResult PostInventoryItemOrder([FromBody] InventoryOrderModel model)
        //{
        //    try
        //    {
        //        var result = _inventoryItemOrderRepository.InsertInventoryOrder(model);
        //        _logger.LogInfo($"PostInventoryItemOrder  Request in InventoryItemOrder Controller return result successfully...");

        //        #region oldcode
        //        //if (result)
        //        //{
        //        //   // string imgpath = @"C:/Users/Payal/source/repos/ProductionProject/SeattleInventory/SeattleInventoryPortal/seattle-inventory-portal/src/assets/images/inventoryimg/";
        //        //    StringBuilder sb = new StringBuilder();
        //        //    foreach (var item in model.cart_item)
        //        //    {
        //        //        //sb.Append("<table style='cellspacing:0; cellpadding:5; border:1px solid gray;'>");
        //        //        sb.Append("<table cellspacing=0 cellpadding=5 border=1>");

        //        //        //sb.Append("<tr style='border:1px solid gray;'>");
        //        //        sb.Append("<tr>");

        //        //        ////sb.Append("<td><img src='" + imgpath + item.inv_image_name + "' height='100px' width='100px'/></td>");
        //        //        ////sb.Append("<td style='border:1px solid gray;'><img src='http://dev.systemsource.com/static/RL/MicrosoftImageTest.pl?p_image_ID=2' height='100px' width='100px'/></td>");
        //        //        //sb.Append("<td style='border:1px solid gray;'><img src='"+item.inv_image_url+"' height='100px' width='100px'/></td>");

        //        //        //sb.Append("<td style='border:1px solid gray;'>" + item.pullqty + "</td>");
        //        //        //sb.Append("<td style='border:1px solid gray;'>" + item.item_code + "</td>");
        //        //        //sb.Append("<td style='border:1px solid gray;'>" + item.description + "</td>");
        //        //        sb.Append("<td><img src='http://dev.systemsource.com/static/RL/MicrosoftImageTest.pl?p_image_ID=2' height='100px' width='100px'/></td>");
        //        //        //sb.Append("<td><img src='" + item.inv_image_url + "' height='100px' width='100px'/></td>");

        //        //        sb.Append("<td>" + item.pullqty + "</td>");
        //        //        sb.Append("<td>" + item.item_code + "</td>");
        //        //        sb.Append("<td>" + item.description + "</td>");
        //        //        sb.Append("</tr></table>");
        //        //    }


        //        //    string body = $"<html><pre>A new Seattle order came in :\nEmail:{model.requestoremail}\n" +
        //        //                     $"Request for :{model.request_individual_project}\nDestination Building:{model.destination_building}\nDestination Floor :{model.destination_floor}\n" +
        //        //                     $"Room / Floor / Area :{model.destination_location}\nRequested Installation Date:{TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(model.requested_inst_date), TimeZoneInfo.Local)}\nComments:{model.comments}</pre>" +
        //        //                     $"{sb}</html>";

        //        //    //_emailNotificationRepository.SendEmail(body, model.request_individual_project, model.requestoremail);
        //        //     _emailNotificationRepository.SendGmailEmail(body, model.request_individual_project);

        //        //    var hdticketresult = _hDTicketRepository.CreateHDTicket(model,body);

        //        //}
        //        #endregion

        //        return Ok(result);
        //    }
        //    catch(Exception ex)
        //    {
        //        string errMsg = $"Exception occured due to call PostInventoryItemOrder in InventoryItemOrder Controller Post Request=>{ex.Message} \n {ex.StackTrace} \n {ex.InnerException}";
        //        _logger.LogError($"PostInventoryItemOrder  Request in InventoryItemOrder Controller=>{errMsg}");
        //        // var strErrMsg = $"Exception while send order {ex.Message} \n {ex.InnerException} \n {ex.StackTrace}";
        //        //return BadRequest(errMsg);
        //        throw new Exception($"Exception occured to post order \n {errMsg}");

        //    }

        //}


        [HttpPost("saveinventoryitemorder")]
        public async Task<IActionResult> SaveInventoryItemOrder([FromBody] List<InventoryCartOrderItemModel> list)
        {
            bool result = false;
            try
            {
                #region oldcode
                //var grpResult = list.GroupBy
                //    (x => new { x.email, x.reqname, x.destbuilding, x.destfloor, x.destroom,x.destdepcostcenter,
                //        x.inst_date, x.comment })
                //    .Select(key =>
                //        new InventoryOrderModel
                //        {
                //            requestoremail = key.Key.email,
                //            //request_individual_project = key.Key.username,
                //            request_individual_project = key.Key.reqname,
                //            destination_building = key.Key.destbuilding,
                //            destination_floor = key.Key.destfloor,
                //            destination_room = key.Key.destroom,
                //            requested_inst_date = key.Key.inst_date,
                //            comments = key.Key.comment,
                //            department_cost_center = key.Key.destdepcostcenter,
                //            cart_item = key.Select(k=>new Cart{
                //               inv_item_id = k.inv_item_id,
                //                building = k.building,
                //                floor = k.floor,
                //                mploc = k.mploc ,
                //                cond = k.cond ,
                //                qty = k.qty ,
                //                inventory_id = k.inventory_id ,
                //                inv_image_name =  k.inv_image_name ,
                //                inv_image_url =  k.inv_image_url,
                //                item_code =  k.item_code,
                //                description =  k.description,
                //                pullqty =  k.pullqty,
                //                client_id =  k.client_id,
                //                client_name = k.client_name
                //            }).ToList()
                //        }).ToList();
                #endregion

                var grpResult = list.GroupBy
                   (x => new
                   {
                       x.Email,
                       x.ReqName                       
                   })
                   .Select(key =>
                       new InventoryOrderModel
                       {
                           RequestorEmail = key.Key.Email,
                           RequestorProjectName = key.Key.ReqName,                          
                           CartItem = key.Select(k => new Cart
                           {
                               InventoryItemID = k.InventoryItemID,
                               BuildingId = k.InventoryBuildingID,
                               FloorId = k.InventoryFloorID,
                               Room = k.Room,
                               Condition = k.Condition,
                               ConditionId = k.ConditionId,
                               Qty = k.Qty,
                               InventoryID = k.InventoryID,
                               InventoryImageName = k.InventoryImageName,
                               InventoryImageUrl = k.InventoryImageUrl,
                               ItemCode = k.ItemCode,
                               Description = k.Description,
                               PullQty = k.PullQty,
                               ClientID = k.ClientID,
                               ClientName = k.ClientName,
                               ClientPath = k.ClientPath,
                               UserId = k.UserId,
                               ChildInventoryItemModels = k.ChildInventoryItemModels,
                               DestBuilding = k.DestBuilding,
                               DestFloor = k.DestFloor,
                               DestRoom = k.DestRoom,
                               InstDate = k.InstDate,
                               Comments = k.Comment,
                               DepartmentCostCenter = k.DestDepCostCenter,
                               DestInventoryBuildingID = k.DestInventoryBuildingID,
                               DestInventoryFloorID = k.DestInventoryFloorID,
                           }).ToList()

                       }).ToList();

                //var grpResult = list.GroupBy
                //   (x => new {
                //       x.Email,
                //       x.ReqName,
                //       x.DestBuilding,
                //       x.DestFloor,
                //       x.DestRoom,
                //       x.DestDepCostCenter,
                //       x.InstDate,
                //       x.Comment,
                //       x.DestInventoryBuildingID,
                //       x.DestInventoryFloorID
                //   })
                //   .Select(key =>
                //       new InventoryOrderModel
                //       {
                //           RequestorEmail = key.Key.Email,
                //           RequestorProjectName = key.Key.ReqName,
                //           DestBuilding = key.Key.DestBuilding,
                //           DestFloor = key.Key.DestFloor,
                //           DestRoom = key.Key.DestRoom,
                //           InstDate = key.Key.InstDate,
                //           Comments = key.Key.Comment,
                //           DepartmentCostCenter = key.Key.DestDepCostCenter,
                //           DestInventoryBuildingID = key.Key.DestInventoryBuildingID,
                //           DestInventoryFloorID = key.Key.DestInventoryFloorID,                          
                //           CartItem = key.Select(k => new Cart
                //           {
                //               InventoryItemID = k.InventoryItemID,
                //               BuildingId = k.InventoryBuildingID,
                //               FloorId = k.InventoryFloorID,
                //               Room = k.Room,
                //               Condition = k.Condition,
                //               ConditionId = k.ConditionId,
                //               Qty = k.Qty,
                //               InventoryID = k.InventoryID,
                //               InventoryImageName = k.InventoryImageName,
                //               InventoryImageUrl = k.InventoryImageUrl,
                //               ItemCode = k.ItemCode,
                //               Description = k.Description,
                //               PullQty = k.PullQty,
                //               ClientID = k.ClientID,
                //               ClientName = k.ClientName,
                //               ClientPath = k.ClientPath,
                //               UserId = k.UserId,
                //               ChildInventoryItemModels = k.ChildInventoryItemModels
                //           }).ToList()

                //       }).ToList();

                foreach (var model in grpResult)
                {
                    //var newdate = Convert.ToDateTime(model.requested_inst_date);
                    //var pstdate = DateTime.SpecifyKind(newdate, DateTimeKind.Local);

                    //TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById("Pacific SA Standard Time");
                    //DateTime kstTime = TimeZoneInfo.ConvertTimeFromUtc(newdate, timeZone);

                    //                    DateTime kstTime =
                    //TimeZoneInfo.ConvertTimeToUtc(DateTime.SpecifyKind(model.requested_inst_date, DateTimeKind.Unspecified), TimeZoneInfo.Local);

                    // TimeZoneInfo.ConvertF(Convert.ToDateTime(model.requested_inst_date), TimeZoneInfo.Local)
                    result = await _inventoryItemOrderRepository.InsertInventoryOrder(model);
                    if (!result)
                        break;
                }
                //result = true;
                _logger.LogInfo($"SaveInventoryItemOrder  Request in InventoryItemOrder Controller return result successfully...");


                return Ok(result);
            }
            catch (Exception ex)
            {
                string errMsg = $"Exception occured due to call SaveInventoryItemOrder in InventoryItemOrder Controller Post Request=>{ex.Message} \n {ex.StackTrace} \n {ex.InnerException}";
                _logger.LogError($"SaveInventoryItemOrder  Request in InventoryItemOrder Controller=>{errMsg}");
                return Problem(errMsg, statusCode: (int)HttpStatusCode.BadRequest);

            }

        }

        //[HttpPut("completeorder/{orderid}/{destbuildingid}/{destfloorid}/{destroom}/{userid}/{apicallurl}")]
        //public async Task<IActionResult> CompleteOrder(int orderid, int destbuildingid, int destfloorid, string destroom, int userid,string apicallurl,[FromBody] string json)
        //{ 
        //    try
        //    {
        //        apicallurl = WebUtility.UrlDecode(apicallurl);
        //        string actionnote = json;

        //        var result = await _inventoryItemOrderRepository.CompleteOrder(orderid, destbuildingid, destfloorid, destroom, userid, apicallurl,actionnote);
        //        _logger.LogInfo($"UpdateOrder  Request in InventoryItemOrder Controller updated record successfully...");

        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        string errMsg = $"Exception occured due to call Put UpdateOrder in InventoryItemOrder Controller Put Request=>{ex.Message} \n {ex.StackTrace} \n {ex.InnerException}";
        //        _logger.LogError($"UpdateOrder  Request in InventoryItemOrder Controller=>{errMsg}");
        //        return Problem(errMsg, statusCode: (int)HttpStatusCode.BadRequest);
        //    }
        //}

        [HttpPut("completeorder/{orderid}/{userid}/{apicallurl}")]
        public async Task<IActionResult> CompleteOrder(int orderid,int userid, string apicallurl, [FromBody] string json)
        {
            try
            {
                apicallurl = WebUtility.UrlDecode(apicallurl);
                string actionnote = json;

                var result = await _inventoryItemOrderRepository.CompleteOrder(orderid, userid, apicallurl, actionnote);
                _logger.LogInfo($"UpdateOrder  Request in InventoryItemOrder Controller updated record successfully...");

                return Ok(result);
            }
            catch (Exception ex)
            {
                string errMsg = $"Exception occured due to call Put UpdateOrder in InventoryItemOrder Controller Put Request=>{ex.Message} \n {ex.StackTrace} \n {ex.InnerException}";
                _logger.LogError($"UpdateOrder  Request in InventoryItemOrder Controller=>{errMsg}");
                return Problem(errMsg, statusCode: (int)HttpStatusCode.BadRequest);
            }
        }
       
        [HttpPut("completeorderitem/{orderitemid}/{userid}/{apicallurl}")]
        public async Task<IActionResult> CompleteOrderItem(int orderitemid, int userid, string apicallurl, [FromBody] string json)
        {
            try
            {
                apicallurl = WebUtility.UrlDecode(apicallurl);
                string actionnote = json;

                var result = await _inventoryItemOrderRepository.CompleteOrderItem(orderitemid, userid, apicallurl, actionnote);
                _logger.LogInfo($"UpdateOrder  Request in InventoryItemOrder Controller updated record successfully...");

                return Ok(result);
            }
            catch (Exception ex)
            {
                string errMsg = $"Exception occured due to call Put UpdateOrder in InventoryItemOrder Controller Put Request=>{ex.Message} \n {ex.StackTrace} \n {ex.InnerException}";
                _logger.LogError($"UpdateOrder  Request in InventoryItemOrder Controller=>{errMsg}");
                return Problem(errMsg, statusCode: (int)HttpStatusCode.BadRequest);
            }
        }

        [HttpPut("removecartitem/{inventoryitemid}")]
        public async Task<IActionResult> RemoveCartItem(int inventoryitemid)
        {
            try
            {
                var result = await _inventoryItemOrderRepository.RemoveCartItem(inventoryitemid);
                _logger.LogInfo($"RemoveCartItem  Request in InventoryItemOrder Controller updated record successfully...");

                return Ok(result);
            }
            catch (Exception ex)
            {
                string errMsg = $"Exception occured due to call Put RemoveCartItem in InventoryItemOrder Controller Put Request=>{ex.Message} \n {ex.StackTrace} \n {ex.InnerException}";
                _logger.LogError($"RemoveCartItem  Request in InventoryItemOrder Controller=>{errMsg}");
                return Problem(errMsg, statusCode: (int)HttpStatusCode.BadRequest);
            }
        }

        [HttpPut("cancelorder/{orderid}/{destbuildingid}/{destfloorid}/{destroom}/{userid}/{apicallurl}")]
        public async Task<IActionResult> CancelOrder(int orderid, int destbuildingid, int destfloorid, string destroom, int userid, string apicallurl)
        {
            try
            {
                apicallurl = WebUtility.UrlDecode(apicallurl);
                var result = await _inventoryItemOrderRepository.CancelOrder(orderid, destbuildingid, destfloorid, destroom, userid,apicallurl);
                _logger.LogInfo($"CancelOrder  Request in InventoryItemOrder Controller cancel record successfully...");

                return Ok(result);
            }
            catch (Exception ex)
            {
                string errMsg = $"Exception occured due to call CancelOrder in InventoryItemOrder Controller Put Request=>{ex.Message} \n {ex.StackTrace} \n {ex.InnerException}";
                _logger.LogError($"CancelOrder  Request in InventoryItemOrder Controller=>{errMsg}");
                return Problem(errMsg, statusCode: (int)HttpStatusCode.BadRequest);
            }
        }
        
        [HttpPut("cancelorderitem/{orderitemid}/{inventoryitemid}/{userid}/{apicallurl}")]
        public async Task<IActionResult> CancelOrderItem(int orderitemid,int inventoryitemid, int userid, string apicallurl)
        {
            try
            {
                apicallurl = WebUtility.UrlDecode(apicallurl);
                var result = await _inventoryItemOrderRepository.CancelOrderItem(orderitemid,inventoryitemid,userid,apicallurl);
                _logger.LogInfo($"CancelOrderItem  Request in InventoryItemOrder Controller cancel order item record successfully...");

                return Ok(result);
            }
            catch (Exception ex)
            {
                string errMsg = $"Exception occured due to call CancelOrderItem in InventoryItemOrder Controller Put Request=>{ex.Message} \n {ex.StackTrace} \n {ex.InnerException}";
                _logger.LogError($"CancelOrderItem  Request in InventoryItemOrder Controller=>{errMsg}");
                return Problem(errMsg, statusCode: (int)HttpStatusCode.BadRequest);
            }
        }

        [HttpPut("updateorderlocation/{userid}/{destbuildingid}/{destfloorid}/{destroom}/{apicallurl}")]
        public async Task<IActionResult> UpdateOrderLocation(int userid,int destbuildingid,int destfloorid,string destroom, string apicallurl,[FromBody] InventoryOrderItemModel inventoryorderitemmodel)
        {
            try
            {
                apicallurl = WebUtility.UrlDecode(apicallurl);
                //var inventoryOrderItem = jsonmodel;              
                //var inventoryOrderItem = JsonConvert.DeserializeObject<InventoryOrderItemModel>(jsonmodel);              

                bool result = await _inventoryItemOrderRepository.UpdateOrderLocation(userid, destbuildingid, destfloorid, destroom, apicallurl, inventoryorderitemmodel);
                _logger.LogInfo($"UpdateOrderLocation  Request in InventoryItemOrder Controller return result successfully...");

                return Ok(result);
            }
            catch (Exception ex)
            {
                string errMsg = $"Exception occured due to call UpdateOrderLocation Request=>{ex.Message} \n {ex.StackTrace}";
                _logger.LogError($"UpdateOrderLocation  Request in InventoryItemOrder Controller=>{errMsg}");
                return Problem(errMsg, statusCode: (int)HttpStatusCode.BadRequest);
            }
        }

        [HttpPut("updateorderitemlocation/{userid}/{apicallurl}")]
        public async Task<IActionResult> UpdateOrderItemLocation(int userid, string apicallurl, [FromBody] InventoryOrderItemModel inventoryorderitemmodel)
        {
            try
            {
                apicallurl = WebUtility.UrlDecode(apicallurl);
               // var inventoryOrderItem = JsonConvert.DeserializeObject<InventoryOrderItemModel>(jsonmodel);

                bool result = await _inventoryItemOrderRepository.UpdateOrderItemLocation(userid, apicallurl, inventoryorderitemmodel);
                _logger.LogInfo($"UpdateOrderItemLocation  Request in InventoryItemOrder Controller return result successfully...");

                return Ok(result);
            }
            catch (Exception ex)
            {
                string errMsg = $"Exception occured due to call UpdateOrderItemLocation Request=>{ex.Message} \n {ex.StackTrace}";
                _logger.LogError($"UpdateOrderItemLocation  Request in InventoryItemOrder Controller=>{errMsg}");
                return Problem(errMsg, statusCode: (int)HttpStatusCode.BadRequest);
            }
        }

        [HttpPut("orderassignto/{apicallurl}")]
        //public async Task<IActionResult> OrderAssignTo(string apicallurl, [FromBody] string json)
        public async Task<IActionResult> OrderAssignTo(string apicallurl, [FromBody] AssignToModel model)
        {
            try
            {
                apicallurl = WebUtility.UrlDecode(apicallurl);
                //var model = JsonConvert.DeserializeObject<AssignToModel>(WebUtility.UrlDecode(json));

                if (model == null)
                    return Problem($"invalid model->{model}", statusCode: (int)HttpStatusCode.BadRequest);
              
                var result = await _inventoryItemOrderRepository.OrderAssignTo(apicallurl,model);

                return Ok(result);
            }
            catch(Exception ex)
            {
                string errMsg = $"Exception occured due to call AssignToInstaller Request=>{ex.Message} \n {ex.StackTrace}";
                _logger.LogError($"AssignToInstaller  Request in InventoryItemOrder Controller=>{errMsg}");
                return Problem(errMsg, statusCode: (int)HttpStatusCode.BadRequest);
            }
        }

        [HttpPut("closeorder/{orderid}/{userid}/{apicallurl}")]
        public async Task<IActionResult> CloseOrder(int orderid, int userid, string apicallurl)
        {
            try
            {
                apicallurl = WebUtility.UrlDecode(apicallurl);
                //string actionnote = json;

                var result = await _inventoryItemOrderRepository.CloseOrder(orderid, userid, apicallurl);
                _logger.LogInfo($"UpdateOrder  Request in InventoryItemOrder Controller updated record successfully...");

                return Ok(result);
            }
            catch (Exception ex)
            {
                string errMsg = $"Exception occured due to call Put UpdateOrder in InventoryItemOrder Controller Put Request=>{ex.Message} \n {ex.StackTrace} \n {ex.InnerException}";
                _logger.LogError($"UpdateOrder  Request in InventoryItemOrder Controller=>{errMsg}");
                return Problem(errMsg, statusCode: (int)HttpStatusCode.BadRequest);
            }
        }

    }
}
