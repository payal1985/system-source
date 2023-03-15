using Microsoft.AspNetCore.Hosting;
using SSInventory.Share.Constants;
using SSInventory.Share.Models.Dto.Inventory;
using SSInventory.Share.Models.Dto.InventoryItem;
using SSInventory.Share.Models.Dto.ItemTypes;
using SSInventory.Web.Utilities;
using System.IO;
using System.Linq;

namespace SSInventory.Web.Helpers
{
    /// <summary>
    /// Inventory helpers
    /// </summary>
    public static class InventoryHelpers
    {
        /// <summary>
        /// Collect selected values to save to Inventory table
        /// </summary>
        /// <param name="itemOption"></param>
        /// <param name="inventory"></param>
        public static void CollectInventoryInfoFromJson(this ItemTypeOptionApiModel itemOption, InventoryModel inventory)
        {
            if (itemOption.ItemTypeOptionCode.Equals(ItemTypeOptionCodeConstants.Description))
            {
                inventory.Description = itemOption.ItemTypeOptionReturnValue.ParseArrayToString();
                return;
            }
            if (itemOption.ItemTypeOptionCode.Equals(ItemTypeOptionCodeConstants.Manufacturer))
            {
                var selectedValue = GetSelectedInventoryItem(itemOption.ItemTypeOptionReturnValue);
                if (selectedValue is null) return;

                inventory.ManufacturerName = selectedValue.ReturnName;
                inventory.ManufacturerId = selectedValue.ReturnID;
                return;
            }
            if (itemOption.ItemTypeOptionCode.Equals(ItemTypeOptionCodeConstants.Modular))
            {
                var selectedManyValues = itemOption.ItemTypeOptionReturnValue.ParseSelectedItemOptions();
                if (selectedManyValues is null || selectedManyValues?.Any() == false) return;

                var selectedValues = selectedManyValues.Where(x => !string.IsNullOrWhiteSpace(x?.ItemTypeOptionLineName))
                                                       .Select(x => x.ItemTypeOptionLineName).ToList();
                if (selectedValues.Count == 0) return;

                inventory.Modular = string.Join(",", selectedValues);
                return;
            }
            if (itemOption.ItemTypeOptionCode.Equals(ItemTypeOptionCodeConstants.PartNumber))
            {
                inventory.PartNumber = itemOption.ItemTypeOptionReturnValue.ParseArrayToString();
                return;
            }
            if (itemOption.ItemTypeOptionCode.Equals(ItemTypeOptionCodeConstants.Finish))
            {
                var (finish1, finish2) = itemOption.ItemTypeOptionReturnValue.ParseCoupleValues();
                inventory.Finish = finish1;
                if (!string.IsNullOrWhiteSpace(finish2))
                    inventory.Finish2 = finish2;
                return;
            }
            if (itemOption.ItemTypeOptionCode.Equals(ItemTypeOptionCodeConstants.Fabric))
            {
                var (fabric1, fabric2) = itemOption.ItemTypeOptionReturnValue.ParseCoupleValues();
                inventory.Fabric = fabric1;
                if (!string.IsNullOrWhiteSpace(fabric2))
                    inventory.Fabric2 = fabric2;
                return;
            }
            if (itemOption.ItemTypeOptionCode.Equals(ItemTypeOptionCodeConstants.Height))
            {
                inventory.Height = itemOption.ItemTypeOptionReturnValue.ConvertReturnValueToDecimal();
                return;
            }
            if (itemOption.ItemTypeOptionCode.Equals(ItemTypeOptionCodeConstants.Width))
            {
                inventory.Width = itemOption.ItemTypeOptionReturnValue.ConvertReturnValueToDecimal();
                return;
            }
            if (itemOption.ItemTypeOptionCode.Equals(ItemTypeOptionCodeConstants.TopFinish))
            {
                inventory.Top = itemOption.ItemTypeOptionReturnValue.ParseItemTypeOptionReturnValueSingleValue();
                return;
            }
            if (itemOption.ItemTypeOptionCode.Equals(ItemTypeOptionCodeConstants.EdgeFinish))
            {
                inventory.Edge = itemOption.ItemTypeOptionReturnValue.ParseItemTypeOptionReturnValueSingleValue();
                return;
            }
            if (itemOption.ItemTypeOptionCode.Equals(ItemTypeOptionCodeConstants.BaseFinish))
            {
                inventory.Base = itemOption.ItemTypeOptionReturnValue.ParseItemTypeOptionReturnValueSingleValue();
                return;
            }
            if (itemOption.ItemTypeOptionCode.Equals(ItemTypeOptionCodeConstants.Depth))
            {
                inventory.Depth = itemOption.ItemTypeOptionReturnValue.ConvertReturnValueToDecimal();
                return;
            }
            if (itemOption.ItemTypeOptionCode.Equals(ItemTypeOptionCodeConstants.Diameter))
            {
                inventory.Diameter = itemOption.ItemTypeOptionReturnValue.ConvertReturnValueToDecimal();
                return;
            }
            if (itemOption.ItemTypeOptionCode.Equals(ItemTypeOptionCodeConstants.FrameFinish))
            {
                inventory.Frame = itemOption.ItemTypeOptionReturnValue.ParseItemTypeOptionReturnValueSingleValue();
                return;
            }
            if (itemOption.ItemTypeOptionCode.Equals(ItemTypeOptionCodeConstants.BackFinish))
            {
                inventory.Back = itemOption.ItemTypeOptionReturnValue.ParseItemTypeOptionReturnValueSingleValue();
                return;
            }
            if (itemOption.ItemTypeOptionCode.Equals(ItemTypeOptionCodeConstants.SeatFinish))
            {
                inventory.Seat = itemOption.ItemTypeOptionReturnValue.ParseItemTypeOptionReturnValueSingleValue();
                return;
            }
            if (itemOption.ItemTypeOptionCode.Equals(ItemTypeOptionCodeConstants.SeatHeight))
            {
                inventory.SeatHeight = itemOption.ItemTypeOptionReturnValue.ParseItemTypeOptionReturnValueSingleValue();
                return;
            }
            if (itemOption.ItemTypeOptionCode.Equals(ItemTypeOptionCodeConstants.Tag))
            {
                inventory.Tag = itemOption.ItemTypeOptionReturnValue.ParseItemTypeOptionReturnValueSingleValue();
                return;
            }
            if (itemOption.ItemTypeOptionCode.Equals(ItemTypeOptionCodeConstants.Unit))
            {
                inventory.Unit = itemOption.ItemTypeOptionReturnValue.ParseItemTypeOptionReturnValueSingleValue();
            }
        }

