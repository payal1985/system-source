using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SSInventory.Core.Models;
using SSInventory.Core.Repositories.Interfaces;
using SSInventory.Share.Models.Dto.Status;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSInventory.Core.Repositories
{
    public class StatusRepository : Repository<Status>, IStatusRepository
    {
        private readonly IMapper _mapper;
        public StatusRepository(SSInventoryDbContext dbContext, IMapper mapper)
            : base(dbContext)
        {
            _mapper = mapper;
        }

        public async Task<List<StatusModel>> ReadAsync(List<int> ids = null, List<string> types = null)
        {
            var entities = GetAll().WhereIf(ids?.Any() == true, x => ids.Contains(x.StatusId))
                                   .WhereIf(types?.Any() == true, x => types.Contains(x.StatusType))
                                   .AsNoTracking();

            var result = await entities.ToListAsync();
            return _mapper.Map<List<StatusModel>>(result);
        }
    }
}
