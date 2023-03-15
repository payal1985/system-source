using InventoryApi.Repository;
using InventoryApi.Repository.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryApi.Mappers
{
    public static class SSIDBRepositoryMapper
    {
        public static void Register(IServiceCollection services)
        {
            services.AddTransient<IHDTicketRepository, HDTicketRepository>();
        }
    }
}
