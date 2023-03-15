using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SSInventory.Core.Models;
using SSInventory.Core.Repositories.Interfaces;
using SSInventory.Share.Models;
using SSInventory.Share.Models.Dto.ItemTypeOptionLines;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSInventory.Core.Repositories
{
    public class ManufactoryRepository : Repository<ItemTypeOptionLines>, IManufactoryRepository
    {
        private readonly IMapper _mapper;

        public ManufactoryRepository(SSInventoryDbContext dbContext, IMapper mapper)
            : base(dbContext)
        {
            _mapper = mapper;
        }

        public async Task<List<ItemTypeOptionLineModel>> InsertAsync(List<CreateOrUpdateItemTypeOptionLineModel> model)
        {
            var entities = new List<ItemTypeOptionLines>();
            foreach (var item in model)
            {
                var orderNumber = GetAll().Where(x => x.ClientId == item.ClientId && x.ItemTypeOptionId == item.ItemTypeOptionId)
                                          .Select(x => (int?)x.OrderSequence).DefaultIfEmpty()?.Max() ?? 0;
                item.OrderSequence = orderNumber + 10;
                entities.Add(_mapper.Map<ItemTypeOptionLines>(item));
            }
            if (entities.Count > 0)
            {
               await AddAsync(entities);
            }

            var data = new List<ItemTypeOptionLines>();
            foreach (var item in model)
            {
                data.Add(await GetAll().AsNoTracking().OrderByDescending(x => x.OrderSequence)
                    .FirstOrDefaultAsync(x => x.ClientId == item.ClientId && x.ItemTypeOptionId == item.ItemTypeOptionId));
            }
            var result = new List<ItemTypeOptionLineModel>();
            foreach (var item in data)
            {
                result.Add(_mapper.Map<ItemTypeOptionLineModel>(item));
            }

            return result;
        }

    }
}
