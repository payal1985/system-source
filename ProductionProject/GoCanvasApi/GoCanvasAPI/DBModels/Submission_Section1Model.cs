using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GoCanvasAPI.DBModels
{
    [Table("Submission_Section1Model")]
    //[Keyless]
    public class Submission_Section1Model
    {
        [Key]
        public Guid Sub_Section1_table_id { get; set; }
        public string SectionName { get; set; }
        public string Section_Screen_Name { get; set; }
        public string Section_Screen_Response_Guid { get; set; }
        public string Section_Screen_Response_Label { get; set; }
        public string Section_Screen_Response_Value { get; set; }
        public string Section_Screen_Response_Type { get; set; }
        public string SubmissionId { get; set; }

    }
}
