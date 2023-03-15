using SSInventory.Share.Models.Dto.InventoryBuildings;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SSInventory.Business.Interfaces
{
    public interface IInventoryBuildingsService
    {
        Task<List<InventoryBuildingModel>> ReadAsync(int clientId);
    }
}