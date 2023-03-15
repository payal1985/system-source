using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SSInventory.Core.Models;
using SSInventory.Core.Models.External;
using SSInventory.Core.Repositories.Interfaces;
using SSInventory.Core.Services.External;
using SSInventory.Share.Models.Dto.Submission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSInventory.Core.Repositories
{
    public class SubmissionRepository : Repository<Submissions>, ISubmissionRepository
    {
        private readonly IMapper _mapper;
        private readonly IBuildingService _buildingService;

        public SubmissionRepository(SSInventoryDbContext dbContext, IMapper mapper,
            IBuildingService buildingService)
            : base(dbContext)
        {
            _mapper = mapper;
            _buildingService = buildingService;
        }

        public virtual async Task<List<SubmissionModel>> ReadAsync(List<int> ids = null, int? userId = null,
            string status = null, string inventoryAppId = null, int? clientId = null)
        {
            var data = GetAll();
            if (ids?.Any() == true)
            {
                data = data.Where(x => ids.Contains(x.SubmissionId));
            }
            if (userId.GetValueOrDefault(0) > 0)
            {
                data = data.Where(x => x.CreateId == userId);
            }
            if (!string.IsNullOrWhiteSpace(status))
            {
                data = data.Where(x => x.Status.Equals(status));
            }
            if (!string.IsNullOrWhiteSpace(inventoryAppId))
            {
                data = data.Where(x => x.InventoryAppId.Contains(inventoryAppId));
            }
            if (clientId.GetValueOrDefault(0) > 0)
            {
                data = data.Where(x => x.ClientId == clientId);
            }

            var entities = await data.ToListAsync();
            return _mapper.Map<List<SubmissionModel>>(entities);
        }

        public virtual async Task<int> InsertAsync(SubmissionModel model)
        {
            var submission = _mapper.Map<Submissions>(model);

            submission.CreateDateTime = DateTime.Now;
            var entity = await AddAsync(submission);

            return entity.SubmissionId;
        }

        public virtual async Task<bool> UpdateStatusAsync(List<int> ids, string status)
        {
            var entities = GetAll().Where(x => ids.Contains(x.SubmissionId));
            foreach (var entity in entities)
            {
                entity.Status = status;
                entity.UpdateDateTime = DateTime.Now;
            }

            await UpdateAsync(entities.ToList());

            return true;
        }

        public virtual async Task DeleteAsync(List<int> ids)
        {
            var data = GetAll();
            if (ids?.Any() == true)
            {
                data = data.Where(x => ids.Contains(x.SubmissionId));
            }
            await DeleteAsync(await data.ToListAsync());
        }

        public virtual async Task<List<ExportSubmissionModel>> ExportSubmissions(List<ExportSubmissionModel> input)
        {
            var submissionIds = input.Select(x => x.SubmissionId).Distinct().ToList();
            var clientId = input.FirstOrDefault().ClientId;
            var submissions = GetAll();
            if (submissionIds?.Any() == true)
            {
                submissions = submissions.Where(x => submissionIds.Contains(x.SubmissionId));
            }
            if (clientId > 0)
            {
                submissions = submissions.Where(x => x.ClientId == clientId);
            }

            // join data
            var query = await (from sub in submissions
                               join inv in _dbContext.Inventory on sub.SubmissionId equals inv.SubmissionId
                               join invItem in _dbContext.InventoryItem on inv.InventoryId equals invItem.InventoryId
                               join invItemImg in _dbContext.InventoryImages on invItem.InventoryItemId equals invItemImg.InventoryItemId
                               select new
                               {
                                   sub,
                                   inv,
                                   invItem,
                                   invItemImg
                               }).AsNoTracking().ToListAsync();
            foreach (var submission in input)
            {
                var inventories = await _dbContext.Inventory.Where(x => x.SubmissionId == submission.SubmissionId).ToListAsync();
                var inventoryItems = await _dbContext.InventoryItem.Where(x => x.SubmissionId == submission.SubmissionId).ToListAsync();
                var inventoryItemIds = inventoryItems.Select(x => x.InventoryItemId).Distinct().ToList();
                var inventoryItemImages = await _dbContext.InventoryImages.Where(x => x.InventoryItemId.HasValue
                                                                                   && inventoryItemIds.Contains(x.InventoryItemId.Value))
                                                                          .ToListAsync();

                var itemTypes = await _dbContext.ItemTypes.Where(x => inventories.Select(y => y.ItemTypeId).Distinct().Contains(x.ItemTypeId)).ToListAsync();
                var manufacturers = await _dbContext.Manufacturers.Where(x => inventories.Select(y => y.ManufacturerId).Distinct().Contains(x.ManufacturerId)).ToListAsync();
                var statuses = await _dbContext.Status.Where(x => inventoryItems.Select(y => y.StatusId).Distinct().Contains(x.StatusId)).ToListAsync();
                var inventoryBuildings = await _buildingService.GetBuildings(clientId);


                var inventoryFloors = await _dbContext.InventoryFloors.Where(x => inventoryItems.Select(y => y.InventoryFloorId).Distinct().Contains(x.InventoryFloorId)).ToListAsync();

                foreach (var inventory in inventories)
                {
                    var inventoryOutput = _mapper.Map<ExportInventoryModel>(inventory);
                    inventoryOutput.ItemTypeText = itemTypes.Find(x => x.ItemTypeId == inventory.ItemTypeId)?.ItemTypeName;
                    inventoryOutput.ManufacturerName = manufacturers.Find(x => x.ManufacturerId == inventory.ManufacturerId)?.ManufacturerName;
                    foreach (var invItem in inventoryItems.Where(x => x.InventoryId == inventory.InventoryId))
                    {
                        var itemImages = inventoryItemImages.Where(x => x.InventoryItemId == invItem.InventoryItemId);
                        var inventoryItemOutput = _mapper.Map<ExportInventoryItemModel>(invItem);
                        inventoryItemOutput.StatusText = statuses.Find(x => x.StatusId == invItem.StatusId)?.StatusName;
                        inventoryItemOutput.InventoryBuildingName = inventoryBuildings.Find(x => x.InventoryBuildingId == invItem.InventoryBuildingId)?.InventoryBuildingName;
                        inventoryItemOutput.InventoryFloorName = inventoryFloors.Find(x => x.InventoryFloorId == invItem.InventoryFloorId)?.InventoryFloorName;

                        inventoryItemOutput.InventoryImages = inventoryItemOutput.InventoryImages == null
                            ? new List<ExportInventoryItemImageModel>() : inventoryItemOutput.InventoryImages;
                        var itemImagesMapper = _mapper.Map<List<ExportInventoryItemImageModel>>(itemImages);
                        inventoryItemOutput.InventoryImages.AddRange(itemImagesMapper.Where(x => !inventoryItemOutput.InventoryImages.Any(y => y.InventoryImageId == x.InventoryImageId)));
                        inventoryOutput.InventoryItems.Add(inventoryItemOutput);
                    }
                    submission.Inventories.Add(inventoryOutput);
                }
            }

            return input;
        }
    }
}
