using SSInventory.Business.Interfaces;
using SSInventory.Core.Repositories.Interfaces;
using SSInventory.Share.Models;
using SSInventory.Share.Models.Dto.Manufactory;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SSInventory.Business.Services
{
    public class ManufacturerService : IManufacturerService
    {
        private readonly IManufacturerRepository _manufacturerRepository;

        public ManufacturerService(IManufacturerRepository manufacturerRepository)
        {
            _manufacturerRepository = manufacturerRepository;
        }

        public Task<List<ManufacturerModel>> ReadAsync(List<int> ids = null, string manufacturerName = "", string vendorNum = "", List<string> pmTypes = null)
            => _manufacturerRepository.ReadAsync(ids, manufacturerName, vendorNum, pmTypes);

        public Task<ResponseModel> CreateAsync(CreateOrEditManufacturerModel model)
            => _manufacturerRepository.CreateAsync(model);
    }
}
