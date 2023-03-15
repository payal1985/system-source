namespace SSInventory.Share.Models.Dto.ItemTypes
{
    public class ItemTypeMappingModel
    {
        public int ItemTypeID { get; set; }
        public string ItemTypeCode { get; set; }
        public string ItemTypeName { get; set; }
        public int ClientId { get; set; }
        public int ItemTypeOptionID { get; set; }
        public string ItemTypeOptionName { get; set; }
        public string ItemTypeOptionCode { get; set; }
        public int Status { get; set; }
        public int OrderSequence { get; set; }
        public string FieldType { get; set; }
        public int ValType { get; set; }
        public int? LimitMin { get; set; }
        public int? LimitMax { get; set; }
        public bool IsRequired { get; set; }
        public bool IsHide { get; set; }
        public string ValTypeDesc { get; set; }
        public int ItemTypeOptionLineID { get; set; }
        public string ItemTypeOptionLineCode { get; set; }
        public string ItemTypeOptionLineName { get; set; }
        public string InventoryUserAcceptanceRulesRequired { get; set; }
        public int ItemTypeAttributeId { get; set; }
        public int ItemTypeSupportFileID { get; set; }

        public int ItemTypeSupportID { get; set; }
        public string SupportFileDesc { get; set; }
        public string SupportFilePath { get; set; }
        public string SupportFileName { get; set; }
        public int StatusId { get; set; }
    }
}
