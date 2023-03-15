using InvHDRequestApi.Models;
using InvHDRequestApi.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace InvHDRequestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        //private readonly IEmailNotificationRepository _emailNotificationRepository;
        //private readonly IHDTicketRepository _hDTicketRepository;
        private readonly ILoggerManagerRepository _logger;


        public OrderController(IOrderRepository orderRepository, ILoggerManagerRepository logger
                                            //,IEmailNotificationRepository emailNotificationRepository
                                            //,IHDTicketRepository hDTicketRepository
                                            )
        {
            //logger = LogManager.GetCurrentClassLogger();
            _logger = logger;
            _orderRepository = orderRepository;
            //_emailNotificationRepository = emailNotificationRepository;
            //_hDTicketRepository = hDTicketRepository;

            _logger.LogInfo("Initialize the Inventory Item Order Controller");

        }

        [HttpGet("testorder")]
        public async Task<IActionResult> Test()
        {
           var envConnectionString = _orderRepository.GetConnectionString();
           _logger.LogInfo($"Connection String-{envConnectionString}");
           return Ok("Success !!!!!!!");
        }

        [HttpPost("submitorder/{apicallurl}")]
        public async Task<IActionResult> SubmitOrderRequest(string apicallurl, [FromBody] List<OrderSubmissionModel> model)
        {
            bool result = false;
            apicallurl = WebUtility.UrlDecode(apicallurl);

            try
            {
                if(model == null || model.Count <= 0)
                {
                    return Problem("order submission model is invalid", statusCode: (int)HttpStatusCode.BadRequest);
                }

                var grpResult = model.GroupBy
                   (x => new
                   {
                       x.Email,
                       x.RequestForName,
                       x.ClientNote
                   })
                   .Select(key =>
                       new OrderModel
                       {
                           RequestorEmail = key.Key.Email,
                           RequestorForName = key.Key.RequestForName,
                           ClientNote = key.Key.ClientNote,
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
                               //InventoryImageUrl = k.InventoryImageUrl,
                               ItemCode = k.ItemCode,
                               Description = k.Description,
                               PullQty = k.PullQty,
                               ClientID = k.ClientID,
                               ClientName = k.ClientName,
                               ClientPath = k.ClientPath,
                               RequestorId = k.RequestorId,
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



                foreach (var item in grpResult)
                {
                    result = await _orderRepository.CreateOrderRequest(apicallurl,item);
                    if (!result)
                        break;
                }
                //result = true;
                _logger.LogInfo($"SubmitOrderRequest  Request in Order Controller return result successfully...");


                return Ok(result);
            }
            catch (Exception ex)
            {
                string errMsg = $"Exception occured due to call SubmitOrderRequest in Order Controller Post Request=>{ex.Message} \n {ex.StackTrace} \n {ex.InnerException}";
                _logger.LogError($"SubmitOrderRequest  Request in Order Controller=>{errMsg}");
                // var strErrMsg = $"Exception while send order {ex.Message} \n {ex.InnerException} \n {ex.StackTrace}";
                //return BadRequest(errMsg);
                return Problem(errMsg, statusCode: (int)HttpStatusCode.BadRequest);

            }

        }
    }
}
