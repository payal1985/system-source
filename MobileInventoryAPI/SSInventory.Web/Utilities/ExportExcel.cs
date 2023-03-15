using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using SSInventory.Web.Models.Files;
using Microsoft.Extensions.Caching.Memory;
using SSInventory.Share.Models.Dto.Submission;
using System.IO;

namespace SSInventory.Web.Utilities
{
    public class ExportExcel
    {
        private readonly IMemoryCache _memoryCache;

        private readonly string[] _inventoyDetailsHeaderText = new[]
        {
            "Inventory ID", "Description", "Fabric 1", "Fabric 2", "Finish 1", "Finish 2", "Size", "Part Number", "Diameter",
            "Height", "Width", "Depth", "Top", "Edge", "Base", "Frame", "Seat", "Back", "Seat Height", "Modular", "Main Image", "Tag", "Unit",
            "Inventory Item ID", "Status ID", "Room", "Condition", "Notes", "Gps Location", "RFID"
        };

        private readonly string[] _inventoryItemImageHeaderText = new[]
        {
                "Inventory Item Image ID", "Image Name", "Image Url", "Width", "Height"
        };

        public ExportExcel(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public FileDto ExportSubmissionToFile(List<ExportSubmissionModel> submissions, string baseUrl)
        {

            return CreateExcelPackage(
                $"Submission-{submissions[0].ClientId}-{submissions[0].SubmissionId}-{submissions[0].Client}.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add($"Submission-{submissions[0].SubmissionId}");
                    AddHeader(sheet, 1, startCol: 0, _inventoyDetailsHeaderText);
                    AddSubmissionDetails(sheet, 2, submissions[0].Inventories, baseUrl);
                    sheet.Columns.AutoFit();
                });
        }

