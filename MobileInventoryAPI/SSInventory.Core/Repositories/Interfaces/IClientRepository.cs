using SSInventory.Core.Models;
using SSInventory.Share.Models.Dto.Client;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SSInventory.Core.Repositories.Interfaces
{
    public interface IClientRepository : IRepository<Client>
    {
        Task<List<ClientModel>> ReadAsync(List<int> ids = null, List<int> userIds = null);
    }
}
