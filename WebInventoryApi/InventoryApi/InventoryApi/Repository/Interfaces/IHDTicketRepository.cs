using InventoryApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryApi.Repository.Interfaces
{
    public interface IHDTicketRepository
    {
        Task<int> CreateHDTicket(InventoryOrderModel model, string mbody);
        //Task<int> CreateWarrentyRequestHDTicket(InventoryItemWarrantyModel model, string mbody);
        Task<int> CreateWarrentyRequestHDTicket(List<InventoryItemWarrantyModel> model);
    }
}
