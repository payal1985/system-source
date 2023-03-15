using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvHDRequestApi.Models
{
    public class ChildInventoryItemModel
    {
        public int InventoryItemID { get; set; }
        public string Building { get; set; }
        public string Floor { get; set; }
        public string Room { get; set; }
        public string Condition { get; set; }
        public int ConditionId { get; set; }
        public int Qty { get; set; }
        public int InventoryID { get; set; }
        public string InventoryImageName { get; set; }
        public string InventoryImageUrl { get; set; }

        public bool IsSelected { get; set; } = false;
        public string ImageBase64 { get; set; }
        public string BucketName { get; set; }
        public string ImagePath { get; set; }

        public string DamageNotes { get; set; }

    }
}
