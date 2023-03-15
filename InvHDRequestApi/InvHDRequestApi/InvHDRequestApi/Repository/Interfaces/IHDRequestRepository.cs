using InvHDRequestApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvHDRequestApi.Repository.Interfaces
{
    public interface IHDRequestRepository
    {
        Task<int> CreateOrderReqHDTicket(OrderModel model, string mbody);

        //Task<int> CreateWarrentyRequestHDTicket(List<WarrantyModel> model);
        //Task<int> CreateMaintenanceRequestHDTicket(List<MaintenanceModel> model);
        //Task<int> CreateCleaningRequestHDTicket(List<CleaningModel> model);
        
        Task<int> CreateRequestHDTicket(List<GenericModel> model, string requesttype);
    }
}
