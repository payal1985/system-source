using AutoMapper;
using SSInventory.Core.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using SSInventory.Core.Models;
using SSInventory.Share.Models;
using SSInventory.Share.Models.Dto.ItemTypes;

namespace SSInventory.Core.Repositories
{
    public class ItemTypeRepository : Repository<ItemTypes>, IItemTypeRepository
    {
        private readonly IMapper _mapper;
        public ItemTypeRepository(SSInventoryDbContext dbContext, IMapper mapper)
            : base(dbContext)
        {
            _mapper = mapper;
        }

        public virtual async Task<List<ItemTypeModel>> ReadAsync(int? clientId = null, List<int?> ids = null)
        {
            var data = GetAll().WhereIf((clientId ?? 0) > 0, x => x.ClientId == clientId || x.ClientId == 1)
                               .WhereIf(ids?.Any() == true, x => ids.Contains(x.ItemTypeId));
            
            var entities = await data.ToListAsync();

            return _mapper.Map<List<ItemTypeModel>>(entities);
        }

        public virtual async Task<List<ItemTypeMappingModel>> GetItemTypeOptionSetAsync(int? clientId, int itemTypeId)
        {
            var listTemp = new List<int> { 0, 3 };
            var result = await (from i in _dbContext.ItemTypes
                          from io in _dbContext.ItemTypeOptions.Where(p => (itemTypeId == 0 && listTemp.Contains(p.ItemTypeAttributeId)) ||
                             (i.ClientId == 1 || i.ClientId == clientId) && (p.ItemTypeAttributeId == 0 || i.ItemTypeAttributeId == p.ItemTypeAttributeId)).DefaultIfEmpty()
                          from il in _dbContext.ItemTypeOptionLines.Where(p => p.StatusId == 5 && (io.ItemTypeOptionId == p.ItemTypeOptionId)).DefaultIfEmpty()
                          from sf in _dbContext.ItemTypeSupportFiles.Where(p => p.ItemTypeSupportFileId == io.ItemTypeSupportFileId).DefaultIfEmpty()
                          where i.ItemTypeId == itemTypeId && io.StatusId == 5
                          select new ItemTypeMappingModel
                          {
                              ClientId = i.ClientId,
                              StatusId = io.StatusId,
                              ItemTypeID = i.ItemTypeId,
                              ItemTypeCode = i.ItemTypeCode,
                              ItemTypeName = i.ItemTypeName,
                              ItemTypeOptionID = io.ItemTypeOptionId,
                              ItemTypeOptionName = io.ItemTypeOptionName,
                              ItemTypeOptionCode = io.ItemTypeOptionCode,
                              OrderSequence = io.OrderSequence,
                              FieldType = io.FieldType,
                              ItemTypeAttributeId = io.ItemTypeAttributeId,
                              ItemTypeSupportFileID = io.ItemTypeSupportFileId,
                              ValType = io.ValType,
                              IsHide = io.IsHide,
                              ValTypeDesc = (
                                                 io.ValType == 1 ? "TextBox" :
                                                 io.ValType == 2 ? "TextParagraph" :
                                                 io.ValType == 3 ? "Dropdown" :
                                                 io.ValType == 4 ? "Date" :
                                                 io.ValType == 5 ? "Time" :
                                                 io.ValType == 6 ? "CheckBox" :
                                                 io.ValType == 20 ? "DataFromAPI" :
                                                 io.ValType == 30 ? "GPS" :
                                                 io.ValType == 40 ? "TotalCount" :
                                                 io.ValType == 50 ? "TextBoxPlus" :
                                                 io.ValType == 60 ? "QuantityPlus" :
                                                 io.ValType == 70 ? "PhotoPlus" :
                                                 io.ValType == 80 ? "RadioPlus" :
                                                 io.ValType == 90 ? "DropdownPlus" :
                                                 io.ValType == 100 ? "RadioButton" : ""
                                             ),
                              ItemTypeOptionLineID = il != null ? il.ItemTypeOptionLineId : 0,
                              ItemTypeOptionLineCode = il != null ? il.ItemTypeOptionLineCode : "",
                              ItemTypeOptionLineName = il != null ? il.ItemTypeOptionLineName : "",
                              InventoryUserAcceptanceRulesRequired = il != null ? il.InventoryUserAcceptanceRulesRequired : "",
                              SupportFileDesc = sf == null ? null : sf.Desc,
                              SupportFilePath = sf == null ? null : sf.FilePath,
                              SupportFileName = sf == null ? null : sf.FileName,
                              ItemTypeSupportID = sf == null ? 0 : sf.ItemTypeSupportFileId
                          }).OrderBy(p=>p.ItemTypeID).ThenBy(p=>p.OrderSequence).ToListAsync(); 

            return result;

        }
    }


}
