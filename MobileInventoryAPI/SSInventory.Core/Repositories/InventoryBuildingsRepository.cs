using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SSInventory.Core.Models;
using SSInventory.Core.Repositories.Interfaces;
using SSInventory.Share.Models.Dto.InventoryBuildings;
using SSInventory.Share.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSInventory.Core.Repositories
{
    public class InventoryBuildingsRepository : Repository<InventoryBuildings>, IInventoryBuildingsRepository
    {
        private readonly IMapper _mapper;
        public InventoryBuildingsRepository(SSInventoryDbContext dbContext, IMapper mapper)
            : base(dbContext)
        {
            _mapper = mapper;
        }

        public virtual async Task<List<InventoryBuildingModel>> ReadAsync(int? clientId = null, List<int> ids = null)
        {
            var data = GetAll().WhereIf(ids?.Any() == true, x => ids.Contains(x.InventoryBuildingId));
            if((clientId ?? 1) > 0)
            {
                data = data.Where(x => x.ClientId == clientId);
            }

            var entities = await data.ToListAsync();
            return _mapper.Map<List<InventoryBuildingModel>>(entities);
        }

        //public virtual async Task<InventoryBuildingModel> InsertAsync(CreateOrUpdateInventoryBuildingModel model)
        //{
        //    var existingEntity = await GetAll().Where(x => EF.Functions.Like(x.InventoryBuildingName, $"{model.InventoryBuildingName}")).FirstOrDefaultAsync();
        //    if (existingEntity != null)
        //        return _mapper.Map<InventoryBuildingModel>(existingEntity);


        //    var orderNumber = GetAll().Select(x => (int?)x.OrderSequence).DefaultIfEmpty()?.Max() ?? 0;

        //    var entity = _mapper.Map<InventoryBuildings>(model);
        //    entity.InventoryBuildingCode = model.InventoryBuildingName.Replace(" ", "");
        //    entity.OrderSequence = orderNumber + 10;
        //    entity.CreateDateTime = System.DateTime.Now;
        //    entity.StatusId = (int)BasiStatusEnums.Active;
        //    entity.CreateId = 0;

        //    await AddAsync(_mapper.Map<InventoryBuildings>(entity));

        //    return _mapper.Map<InventoryBuildingModel>(entity);
        //}
    }
}
