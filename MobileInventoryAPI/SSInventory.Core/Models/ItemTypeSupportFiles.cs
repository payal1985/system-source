using System;
using System.Collections.Generic;

#nullable disable

namespace SSInventory.Core.Models
{
    public partial class ItemTypeSupportFiles
    {
        public int ItemTypeSupportFileId { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public DateTime CreateDateTime { get; set; }
        public int CreateId { get; set; }
        public DateTime? UpdateDateTime { get; set; }
        public int? UpdateId { get; set; }
    }
}
