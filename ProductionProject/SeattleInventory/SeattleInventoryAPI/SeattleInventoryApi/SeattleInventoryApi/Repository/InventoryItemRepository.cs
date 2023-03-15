using Microsoft.EntityFrameworkCore.Storage;
using SeattleInventoryApi.DBContext;
using SeattleInventoryApi.DBModels;
using SeattleInventoryApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeattleInventoryApi.Repository
{
    public class InventoryItemRepository : IInventoryItemRepository
    {
        InventoryContext _dbContext;
        public InventoryItemRepository(InventoryContext dbContext)
        {
            _dbContext = dbContext;
        }
        public List<string> GetBuilding(int client_id)
        {
            try
            {
                return _dbContext.InventoryItems.Where(x => x.building != "" && x.client_id == client_id && x.building.ToLower() != "donate").Select(bul => bul.building).Distinct().ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public List<string> GetFloor(int client_id)
        {
            try
            {
                //var items =  _dbContext.InventoryItems.Where(x => x.floor != "").Select(f => f.floor).Distinct().ToList();
                return _dbContext.InventoryItems.Where(x => x.floor != "" && x.client_id == client_id).Select(f => f.floor).Distinct().ToList();

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<string> GetRoom(int client_id)
        {
            try
            {
                return _dbContext.InventoryItems.Where(x => x.mploc != "" && x.client_id == client_id).Select(f => f.mploc).Distinct().ToList();

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public List<string> GetCondition()
        {
            try
            {
                List<string> listCond = new List<string>();
                listCond.Add("Good");
                listCond.Add("Fair");
                listCond.Add("Poor");
                listCond.Add("Damage");
                listCond.Add("Missing Parts");
                return listCond;

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<string> GetDepCostCenter()
        {
            try
            {
                List<string> listDepCost = new List<string>();
                listDepCost.Add("HR");
                listDepCost.Add("Management");
                listDepCost.Add("IT");
                listDepCost.Add("Network");
                listDepCost.Add("Marketing");
                return listDepCost;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> InsertBuilding(InventoryBuildingsModel inventoryBuildingsModel)
        {
            bool result = false;
            using (IDbContextTransaction transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    InventoryBuildings buildingModel = new InventoryBuildings();
                    buildingModel.ClientID = inventoryBuildingsModel.client_id;
                    buildingModel.InventoryBuildingCode = inventoryBuildingsModel.Building;
                    buildingModel.InventoryBuildingName = inventoryBuildingsModel.Building;
                    buildingModel.InventoryBuildingDesc = inventoryBuildingsModel.Building;
                    buildingModel.StatusID = 20;
                    buildingModel.OrderBy = 0;
                    buildingModel.CreatedBy = 1;
                    buildingModel.CreatedDate = System.DateTime.Now;
                    buildingModel.LastModBy = 1;
                    buildingModel.LastModDate = System.DateTime.Now;


                    await _dbContext.AddAsync(buildingModel);
                    await _dbContext.SaveChangesAsync();

                    await transaction.CommitAsync();

                    result = true;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }

            return result;
        }

        public async Task<bool> InsertFloor(InventoryFloorsModel inventoryFloorsModel)
        {
            bool result = false;
            using (IDbContextTransaction transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    InventoryFloors floorModel = new InventoryFloors();
                    floorModel.ClientID = inventoryFloorsModel.client_id;
                    floorModel.InventoryFloorCode = inventoryFloorsModel.Floor;
                    floorModel.InventoryFloorName = inventoryFloorsModel.Floor;
                    floorModel.InventoryFloorDesc = inventoryFloorsModel.Floor;
                    floorModel.StatusID = 20;
                    floorModel.OrderBy = 0;
                    floorModel.CreatedBy = 1;
                    floorModel.CreatedDate = System.DateTime.Now;
                    floorModel.LastModBy = 1;
                    floorModel.LastModDate = System.DateTime.Now;


                    await _dbContext.AddAsync(floorModel);
                    await _dbContext.SaveChangesAsync();

                    await transaction.CommitAsync();

                    result = true;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }

            return result;
        }

    }
}
