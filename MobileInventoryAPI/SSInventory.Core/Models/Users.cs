using System;
using System.Collections.Generic;

#nullable disable

namespace SSInventory.Core.Models
{
    public partial class Users
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string PasswordEncrypted { get; set; }
        public string PreferLanguage { get; set; }
        public string CellPhone { get; set; }
        public string WorkPhone { get; set; }
        public int? UserTypeId { get; set; }
        public int? UserRoleId { get; set; }
        public int? CompanyId { get; set; }
        public int? StatusId { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? LastModBy { get; set; }
        public DateTime? LastModDate { get; set; }
        public string UserLname { get; set; }
        public string UserFname { get; set; }
    }
}
