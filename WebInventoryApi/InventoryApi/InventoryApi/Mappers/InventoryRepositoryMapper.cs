using InventoryApi.Repository;
using InventoryApi.Repository.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryApi.Mappers
{
    public static class InventoryRepositoryMapper
    {
        public static void Register(IServiceCollection services)
        {
            services.AddTransient<IInventoryRepository, InventoryRepository>();
            services.AddTransient<IInventoryItemRepository, InventoryItemRepository>();
            services.AddTransient<IInventoryItemImageRepository, InventoryItemImageRepository>();
            services.AddTransient<IInventoryItemOrderRepository, InventoryItemOrderRepository>();
            services.AddTransient<IDonationInventoryRepository, DonationInventoryRepository>();
            services.AddTransient<IInventoryItemWarrantyRepository, InventoryItemWarrantyRepository>();
            services.AddTransient<IAdminInventoryRepository, AdminInventoryRepository>();
            services.AddTransient<IInventoryManufacturerRepository, InventoryManufacturerRepository>();
            services.AddTransient<IInventoryCompanyRepository, InventoryCompanyRepository>();
            
        }
    }
}
