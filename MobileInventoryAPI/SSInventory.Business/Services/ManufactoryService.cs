using SSInventory.Business.Interfaces;
using SSInventory.Core.Repositories.Interfaces;
using SSInventory.Share.Models;
using SSInventory.Share.Models.Dto.ItemTypeOptionLines;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SSInventory.Business.Services
{
    public class ManufactoryService : IManufactoryService
    {
        private readonly IManufactoryRepository _manufactoryRepository;

        public ManufactoryService(IManufactoryRepository manufactoryRepository)
        {
            _manufactoryRepository = manufactoryRepository;
        }

        public virtual async Task<List<ItemTypeOptionLineModel>> InsertAsync(List<CreateOrUpdateItemTypeOptionLineModel> model)
            => await _manufactoryRepository.InsertAsync(model);
    }
}
