using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SSInventory.Business.Interfaces;
using SSInventory.Business.Services;

namespace SSInventory.Business
{
    public static class ServiceInstaller
    {
        public static void Register(IServiceCollection services)
        {
            services.AddTransient<IItemTypeService, ItemTypeService>();
            services.AddTransient<IItemTypeOptionSetService, ItemTypeOptionSetService>();
            services.AddTransient<IItemTypeOptionService, ItemTypeOptionService>();
            services.AddTransient<IDapperService, DapperService>();
            services.AddTransient<ISubmissionService, SubmissionService>();
            services.AddTransient<IInventoryService, InventoryService>();
            services.AddTransient<IInventoryItemService, InventoryItemService>();
            services.AddTransient<IInventoryImageService, InventoryImageService>();
            services.AddTransient<IManufactoryService, ManufactoryService>();
            services.AddTransient<IItemTypeOptionLinesService, ItemTypeOptionLinesService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IInventoryBuildingsService, InventoryBuildingsService>();
            services.AddTransient<IInventoryFloorsService, InventoryFloorsService>();
            services.AddTransient<IManufacturerService, ManufacturerService>();
            services.AddTransient<IStatusService, StatusService>();
            services.AddTransient<IClientService, ClientService>();
            services.AddTransient<IInventoryItemConditionService, InventoryItemConditionService>();
            services.AddTransient<IInventoryImageService, InventoryImageService>();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<ICurrentUser, CurrentUser>();
            services.AddTransient<IOrderService, OrderService>();
            services.AddTransient<IOrderItemService, OrderItemService>();
            services.AddTransient<IHistoryTableService, HistoryTableService>();
        }
    }
}
