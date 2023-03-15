using System;
using System.Collections.Generic;

#nullable disable

namespace SSInventory.Core.Models
{
    public partial class Roles
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string RoleDescription { get; set; }
        public int? StatusId { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? LastModBy { get; set; }
        public DateTime? LastModDate { get; set; }
        public int? DepartmentId { get; set; }
    }
}
