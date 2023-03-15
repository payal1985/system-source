using InvHDRequestApi.Repository;
using InvHDRequestApi.Repository.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvHDRequestApi.Mappers
{
    public class InventoryRepositoryMapper
    {
        public static void Register(IServiceCollection services)
        {
            services.AddTransient<IOrderRepository, OrderRepository>();
            services.AddTransient<IWarrantyRepository, WarrantyRepository>();
            services.AddTransient<IMaintenanceRepository, MaintenanceRepository>();
            services.AddTransient<ICleaningRepository, CleaningRepository>();

        }
    }
}
