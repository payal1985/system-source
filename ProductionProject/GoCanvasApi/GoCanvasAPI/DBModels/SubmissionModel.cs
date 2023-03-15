using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GoCanvasAPI.DBModels
{
    [Table("SubmissionModel")]
    public class SubmissionModel
    {
       [Key]
       public Guid SubmissionsData_table_id { get; set; }
       public string SubmissionId { get; set; }
       public string FormId { get; set; }
       public string FormName { get; set; }        
       public DateTime Date { get; set; }
       public DateTime DeviceDate { get; set; }
       public string UserName { get; set; }
       public string FirstName { get; set; }
       public string LastName { get; set; }
       public string ResponseID { get; set; }
       public string WebAccessToken { get; set; }
       public string No { get; set; }
       public string SubmissionNumber { get; set; }
       public string SubFormSatus { get; set; }
       public string SubFormVersion { get; set; }

       // public string CurrentSubmissionNum { get; set; }
        //public ICollection<Submission_Section1Model> submission_Section1Model { get; set; }
        //public ICollection<Submission_Section2Model> submission_Section2Model { get; set; }
        //public ICollection<Submission_Section2_ResourceGroupModel> Submission_Section2_ResourceGroupModel { get; set; }

        //public ICollection<ImageDataModel> ImageDataModel { get; set; }
    }
}
