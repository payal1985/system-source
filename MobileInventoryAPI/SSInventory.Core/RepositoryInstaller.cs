using Microsoft.Extensions.DependencyInjection;
using SSInventory.Core.Repositories.Interfaces;
using SSInventory.Core.Repositories;
using SSInventory.Core.Services.External;
using Microsoft.AspNetCore.Http;

namespace SSInventory.Core
{
    public static class RepositoryInstaller
    {
        public static void Register(IServiceCollection services)
        {
            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
            services.AddTransient<ISubmissionRepository, SubmissionRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IManufactoryRepository, ManufactoryRepository>();
            services.AddTransient<IItemTypeRepository, ItemTypeRepository>();
            services.AddTransient<IItemTypeOptionSetRepository, ItemTypeOptionSetRepository>();
            services.AddTransient<IItemTypeOptionRepository, ItemTypeOptionRepository>();
            services.AddTransient<IItemTypeOptionLinesRepository, ItemTypeOptionLinesRepository>();
            services.AddTransient<IInventoryRepository, InventoryRepository>();
            services.AddTransient<IInventoryItemRepository, InventoryItemRepository>();
            services.AddTransient<IInventoryImageRepository, InventoryImageRepository>();
            services.AddTransient<IInventoryBuildingsRepository, InventoryBuildingsRepository>();
            services.AddTransient<IInventoryFloorsRepository, InventoryFloorsRepository>();
            services.AddTransient<IManufacturerRepository, ManufacturerRepository>();
            services.AddTransient<IStatusRepository, StatusRepository>();
            services.AddTransient<IClientRepository, ClientRepository>();
            services.AddTransient<IInventoryItemConditionRepository, InventoryItemConditionRepository>();
            services.AddTransient<IOrderRepository, OrderRepository>();
            services.AddTransient<IOrderItemRepository, OrderItemRepository>();
            services.AddTransient<IBuildingService, BuildingService>();
            services.AddTransient<IHistoryTableRepository, HistoryTableRepository>();
            services.AddTransient<ICountryService, CountryService>();
            services.AddTransient<IStateProvinceService, StateProvinceService>();
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
        }
    }
}
