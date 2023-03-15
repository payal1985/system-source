using LoginInventoryApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoginInventoryApi.Repository.Interfaces
{
    public interface IBuildingRepository
    {
        Task<List<ClientLocationsModel>> GetBuildings(int clientid);
        Task<List<ClientLocationsModel>> GetDestinationBuilding(int clientid);

        Task<List<ClientLocationsModel>> GetBuildings(BuildingModel model);

        Task<ClientLocationsModel> InsertBuilding(CreateInventoryBuildingModel buildingModel);

        Task<bool> IsExistingBuilding(int clientId, string buildingName);
    }
}