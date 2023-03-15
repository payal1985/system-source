using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryApi.Models
{
    public class InventoryItemWarrantyModel
    {
        public int InventoryItemID { get; set; }
        public string Building { get; set; }
        public string Floor { get; set; }
        public string Room { get; set; }
        public string Condition { get; set; }
        public int ConditionId { get; set; }
        public int InventoryBuildingID { get; set; }
        public int InventoryFloorID { get; set; }
        public int Qty { get; set; }
        public int PullQty { get; set; }
        public int InventoryID { get; set; }
        public string InventoryImageName { get; set; }
        public string InventoryImageUrl { get; set; }
        public string ItemCode { get; set; }
        public string Description { get; set; }
        public int ClientID { get; set; }
        public string Comment { get; set; }
        //public string username{get; set;}
        public string ReqName { get; set; }
        public string Email { get; set; }
        public string ClientName { get; set; }
        public string ClientPath { get; set; }

        public List<ChildInventoryItemModel> ChildInventoryItemModels { get; set; }
        public int UserId { get; set; }

        public string BucketName { get; set; }
        public string ImagePath { get; set; }

        public string[] Files { get; set; }
        public long FileSize { get; set; }

        public List<IFormFile> FileData { get; set; }
        //public IFormFileCollection FileData { get;set;}
    }

    public class InventoryItemWarrantyModelTest
    {
        public InventoryItemWarrantyModel warrantydatasource { get; set; }
        public List<IFormFile> file { get; set; }
    }
}
