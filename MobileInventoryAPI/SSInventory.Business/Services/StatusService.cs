using SSInventory.Business.Interfaces;
using SSInventory.Core.Repositories.Interfaces;
using SSInventory.Share.Models.Dto.Status;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SSInventory.Business.Services
{
    public class StatusService : IStatusService
    {
        private readonly IStatusRepository _statusRepository;

        public StatusService(IStatusRepository statusRepository)
        {
            _statusRepository = statusRepository;
        }

        public async Task<List<StatusModel>> ReadAsync(List<int> ids = null, List<string> types = null)
            => await _statusRepository.ReadAsync(ids, types);
    }
}
