using System;

namespace SSInventory.Share.Models.Dto.Submission
{
    public class SubmissionModel
    {
        public int? SubmissionId { get; set; }
        public string DeviceDate { get; set; }
        public string Client { get; set; }
        public int? ClientId { get; set; }
        public string Status { get; set; }
        public bool IsBarScan { get; set; }
        public string InventoryAppId { get; set; }
        public string TempColumnJsonForTesting { get; set; }
        public DateTime CreateDateTime { get; set; }
        public int CreateId { get; set; }
        public DateTime? UpdateDateTime { get; set; }
        public int? UpdateId { get; set; }
        public int InventoryBuildingID { get; set; }
        public int InventoryFloorID { get; set; }
    }
}
