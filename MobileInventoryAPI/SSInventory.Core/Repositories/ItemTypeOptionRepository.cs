using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SSInventory.Core.Models;
using SSInventory.Core.Repositories.Interfaces;
using SSInventory.Share.Models;
using SSInventory.Share.Models.Dto.ItemTypeOption;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSInventory.Core.Repositories
{
    public class ItemTypeOptionRepository : Repository<ItemTypeOptions>, IItemTypeOptionRepository
    {
        private readonly IMapper _mapper;
        public ItemTypeOptionRepository(SSInventoryDbContext dbContext, IMapper mapper)
            : base(dbContext)
        {
            _mapper = mapper;
        }

        public async Task<List<ItemTypeOptionModel>> ReadAsync(int? clientId = null, List<int> ids = null, List<int> itemTypeIds = null, List<string> itemTypeOptionCodes = null)
        {
            var query = GetAll();
            if(ids?.Any() == true)
            {
                query = query.Where(x => ids.Contains(x.ItemTypeOptionId));
            }
            if (clientId.HasValue)
            {
                query = query.Where(x => x.ClientId == clientId);
            }
            if (itemTypeIds?.Any() == true)
            {
                query = query.Where(x => itemTypeIds.Contains(x.ItemTypeId));
            }
            if(itemTypeOptionCodes?.Any() == true)
            {
                query = query.Where(x => itemTypeOptionCodes.Contains(x.ItemTypeOptionCode));
            }
            var entities = await query.ToListAsync();
            var result = new List<ItemTypeOptionModel>();
            foreach (var item in entities)
            {
                result.Add(_mapper.Map<ItemTypeOptionModel>(item));
            }
            return result;
        }

        public async Task CreateAsync(List<CreateOrUpdateItemTypeOptionModel> itemTypeOptions)
        {
            var entities = new List<ItemTypeOptions>();
            foreach (var item in itemTypeOptions)
            {
                item.CreateDateTime = System.DateTime.Now;
                entities.Add(_mapper.Map<ItemTypeOptions>(item));
            }

            if (entities.Count > 0)
            {
                await AddAsync(entities);
            }
        }
    }
}
