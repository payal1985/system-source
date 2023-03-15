using InventoryApi.Repository;
using InventoryApi.Repository.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryApi.Mappers
{
    public static class SharedRepositoryMapper
    {
        public static void Register(IServiceCollection services)
        {
            services.AddSingleton<ILoggerManagerRepository, LoggerManagerRepository>();
            services.AddTransient<IAws3Repository, Aws3Repository>();
            services.AddTransient<IEmailNotificationRepository, EmailNotificationRepository>();
           
        }
    }
}
