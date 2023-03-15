namespace SSInventory.Share.Models.Dto.Submission
{
    public class SubmissionResponseModel : ItemTypeModel
    {
        public int SubmissionId { get; set; }
        public int InventoryId { get; set; }
        //public string InventoryQRcode { get; set; }
    }
}
