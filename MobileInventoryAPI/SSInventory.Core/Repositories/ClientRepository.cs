using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SSInventory.Core.Models;
using SSInventory.Core.Repositories.Interfaces;
using SSInventory.Share.Models.Dto.Client;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSInventory.Core.Repositories
{
    public class ClientRepository : Repository<Client>, IClientRepository
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        public ClientRepository(SSInventoryDbContext dbContext, IMapper mapper, IUserRepository userRepository)
            : base(dbContext)
        {
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public async Task<List<ClientModel>> ReadAsync(List<int> ids = null, List<int> userIds = null)
        {
            var entities = GetAll().WhereIf(ids?.Any() == true, x => ids.Contains(x.ClientId))
                                   .AsNoTracking();

            var result = await entities.ToListAsync();
            return _mapper.Map<List<ClientModel>>(result);
        }
    }
}
