namespace SSInventory.Web.Models
{
    /// <summary>
    /// Clean up paramters setting for background job
    /// </summary>
    public class CleanupAppSettings
    {
        /// <summary>
        /// Root file path
        /// </summary>
        public string SaveFileRootPath { get; set; }

        /// <summary>
        /// Inteval time will executes the background
        /// </summary>
        public long CleanupOldFilesExpiration { get; set; } = 86400000;

        /// <summary>
        /// File will be deleted after X day
        /// </summary>
        public int DeleteFilesAfterXday { get; set; } = 1;

        /// <summary>
        /// Temporary folder name
        /// </summary>
        public string TemporaryFolderName { get; set; }

        /// <summary>
        /// Orgin folder name
        /// </summary>

        public string UploadOriginFolder { get; set; }
    }
}
