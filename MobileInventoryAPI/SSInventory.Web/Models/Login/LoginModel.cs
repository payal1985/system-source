using System.ComponentModel.DataAnnotations;

namespace SSInventory.Web.Models
{
    /// <summary>
    /// Login paramters
    /// </summary>
    public class LoginModel
    {
        /// <summary>
        /// User email address
        /// </summary>
        [EmailAddress]
        public string LoginEmail { get; set; }

        /// <summary>
        /// User password
        /// </summary>
        public string LoginPW { get; set; }
    }
}
