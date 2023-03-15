using SSInventory.Core.Models;
using SSInventory.Share.Models.Dto.Status;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SSInventory.Core.Repositories.Interfaces
{
    public interface IStatusRepository : IRepository<Status>
    {
        Task<List<StatusModel>> ReadAsync(List<int> ids = null, List<string> types = null);
    }
}
