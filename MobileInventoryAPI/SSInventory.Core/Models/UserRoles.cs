using System;
using System.Collections.Generic;

#nullable disable

namespace SSInventory.Core.Models
{
    public partial class UserRoles
    {
        public int UserRoleId { get; set; }
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public int? StatusId { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? LastModBy { get; set; }
        public DateTime? LastModDate { get; set; }
    }
}
