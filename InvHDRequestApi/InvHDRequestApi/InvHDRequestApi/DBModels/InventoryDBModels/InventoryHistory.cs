using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace InvHDRequestApi.DBModels.InventoryDBModels
{
    [Table("InventoryHistory")]
    public class InventoryHistory
    {
        [Key]
        public int InventoryHistoryID { get; set; }

        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid BatchTransactionGUID { get; set; }
        public int? OrderID { get; set; }
        public int EntityID { get; set; }
        public string? ApiName { get; set; }
        public string TableName { get; set; }
        public string? ColumnName { get; set; }
        public string? OldValue { get; set; }
        public string? NewValue { get; set; }
        public string? Description { get; set; }
        public DateTime CreateDateTime { get; set; }
        public int CreateID { get; set; }

    }
}
