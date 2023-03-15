using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryApi.Models
{
    public class InventoryItemConditionModel
    {
        public int InventoryItemConditionID { get; set; }
        public string ConditionName { get; set; }
        public int OrderSequence { get; set; }

    }
}
