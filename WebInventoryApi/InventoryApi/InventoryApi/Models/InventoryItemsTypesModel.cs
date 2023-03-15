using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryApi.Models
{
    public class InventoryItemsTypesModel
    {
        public List<SpaceTypesModel> SpaceTypesModels { get; set; }
        public List<InventoryOwnersModel> InventoryOwnersModels { get; set; }
    }

    public class SpaceTypesModel
    {
        public int SpaceTypeID { get; set; }
        public int ClientID { get; set; }
        public string SpaceTypeName { get; set; }
    }

    public class InventoryOwnersModel
    {
        public int InventoryOwnerID { get; set; }
        public int ClientID { get; set; }
        public string OwnerName { get; set; }

    }
}