        /// <summary>
        /// Collect selected values to save to Inventory Item table
        /// </summary>
        /// <param name="itemOption"></param>
        /// <param name="inventoryItem"></param>
        public static void CollectInventoryItemInfoFromJson(this ItemTypeOptionApiModel itemOption, InventoryItemModel inventoryItem)
        {

            if (itemOption.ItemTypeOptionCode.Equals(ItemTypeOptionCodeConstants.GPS))
            {
                inventoryItem.GpsLocation = itemOption.ItemTypeOptionReturnValue.ParseArrayToString();
                return;
            }
            if (itemOption.ItemTypeOptionCode.Equals(ItemTypeOptionCodeConstants.Building))
            {
                var selectedValue = GetSelectedInventoryItem(itemOption.ItemTypeOptionReturnValue);
                if (selectedValue is null) return;

                inventoryItem.InventoryBuildingId = selectedValue.ReturnID;
                return;
            }
            if (itemOption.ItemTypeOptionCode.Equals(ItemTypeOptionCodeConstants.Floor))
            {
                var selectedValue = GetSelectedInventoryItem(itemOption.ItemTypeOptionReturnValue);
                if (selectedValue is null) return;

                inventoryItem.InventoryFloorId = selectedValue.ReturnID;
                return;
            }
            if (itemOption.ItemTypeOptionCode.Equals(ItemTypeOptionCodeConstants.AreaOrRoom))
            {
                inventoryItem.Room = itemOption.ItemTypeOptionReturnValue.ParseArrayToString();
            }
        }

        /// <summary>
        /// Check an image existed in temporary folder
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="environment"></param>
        /// <param name="folderPath"></param>
        /// <returns></returns>
        public static bool CheckExistingImageInTempFolder(this string fileName, IWebHostEnvironment environment, string folderPath)
        {
            if (string.IsNullOrWhiteSpace(fileName)) return false;

            var sourcePath = @$"{environment.ContentRootPath}\{Path.Combine(folderPath, fileName)}";

            return File.Exists(sourcePath);
        }

        #region Private methods

        private static SelectItemOptionModel GetSelectedInventoryItem(object returnValue)
        {
            var selectedManyValues = returnValue.ParseSelectedDropdownValueOption();
            if (selectedManyValues is null || selectedManyValues?.Any() == false) return null;

            var selectedValues = selectedManyValues.FirstOrDefault(x => !string.IsNullOrWhiteSpace(x?.ReturnName));
            return selectedValues ?? null;
        }

        #endregion
    }
}
