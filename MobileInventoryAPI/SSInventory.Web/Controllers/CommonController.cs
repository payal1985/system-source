using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SSInventory.Share.Models;
using SSInventory.Share.Models.Dto.InventoryItem;
using SSInventory.Share.Models.Dto.ItemTypes;
using SSInventory.Share.Models.Dto.ItemTypeSupportFiles;
using SSInventory.Share.Ultilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SSInventory.Web.Controllers
{
    /// <summary>
    /// Common controller
    /// </summary>
    public class CommonController : ControllerBase
    {
        /// <summary>
        /// Convert json data to ItemTypes model
        /// </summary>
        /// <param name="data"></param>
        /// <param name="isSearch"></param>
        /// <param name="dataItemType"></param>
        /// <param name="fetchFirst"></param>
        /// <param name="itemTypeOptionCodes"></param>
        /// <param name="inventoryItem">Response more inventory item information: StatusId, Condition, NoteForItem</param>
        /// <returns></returns>
        protected List<ItemTypeModel> ConvertToItemTypesModel(List<ItemTypeMappingModel> data, bool isSearch = false,
            DataItemType dataItemType = null, bool fetchFirst = false, List<string> itemTypeOptionCodes = null, InventoryItemModel inventoryItem = null)
        {
            var itemTypeIds = data.Select(x => x.ItemTypeID).Distinct();
            if (fetchFirst)
            {
                itemTypeIds = new List<int> { itemTypeIds.First() };
            }
            var result = new List<ItemTypeModel>();
            foreach (var itemTypeId in itemTypeIds)
            {
                var itemType = data.FirstOrDefault(x => x.ItemTypeID == itemTypeId);
                var itemTypeModel = ConvertToItemTypeModel(itemType, mainImage: isSearch ? dataItemType.MainImage : "", inventoryItem: inventoryItem);
                itemTypeModel.InventoryId = dataItemType?.InventoryId;
                foreach (var itemTypeOptionId in data.Where(x => x.ItemTypeID == itemTypeId).Select(x => x.ItemTypeOptionID).Distinct())
                {
                    var option = data.WhereIf(itemTypeOptionCodes?.Any() == true, x => itemTypeOptionCodes.Any(y => y.Equals(x.ItemTypeOptionCode)))
                                     .Where(x => x.ItemTypeOptionID == itemTypeOptionId).FirstOrDefault();
                    if (option is null) continue;

                    var itemTypeOption = ConvertToItemTypeOptionModel(option, itemTypeId, itemTypeOptionId);
                    if (isSearch)
                    {
                        // If is searching data, return value object must have included
                        itemTypeOption.ItemTypeOptionReturnValue = GetItemTypeOptionReturnValue(dataItemType.ItemTypeOptions, option.ItemTypeOptionCode);
                    }
                    foreach (var optionLine in data.Where(x => x.ItemTypeID == itemTypeId && x.ItemTypeOptionID == itemTypeOptionId))
                    {
                        if (optionLine.ItemTypeOptionLineID != 0)
                        {
                            itemTypeOption.ItemTypeOptionLines.Add(new ItemTypeOptionLineModel
                            {
                                ItemTypeOptionLineID = optionLine.ItemTypeOptionLineID,
                                ItemTypeOptionLineName = optionLine.ItemTypeOptionLineName,
                                ItemTypeOptionLineCode = optionLine.ItemTypeOptionLineCode,
                                ItemTypeOptionID = itemTypeOptionId,
                                InventoryUserAcceptanceRulesRequired = optionLine.InventoryUserAcceptanceRulesRequired
                            });
                        }
                    }
                    itemTypeModel.ItemTypeOptions.Add(itemTypeOption);
                }
                result.Add(itemTypeModel);
            }

            return result;
        }

        /// <summary>
        /// Save files
        /// </summary>
        /// <param name="files"></param>
        /// <param name="path"></param>
        /// <param name="hostingEnvironment"></param>
        /// <returns></returns>
        protected virtual async Task SaveFile(List<IFormFile> files, string path, IWebHostEnvironment hostingEnvironment)
        {
            foreach (var file in files)
            {
                var filePath = Path.Combine(path, file.FileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                await file.CopyToAsync(stream);
            }
        }

        /// <summary>
        /// Save bytes content to file
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        protected virtual async Task SaveBytesToFile(byte[] bytes, string target)
        {
            if (bytes.Length > 0)
            {
                FileStream fs = new FileStream(target, FileMode.CreateNew);
                await fs.WriteAsync(bytes, 0, bytes.Length);
                fs.Close();
            }
        }

        private ItemTypeModel ConvertToItemTypeModel(ItemTypeMappingModel itemType, string mainImage = "", InventoryItemModel inventoryItem = null)
        {
            var model = new ItemTypeModel
            {
                ItemTypeId = itemType.ItemTypeID,
                ClientId = itemType.ClientId,
                ItemTypeName = itemType.ItemTypeName,
                ItemTypeCode = itemType.ItemTypeCode.ConvertItemTypeName(itemType.ItemTypeName),
                ItemTypeOptions = new List<ItemTypeOptionModel>()
            };
            if (!string.IsNullOrWhiteSpace(mainImage))
            {
                model.MainImage = mainImage;
            }
            if(inventoryItem != null && inventoryItem.StatusId == 5)
            {
                model.StatusId = inventoryItem.StatusId;
                model.Condition = inventoryItem.Condition;
                model.NoteForItem = inventoryItem.DamageNotes;
                model.InventoryItemId = inventoryItem.InventoryItemId;
            }

            return model;
        }

        private ItemTypeOptionModel ConvertToItemTypeOptionModel(ItemTypeMappingModel option, int itemTypeId, int itemTypeOptionId)
        {
            return new ItemTypeOptionModel
            {
                ItemTypeOptionId = itemTypeOptionId,
                ItemTypeOptionName = option.ItemTypeOptionName,
                ItemTypeOptionCode = option.ItemTypeOptionCode,
                OrderSequence = option.OrderSequence,
                FieldType = option.FieldType,
                ValType = option.ValType,
                ItemTypeOptionDesc = option.ValTypeDesc,
                LimitMin = option.LimitMin ?? 0,
                LimitMax = option.LimitMax ?? 0,
                IsRequired = option.IsRequired,
                IsHide = option.IsHide,
                ItemTypeAttributeId = option.ItemTypeAttributeId,
                ItemTypeOptionLines = new List<ItemTypeOptionLineModel>(),
                InventorySupportFile = option.ItemTypeSupportFileID == 0 ? null : new ItemTypeSupportFileModel
                {
                    ItemTypeSupportFileId = option.ItemTypeSupportID,
                    Desc = option.SupportFileDesc,
                    FileName = option.SupportFileName,
                    FilePath = option.SupportFilePath
                }
            };
        }

        private object GetItemTypeOptionReturnValue(List<ItemTypeOptionApiModel> itemTypeOptionReturnValues, string itemTypeOptionCode)
        {
            if (string.IsNullOrWhiteSpace(itemTypeOptionCode) || itemTypeOptionReturnValues.Count == 0)
                return "";

            var selectedValue = itemTypeOptionReturnValues.FirstOrDefault(x => x.ItemTypeOptionCode.Equals(itemTypeOptionCode, StringComparison.CurrentCultureIgnoreCase));
            return selectedValue is null ? "" : selectedValue.ItemTypeOptionReturnValue;
        }
    }
}
