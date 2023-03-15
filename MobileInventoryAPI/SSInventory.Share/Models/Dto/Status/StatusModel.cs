namespace SSInventory.Share.Models.Dto.Status
{
    public class StatusModel
    {
        public int StatusId { get; set; }
        public string StatusType { get; set; }
        public string StatusName { get; set; }
        public string StatusDesc { get; set; }
        public int? SortOrder { get; set; }
        public int CreateId { get; set; }
    }
}
