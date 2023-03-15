using InventoryApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryApi.Repository.Interfaces
{
    public interface IInventoryItemOrderRepository
    {
        Task<bool> InsertInventoryOrder(InventoryOrderModel model);

        Task<List<InventoryOrderItemModel>> GetInventoryOrderItems(int clientid,int ordertypeid, int statusid);
        Task<List<InventoryOrderItemModel>> ExpandInventoryOrderItems(int orderid,int destbuildingid, int destfloorid, string destroom);

        //Task<bool> CompleteOrder(int orderid, int destbuildingid, int destfloorid, string destroom, int userid, string apicallurl,string actionnote);
        Task<bool> CompleteOrder(int orderid, int userid, string apicallurl,string actionnote);
        Task<bool> CompleteOrderItem(int orderitemid,int userid, string apicallurl,string actionnote);
        //int InsertInventoryOrderItem();

        Task<bool> RemoveCartItem(int inventoryitemid);
        //int InsertInventoryItemRivisonLog();
        //int UpdateInventoryItem();

        Task<bool> CancelOrderItem(int orderitemid, int inventoryitemid, int userid, string apicallurl);
        Task<bool> CancelOrder(int orderid, int destbuildingid, int destfloorid, string destroom, int userid, string apicallurl);

        Task<bool> UpdateOrderLocation(int userid, int destbuildingid, int destfloorid, string destroom, string apicallurl, InventoryOrderItemModel model);
        Task<bool> UpdateOrderItemLocation(int userid, string apicallurl, InventoryOrderItemModel model);
        Task<bool> OrderAssignTo(string apicallurl, AssignToModel model);

        Task<List<OrderTypeModel>> GetOrderTypes();
        Task<List<StatusModel>> GetStatus();
        Task<bool> CloseOrder(int orderid, int userid, string apicallurl);

    }
}
