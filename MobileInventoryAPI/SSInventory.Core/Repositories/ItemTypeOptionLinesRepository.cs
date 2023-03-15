using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SSInventory.Core.Models;
using SSInventory.Core.Repositories.Interfaces;
using SSInventory.Share.Models.Dto.ItemTypeOptionLines;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSInventory.Core.Repositories
{
    public class ItemTypeOptionLinesRepository : Repository<ItemTypeOptionLines>, IItemTypeOptionLinesRepository
    {
        private readonly IMapper _mapper;
        public ItemTypeOptionLinesRepository(SSInventoryDbContext dbContext, IMapper mapper)
            : base(dbContext)
        {
            _mapper = mapper;
        }

        public virtual async Task<List<ItemTypeOptionLinesModel>> ReadAsync(List<int> itemTypeOptionIds = null)
        {
            var query = GetAll();
            if(itemTypeOptionIds?.Any() == true)
            {
                query = query.Where(x => itemTypeOptionIds.Contains(x.ItemTypeOptionId));
            }
            var entities = await query.ToListAsync();
            var result = new List<ItemTypeOptionLinesModel>();
            foreach (var item in entities)
            {
                result.Add(_mapper.Map<ItemTypeOptionLinesModel>(item));
            }
            return result;
        }

        public virtual async Task InsertAsync(List<CreateOrUpdateItemTypeOptionLineModel> model)
        {
            var entities = new List<ItemTypeOptionLines>();
            foreach (var item in model)
            {
                entities.Add(_mapper.Map<ItemTypeOptionLines>(item));
            }

            if(entities.Count > 0)
            {
                await AddAsync(entities);
            }
        }
    }
}
