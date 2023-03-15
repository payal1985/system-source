using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GoCanvasAPI.DBModels
{
    [Table("Submission_Section2Model")]
    public class Submission_Section2Model
    {
       [Key]
       public Guid Sub_Section2_table_id { get; set; } 
       public string SectionName { get; set; }
       public string Section_Screen_Name { get; set; }
       public string Section_Screen_ResponseGroup_Guid { get; set; }
       public string Section_Screen_ResponseGroup_Response_Guid { get; set; }
       public string Section_Screen_ResponseGroup_Response_Label { get; set; }
       public string Section_Screen_ResponseGroup_Response_Value { get; set; }
       public string Section_Screen_ResponseGroup_Response_Type { get; set; }
       public string SubmissionId { get; set; }

     //  public ICollection<Submission_Section2_ResourceGroupModel> submission_Section2_ResourceGroupModelsColl { get; set; }
    }
}