        /// <summary>
        /// Create excel package
        /// </summary>
        /// <returns></returns>
        public FileDto CreateExcelPackage(string fileName, Action<ExcelPackage> creator)
        {
            var file = new FileDto(fileName, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            using (var excelPackage = new ExcelPackage())
            {
                creator(excelPackage);
                Save(excelPackage, file);
            }

            return file;
        }

        protected void AddHeader(ExcelWorksheet sheet, int rowIndex, int startCol = 0, params string[] headerTexts)
        {
            if (headerTexts?.Any() != true)
            {
                return;
            }

            for (var i = 0; i < headerTexts.Length; i++)
            {
                AddHeader(sheet, rowIndex, i + 1 + startCol, headerTexts[i]);
            }
        }

        protected void AddHeader(ExcelWorksheet sheet, int rowIndex, int columnIndex, string headerText)
        {
            sheet.Cells[rowIndex, columnIndex].Value = headerText;
            sheet.Cells[rowIndex, columnIndex].Style.Font.Bold = true;
        }

        private void AddInventoryItemObjects(ExcelWorksheet sheet, int startRowIndex, List<ExportInventoryItemModel> inventoryItems, ref int continueRowIndex,
            params Func<ExportInventoryItemModel, object>[] propertySelectors)
        {
            foreach (var inventoryitem in inventoryItems)
            {
                AddObjects(sheet, startRowIndex, new List<ExportInventoryItemModel> { inventoryitem }, startCol: 1, null, propertySelectors);
                if (inventoryitem.InventoryImages.Count > 0)
                {
                    AddHeader(sheet, ++startRowIndex, startCol: 2, _inventoryItemImageHeaderText);
                    AddObjects(sheet, ++startRowIndex, inventoryitem.InventoryImages, startCol: 2, null,
                        _ => _.InventoryImageId,
                        _ => _.ImageName,
                        _ => _.ImageUrl,
                        _ => _.Width,
                        _ => _.Height);
                    startRowIndex += inventoryitem.InventoryImages.Count;
                }
                else
                {
                    startRowIndex++;
                }
            }
            continueRowIndex = startRowIndex;
        }

        private void AddSubmissionDetails(ExcelWorksheet sheet, int startRowIndex, List<ExportInventoryModel> inventories, string baseUrl)
        {
            var inventoryItems = new List<ExportInventoryItemIncludeInventoryModel>();
            var inventoryItemsData = inventories.SelectMany(x => x.InventoryItems).ToList();
            foreach (var inventoryItem in inventoryItemsData)
            {
                var inventory = inventories.First(x => x.InventoryId == inventoryItem.InventoryId);
                var model = new ExportInventoryItemIncludeInventoryModel
                {
                    InventoryId = inventory.InventoryId,
                    Description = inventory.Description,
                    Fabric = inventory.Fabric,
                    Fabric2 = inventory.Fabric2,
                    Finish = inventory.Finish,
                    Finish2 = inventory.Finish2,
                    Size = inventory.Size,
                    Unit = inventory.Unit,
                    Ownership = inventory.Ownership,
                    PartNumber = inventory.PartNumber,
                    Diameter = inventory.Diameter,
                    Height = inventory.Height,
                    Width = inventory.Width,
                    Depth = inventory.Depth,
                    Top = inventory.Top,
                    Edge = inventory.Edge,
                    Base = inventory.Base,
                    Frame = inventory.Frame,
                    Seat = inventory.Seat,
                    Back = inventory.Back,
                    SeatHeight = inventory.SeatHeight,
                    Modular = inventory.Modular,
                    MainImage = baseUrl + inventory.MainImage,
                    Tag = inventory.Tag,
                    InventoryItemId = inventoryItem.InventoryItemId,
                    LocationId = inventoryItem.LocationId,
                    Room = inventoryItem.Room,
                    Condition = inventoryItem.Condition,
                    Notes = inventoryItem.Notes,
                    GpsLocation = inventoryItem.GpsLocation,
                    Rfidcode = inventoryItem.Rfidcode
                };
                inventoryItems.Add(model);
            }

            AddObjects(sheet, startRowIndex, inventoryItems, startCol: 0, 20,
                _ => _.InventoryId,
                _ => _.Description,
                _ => _.Fabric,
                _ => _.Fabric2,
                _ => _.Finish,
                _ => _.Finish2,
                _ => _.Size,
                _ => _.PartNumber,
                _ => _.Diameter,
                _ => _.Height,
                _ => _.Width,
                _ => _.Depth,
                _ => _.Top,
                _ => _.Edge,
                _ => _.Base,
                _ => _.Frame,
                _ => _.Seat,
                _ => _.Back,
                _ => _.SeatHeight,
                _ => _.Modular,
                _ => _.MainImage,
                _ => _.Tag,
                _ => _.Unit,
                _ => _.InventoryItemId,
                _ => _.StatusId,
                _ => _.Room,
                _ => _.Condition,
                _ => _.Notes,
                _ => _.GpsLocation,
                _ => _.Rfidcode
            );
        }

        protected void AddObjects<T>(ExcelWorksheet sheet, int startRowIndex, IList<T> items, int startCol, int? imageIndex = null, params Func<T, object>[] propertySelectors)
        {
            if (items?.Count == 0 || propertySelectors?.Any() != true)
            {
                return;
            }

            for (var i = 0; i < items.Count; i++)
            {
                sheet.Row(i + startRowIndex).Height = 40;
                for (var j = 0; j < propertySelectors.Length; j++)
                {
                    if (imageIndex.HasValue && j == imageIndex)
                    {
                        var imgPath = propertySelectors[imageIndex.Value](items[i]).ToString();
                        if (File.Exists(imgPath))
                        {
                            var fileName = Path.GetFileName(imgPath);
                            try
                            {
                                var picture = sheet.Drawings.AddPicture($"{i}{fileName}", imgPath);
                                picture.SetSize(70, 50);
                                picture.SetPosition(i + startRowIndex - 1, 0, j, 0);
                            }
                            catch { }
                        }
                        else
                        {
                            sheet.Cells[i + startRowIndex, j + 1 + startCol].Value = propertySelectors[j](items[i]);
                        }
                    }
                    else
                    {
                        sheet.Cells[i + startRowIndex, j + 1 + startCol].Value = propertySelectors[j](items[i]);
                    }
                }
            }
        }

        protected void Save(ExcelPackage excelPackage, FileDto file)
        {
            file.FileContent = excelPackage.GetAsByteArray();
            SetFile(file.FileToken, file.FileContent);
        }

        public void SetFile(string token, byte[] content)
        {
            _memoryCache.Set(token, new TempFileInfo(content), TimeSpan.FromMinutes(1)); // expire time is 1 min by default
        }

        protected byte[] GetFile(string token)
        {
            var cache = _memoryCache.Get(token) as TempFileInfo;
            return cache?.File;
        }
    }
}
