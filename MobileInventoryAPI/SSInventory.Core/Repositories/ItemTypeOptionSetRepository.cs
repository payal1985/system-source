using AutoMapper;
using SSInventory.Core.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using SSInventory.Share.Models.Dto.ItemTypes;
using SSInventory.Core.Models;
using SSInventory.Share.Models;

namespace SSInventory.Core.Repositories
{
    public class ItemTypeOptionSetRepository : Repository<ItemTypes>, IItemTypeOptionSetRepository
    {
        private readonly IMapper _mapper;
        
        public ItemTypeOptionSetRepository(SSInventoryDbContext dbContext, IMapper mapper)
            : base(dbContext)
        {
            _mapper = mapper;
        }

        public virtual async Task<List<ItemTypeModel>> ReadAsync(string keyword = "")
        {
            var data = GetAll();
            
            if(!string.IsNullOrWhiteSpace(keyword))
            {
                data = data.Where(x => EF.Functions.Like(x.ItemTypeName, $"{keyword}") || EF.Functions.Like(x.ItemTypeDesc, $"{keyword}"));
            }
            var entities = await data.ToListAsync();
            return _mapper.Map<List<ItemTypeModel>>(entities);
        }

        public virtual async Task<CreateOrUpdateItemTypeModel> CreateAsync(CreateOrUpdateItemTypeModel model)
        {
            var entity = _mapper.Map<ItemTypes>(model);
            entity.CreateDateTime = System.DateTime.Now;
            entity.ItemTypeAttributeId = 3;
            await AddAsync(entity);

            return _mapper.Map<CreateOrUpdateItemTypeModel>(entity);
        }

        public virtual async Task<CreateOrUpdateItemTypeModel> UpdateAsync(CreateOrUpdateItemTypeModel model)
        {
            var entity = await GetAll().AsNoTracking().FirstOrDefaultAsync(x => x.ItemTypeId == model.ItemTypeId);
            if (entity is null) return null;

            entity = _mapper.Map<ItemTypes>(model);
            entity.UpdateDateTime = System.DateTime.Now;
            await UpdateAsync(entity);

            return model;
        }

        public virtual async Task<bool> DeleteAsync(List<int> ids)
        {
            var entities = await GetAll().Where(x => ids.Contains(x.ItemTypeId)).AsNoTracking().ToListAsync();
            if(entities?.Any() == true)
            {
                await DeleteAsync(entities);

                return true;
            }

            return false;
        }
    }
}
