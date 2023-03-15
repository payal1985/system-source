using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryApi.Models
{
    public class CompanyUserModel
    {
        public int UserId {get;set;}
        public string UserName { get; set; }
    }

    public class AssignToModel
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public int? AssignedToId { get; set; }
        public string InstallerInstruction { get; set; }

        public int? DestBuildingId { get; set; }
        public int DestFloorId { get; set; }

        public string DestRoom { get; set; }
        public int AssignToCompanyId { get; set; }
    }

    public class CompanyModel
    {
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
    }
}
