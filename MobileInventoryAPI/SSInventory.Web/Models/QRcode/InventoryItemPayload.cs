using SSInventory.Share.Enums;
using SSInventory.Share.Models.Dto.InventoryItem;
using SSInventory.Share.Ultilities;
using static QRCoder.PayloadGenerator;

namespace SSInventory.Web.Models
{
    /// <summary>
    /// Inventory Item QRCode
    /// </summary>
    public class InventoryItemPayload : Payload
    {
        private readonly InventoryItemQRcodeModel inventoryItem;
        private readonly QrGeneratorType type;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="inventoryItem"></param>
        /// <param name="type"></param>
        public InventoryItemPayload(InventoryItemQRcodeModel inventoryItem,
            QrGeneratorType type = QrGeneratorType.JsonOnly)
        {
            this.inventoryItem = inventoryItem;
            this.type = type;
        }

        /// <summary>
        /// Convert object to string to embed
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (this.type == QrGeneratorType.JsonOnly)
            {
                return $"[{System.Text.Json.JsonSerializer.Serialize(this.inventoryItem)}]";
            }
            if (this.type == QrGeneratorType.StringOnly)
            {
                return this.inventoryItem.ConvertObjectToString();
            }

            return $"QRJson:[{System.Text.Json.JsonSerializer.Serialize(this.inventoryItem)}]\r\n,QRstring:[{this.inventoryItem.ConvertObjectToString()}]";
        }
    }
}
