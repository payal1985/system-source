using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SSInventory.Core.Models;
using SSInventory.Core.Repositories.Interfaces;
using SSInventory.Share.Models.Dto.InventoryFloors;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSInventory.Core.Repositories
{
    public class InventoryFloorsRepository : Repository<InventoryFloors>, IInventoryFloorsRepository
    {
        private readonly IMapper _mapper;
        public InventoryFloorsRepository(SSInventoryDbContext dbContext, IMapper mapper)
            : base(dbContext)
        {
            _mapper = mapper;
        }

        public virtual async Task<List<InventoryFloorModel>> ReadAsync(int? clientId = null, List<int> ids = null)
        {
            var data = GetAll().WhereIf(ids?.Any() == true, x => ids.Contains(x.InventoryFloorId));
            if(clientId.GetValueOrDefault(0) > 0)
            {
                data = data.Where(x => x.ClientId == clientId);
            }

            var entities = await data.ToListAsync();

            return _mapper.Map<List<InventoryFloorModel>>(entities);
        }
    }
}
