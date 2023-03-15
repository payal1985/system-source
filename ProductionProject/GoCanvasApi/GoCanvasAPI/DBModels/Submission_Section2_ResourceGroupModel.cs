using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GoCanvasAPI.DBModels
{
    [Table("Submission_Section2_ResourceGroupModel")]
    public class Submission_Section2_ResourceGroupModel
    {
        [Key]
        public Guid Sub_Sec2_ResGrp_Section_table_id { get; set; }
        public string ResGrp_Section_Name { get; set; }
        public string ResGrp_Section_Screen_Name { get; set; }
        public string ResGrp_Section_Screen_Response_Guid { get; set; }
        public string ResGrp_Section_Screen_Response_Label { get; set; }
        public string ResGrp_Section_Screen_Response_Value { get; set; }
        public string ResGrp_Section_Screen_Response_Type { get; set; }
        public string SubmissionId { get; set; }
        public string Section_Screen_ResponseGroup_Response_Value { get; set; }

    }
}
