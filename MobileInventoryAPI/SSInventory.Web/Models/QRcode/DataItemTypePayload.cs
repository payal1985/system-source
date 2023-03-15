using SSInventory.Share.Enums;
using SSInventory.Share.Models.Dto.ItemTypes;
using SSInventory.Share.Ultilities;
using static QRCoder.PayloadGenerator;

namespace SSInventory.Web.Models.QRcode
{
    public class DataItemTypePayload : Payload
    {
        private readonly QrGeneratorType type;
        private readonly DataItemType dataItemType;
        public DataItemTypePayload(DataItemType dataItemType, QrGeneratorType type = QrGeneratorType.JsonOnly)
        {
            this.dataItemType = dataItemType;
            this.type = type;
        }

        public override string ToString()
        {
            if (this.type == QrGeneratorType.JsonOnly)
            {
                return $"[{System.Text.Json.JsonSerializer.Serialize(this.dataItemType)}]";
            }
            if (this.type == QrGeneratorType.StringOnly)
            {
                return this.dataItemType.ConvertObjectToString();
            }

            return $"QRJson:[{System.Text.Json.JsonSerializer.Serialize(this.dataItemType)}]\r\nQRstring:[{this.dataItemType.ConvertObjectToString()}]";
        }
    }
}
