using System.Collections.Generic;
using System;

namespace SSInventory.Share.Models.Dto.Inventory
{
    public class SearchInventoryModel
    {
        public int? ClientId { get; set; }
        public List<int> Ids { get; set; }
        public string Keyword { get; set; }
        public int? ItemTypeId { get; set; }
        public List<int> ItemRowIds { get; set; }
        public DateTime? DeviceDate { get; set; }
        public List<int?> SubmissionIds { get; set; }
    }
}
