namespace SSInventory.Share.Enums
{
    /// <summary>
    /// Generate data into QRcode with type
    /// </summary>
    public enum QrGeneratorType
    {
        /// <summary>
        /// Generate data as json string format
        /// </summary>
        JsonOnly = 1,
        /// <summary>
        /// Generate data as string format
        /// </summary>
        StringOnly = 2,
        /// <summary>
        /// Generate data as json and string formats
        /// </summary>
        Both = 3
    }
}
