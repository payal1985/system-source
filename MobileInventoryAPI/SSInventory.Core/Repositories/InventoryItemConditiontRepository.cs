using Microsoft.EntityFrameworkCore;
using SSInventory.Core.Models;
using SSInventory.Core.Repositories.Interfaces;
using SSInventory.Share.Models.Dto.InventoryItemConditions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSInventory.Core.Repositories
{
    public class InventoryItemConditionRepository : Repository<InventoryItemCondition>, IInventoryItemConditionRepository
    {
        public InventoryItemConditionRepository(SSInventoryDbContext dbContext)
            : base(dbContext)
        {

        }

        public async Task<List<InventoryItemConditionModel>> ReadAsync(List<int> ids = null)
        {
            var entities = GetAll().WhereIf(ids?.Any() == true, x => ids.Contains(x.InventoryItemConditionId))
                                    .Where(x => x.ConditionName != "New" && x.IsMobileCondition)
                                   .AsNoTracking();

            return await entities.Select(x => new InventoryItemConditionModel
            {
                InventoryItemConditionId = x.InventoryItemConditionId,
                ConditionCode = x.ConditionName.Replace(" ", ""),
                ConditionName = x.ConditionName,
                ConditionDescription = x.ConditionName
            }).ToListAsync();
        }
    }
}
