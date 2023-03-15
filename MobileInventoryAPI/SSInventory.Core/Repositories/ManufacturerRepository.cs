using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SSInventory.Core.Models;
using SSInventory.Core.Repositories.Interfaces;
using SSInventory.Share.Models;
using SSInventory.Share.Models.Dto.Manufactory;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSInventory.Core.Repositories
{
    public class ManufacturerRepository : Repository<Manufacturers>, IManufacturerRepository
    {
        private readonly IMapper _mapper;
        public ManufacturerRepository(SSInventoryDbContext dbContext, IMapper mapper)
            : base(dbContext)
        {
            _mapper = mapper;
        }

        public async Task<List<ManufacturerModel>> ReadAsync(List<int> ids = null, string manufacturerName = "", string vendorNum = "", List<string> pmTypes = null)
        {
            var query = GetAll();

            if(ids?.Any() == true)
            {
                query = query.Where(x => ids.Contains(x.ManufacturerId));
            }
            if (!string.IsNullOrWhiteSpace(manufacturerName))
            {
                query = query.Where(x => EF.Functions.Like(x.ManufacturerName, $"%{manufacturerName}%"));
            }
            if (!string.IsNullOrWhiteSpace(vendorNum))
            {
                query = query.Where(x => EF.Functions.Like(x.VendorNum, $"%{vendorNum}%"));
            }
            if (pmTypes?.Any() == true)
            {
                query = query.Where(x => pmTypes.Contains(x.Pmtype));
            }

            query = query.OrderBy(x => x.ManufacturerName);
            var data = await query.ToListAsync();

            return _mapper.Map<List<ManufacturerModel>>(data);
        }

        public async Task<ResponseModel> CreateAsync(CreateOrEditManufacturerModel model)
        {
            if (string.IsNullOrWhiteSpace(model.ManufacturerName))
                return ResponseModel.Failed("Company name is required");

            var existing = await GetAll().AnyAsync(x => x.ManufacturerName.Equals(model.ManufacturerName));
            if(existing)
                return ResponseModel.Failed("Company name is existing");

            var entity = _mapper.Map<Manufacturers>(model);
            entity.CreateDateTime = System.DateTime.Now;
            if (string.IsNullOrWhiteSpace(entity.Pmtype))
            {
                entity.Pmtype = "P";
            }
            var response = await AddAsync(entity);
            return ResponseModel.Successed("Save manufacturer successfully", data: _mapper.Map<ManufacturerModel>(response));
        }
    }
}
