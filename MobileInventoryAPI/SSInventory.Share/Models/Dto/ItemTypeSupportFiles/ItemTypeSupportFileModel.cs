using System;

namespace SSInventory.Share.Models.Dto.ItemTypeSupportFiles
{
    public class ItemTypeSupportFileModel
    {
        public int ItemTypeSupportFileId { get; set; }
        public string Desc { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public DateTime CreateDateTime { get; set; }
        public int CreateId { get; set; }
        public DateTime? UpdateDateTime { get; set; }
        public int? UpdateId { get; set; }
    }
}
