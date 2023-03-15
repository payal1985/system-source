using SSInventory.Share.Enums;
using SSInventory.Share.Models.Dto.ItemTypes;
using SSInventory.Share.Ultilities;
using static QRCoder.PayloadGenerator;

namespace SSInventory.Web.Models.QRcode
{
    public class ItemTypeOptionPayload : Payload
    {
        private readonly ItemTypeOptionApiModel itemTypeOption;
        private readonly QrGeneratorType type;

        public ItemTypeOptionPayload(ItemTypeOptionApiModel itemTypeOption, QrGeneratorType type = QrGeneratorType.JsonOnly)
        {
            this.itemTypeOption = itemTypeOption;
            this.type = type;
        }

        public override string ToString()
        {
            if (this.type == QrGeneratorType.JsonOnly)
            {
                return $"[{System.Text.Json.JsonSerializer.Serialize(this.itemTypeOption)}]";
            }
            if (this.type == QrGeneratorType.StringOnly)
            {
                return this.itemTypeOption.ConvertObjectToString();
            }

            return $"QRJson:[{System.Text.Json.JsonSerializer.Serialize(this.itemTypeOption)}]\r\nQRstring:[{this.itemTypeOption.ConvertObjectToString()}]";
        }
    }
}
