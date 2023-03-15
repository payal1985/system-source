using SSInventory.Share.Models.Dto.Client;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SSInventory.Business.Interfaces
{
    public interface IClientService
    {
        Task<List<ClientModel>> ReadAsync(List<int> ids = null, List<int> userIds = null);
    }
}
