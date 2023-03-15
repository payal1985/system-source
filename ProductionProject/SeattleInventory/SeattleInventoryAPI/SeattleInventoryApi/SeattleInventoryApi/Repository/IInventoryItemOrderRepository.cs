using SeattleInventoryApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeattleInventoryApi.Repository
{
    public interface IInventoryItemOrderRepository
    {
        bool InsertInventoryOrder(InventoryOrderModel model);

        Task<List<InventoryOrderItemModel>> GetInventoryOrderItems();

        bool InsertWarrantyRequest(InventoryOrderModel model);

        //int InsertInventoryOrderItem();

        //int InsertInventoryItemRivisonLog();
        //int UpdateInventoryItem();
    }
}
