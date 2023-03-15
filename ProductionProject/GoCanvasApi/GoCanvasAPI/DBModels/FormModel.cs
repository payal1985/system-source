using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GoCanvasAPI.DBModels
{
    [Table("FormModel")]
    //[Keyless]
    public class FormModel
    {
        //[Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //public int Form_table_Id { get; set; }
        [Key]
        public Guid FormData_table_id { get; set; }
        public string FormId { get; set; }
        public string OriginatingLibraryTemplateId { get; set; }
        public string GUID { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public string Version { get; set; }
    }
}
