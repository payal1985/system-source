using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryApi.Models
{
    public class OrderTypeModel
    {
        public int OrderTypeId { get; set; }
        public string OrderTypeName { get; set; }
        public string OrderTypeDesc { get; set; }
    }
}
