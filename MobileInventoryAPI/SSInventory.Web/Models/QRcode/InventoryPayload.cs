using SSInventory.Share.Enums;
using SSInventory.Share.Models.Dto.Inventory;
using SSInventory.Share.Ultilities;
using static QRCoder.PayloadGenerator;

namespace SSInventory.Web.Models
{
    public class InventoryPayload : Payload
    {
        private readonly InventoryModel inventory;
        private readonly QrGeneratorType type;

        public InventoryPayload(InventoryModel inventory, QrGeneratorType type = QrGeneratorType.JsonOnly)
        {
            this.inventory = inventory;
            this.type = type;
        }

        public override string ToString()
        {
            if(this.type == QrGeneratorType.JsonOnly)
{
                return $"[{System.Text.Json.JsonSerializer.Serialize(this.inventory)}]";
            }
            if (this.type == QrGeneratorType.StringOnly)
            {
                return this.inventory.ConvertObjectToString();
            }

            return $"QRJson:[{System.Text.Json.JsonSerializer.Serialize(this.inventory)}]\r\nQRstring:[{this.inventory.ConvertObjectToString()}]";
        }
    }
}
