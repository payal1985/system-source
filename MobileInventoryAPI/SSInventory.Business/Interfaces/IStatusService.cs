using SSInventory.Share.Models.Dto.Status;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SSInventory.Business.Interfaces
{
    public interface IStatusService
    {
        Task<List<StatusModel>> ReadAsync(List<int> ids = null, List<string> types = null);
    }
}
