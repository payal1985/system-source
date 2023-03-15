using SSInventory.Business.Interfaces;
using SSInventory.Core.Models.External;
using SSInventory.Core.Repositories.Interfaces;
using SSInventory.Core.Services.External;
using SSInventory.Share.Models.Dto.InventoryBuildings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSInventory.Business.Services
{
    public class InventoryBuildingsService : IInventoryBuildingsService
    {
        private readonly IInventoryBuildingsRepository _inventoryBuildingsRepository;
        private readonly IBuildingService _buildingService;
        private readonly ICurrentUser _currentUser;
        public InventoryBuildingsService(IInventoryBuildingsRepository inventoryBuildingsRepository,
            IBuildingService buildingService,
            ICurrentUser currentUser)
        {
            _inventoryBuildingsRepository = inventoryBuildingsRepository;
            _buildingService = buildingService;
            _currentUser = currentUser;
        }

        public virtual async Task<List<InventoryBuildingModel>> ReadAsync(int clientId)
        {
            var buildings = await _buildingService.GetBuildings(clientId);

            return buildings.Select(x => new InventoryBuildingModel
            {
                ClientId= clientId,
                InventoryBuildingId = x.InventoryBuildingId,
                InventoryBuildingName = x.InventoryBuildingName,
                InventoryBuildingCode = x.InventoryBuildingName,
                InventoryBuildingDesc = x.InventoryBuildingName
            }).ToList();
        }
    }
}
