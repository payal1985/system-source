namespace SSInventory.Web.Models.Client.External
{
    /// <summary>
    /// Client information requested from external service
    /// </summary>
    public class ClientExternalResponseModel
    {
        /// <summary>
        /// Client ID
        /// </summary>
        public int ClientID { get; set; }
        
        /// <summary>
        /// Client name
        /// </summary>
        public string ClientName { get; set; }

        /// <summary>
        /// Inventory client group ID
        /// </summary>
        public int Inventory_Client_Group_ID { get; set; }
    }
}
