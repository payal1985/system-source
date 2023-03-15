using System;
using System.Collections.Generic;

#nullable disable

namespace SSInventory.Core.Models
{
    public partial class Submissions
    {
        public int SubmissionId { get; set; }
        public string Client { get; set; }
        public string Status { get; set; }
        public bool? IsBarScan { get; set; }
        public int? ClientId { get; set; }
        public string InventoryAppId { get; set; }
        public string DeviceDate { get; set; }
        public string TempColumnJsonForTesting { get; set; }
        public DateTime CreateDateTime { get; set; }
        public int CreateId { get; set; }
        public DateTime? UpdateDateTime { get; set; }
        public int? UpdateId { get; set; }
    }
}
