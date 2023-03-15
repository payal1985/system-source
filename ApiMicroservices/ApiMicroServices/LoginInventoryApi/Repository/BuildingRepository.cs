using LoginInventoryApi.DBContext;
using LoginInventoryApi.DBModels;
using LoginInventoryApi.Models;
using LoginInventoryApi.Models.Constants;
using LoginInventoryApi.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoginInventoryApi.Repository
{
    public class BuildingRepository : IBuildingRepository
    {
        LoginContext _dbContext;

        public BuildingRepository(LoginContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<ClientLocationsModel>> GetBuildings(int clientid)
        {
            try
            {
                var buildings = await _dbContext.ClientLocations.Where(cl => cl.client_id == clientid && !string.IsNullOrWhiteSpace(cl.location_name)).ToListAsync();

                return buildings.Select(x => new ClientLocationsModel()
                {
                    ClientId = x.client_id,
                    InventoryBuildingId = x.location_id,
                    InventoryBuildingName = x.location_name
                }).OrderBy(ord => ord.InventoryBuildingName).ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<ClientLocationsModel>> GetDestinationBuilding(int clientid)
        {
            try
            {
                var buildings = await _dbContext.ClientLocations.Where(cl => cl.client_id == clientid && !string.IsNullOrWhiteSpace(cl.location_name)).ToListAsync();

                var locations = buildings.Select(x => new ClientLocationsModel()
                                {
                                    ClientId = clientid,
                                    InventoryBuildingId = x.location_id,
                                    InventoryBuildingName = x.location_name
                                }).OrderBy(ord => ord.InventoryBuildingName).ToList();

                locations.Add(new ClientLocationsModel() { ClientId = clientid, InventoryBuildingId = -4, InventoryBuildingName = "Dispose / Landfill" });
                locations.Add(new ClientLocationsModel() { ClientId = clientid, InventoryBuildingId = -3, InventoryBuildingName = "Dispose / Donation" });
                locations.Add(new ClientLocationsModel() { ClientId = clientid, InventoryBuildingId = -2, InventoryBuildingName = "Dispose / Recycle" });
                locations.Add(new ClientLocationsModel() { ClientId = clientid, InventoryBuildingId = -1, InventoryBuildingName = "Dispose / Internal Sale" });

                return locations;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<ClientLocationsModel>> GetBuildings(BuildingModel model)
        {
            try
            {
                var buildings = new List<ClientLocationsModel>();
                if (model.BuildingIds?.Any() == false && (model.ClientId <= 0 || model.ClientId == null))
                    return buildings;

                var buildingsQuery = _dbContext.ClientLocations.Where(cl =>
                cl.location_type == BuildingConstant.LocationType &&
                !string.IsNullOrWhiteSpace(cl.location_name));
                if (model.BuildingIds?.Any() == true)
                    buildingsQuery = buildingsQuery.Where(cl => model.BuildingIds.Contains(cl.location_id));
                if (model.ClientId > 0)
                    buildingsQuery = buildingsQuery.Where(cl => cl.client_id == model.ClientId);

                buildings = await buildingsQuery.Select(x => new ClientLocationsModel()
                {
                    ClientId = x.client_id,
                    InventoryBuildingId = x.location_id,
                    InventoryBuildingName = x.location_name
                })
                .OrderBy(ord => ord.InventoryBuildingName)
                .ToListAsync();
                return buildings;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> IsExistingBuilding(int clientId, string buildingName)
        {
            return await _dbContext.ClientLocations.AnyAsync(cl =>
                cl.location_type == BuildingConstant.LocationType &&
                cl.client_id == clientId
                && cl.location_name == buildingName);
        }

        public async Task<ClientLocationsModel> InsertBuilding(CreateInventoryBuildingModel buildingModel)
        {
            try
            {
                var buildingEntity = new ClientLocations
                {
                    client_id = buildingModel.ClientId,
                    location_name = buildingModel.InventoryBuildingName,
                    location_type = BuildingConstant.LocationType,
                    createid = buildingModel.UserId,
                    updateid = buildingModel.UserId,
                    createprocess = buildingModel.UserId,
                    updateprocess = buildingModel.UserId,
                    createdate = DateTime.UtcNow,
                    updatedate = DateTime.UtcNow
                };

                await _dbContext.ClientLocations.AddAsync(buildingEntity);
                await _dbContext.SaveChangesAsync();

                return  new ClientLocationsModel
                    {
                        ClientId = buildingEntity.client_id,
                        InventoryBuildingId = buildingEntity.location_id,
                        InventoryBuildingName = buildingEntity.location_name
                    };
            }
            catch(Exception)
            {
                throw;
            }
        }    
    }
}