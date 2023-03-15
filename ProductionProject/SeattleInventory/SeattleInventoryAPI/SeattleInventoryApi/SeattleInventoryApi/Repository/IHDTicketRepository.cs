using SeattleInventoryApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeattleInventoryApi.Repository
{
    public interface IHDTicketRepository
    {
        bool CreateHDTicket(InventoryOrderModel model, string mbody);
        bool CreateWarrentyRequestHDTicket(InventoryOrderModel model, string mbody);
    }
}
