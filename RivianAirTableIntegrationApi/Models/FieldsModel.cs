using System.Runtime.Intrinsics.X86;
using System.Text.Json.Serialization;

namespace RivianAirtableIntegrationApi.Models
{
    public class FieldsModel
    {
        public string id { get; set; }
        public string project_type { get; set; }
        public string project_sub_type { get; set; }
        public string fos_project_name { get; set; }
        public string project_decription { get; set; }
        public string status { get; set; }
        public string square_footage { get; set; }
        public string address1 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public string zipcode { get; set; }
        public string build_type { get; set; }
        public string planned_go_live_date { get; set; }
        public string schedule_update_date { get; set; }
        public string gate1_date { get; set; }
        public string OAC_Kickoff { get; set; }
        public string gate3_date { get; set; }
        public string gate4_date { get; set; }
        public string gate5_date { get; set; }
        public string gate6_date { get; set; }
        public string rivian_construction_start_date { get; set; }
        public string latest_note_design { get; set; }
        public string prev_note_design { get; set; }
        public string latest_note_construnction { get; set; }
        public string prev_note_construnction { get; set; }
        public string latest_note_real_estate { get; set; }
        public string prev_note_real_estate { get; set; }
        public string latest_note_operations { get; set; }
        public string prev_note_operations { get; set; }

        public string SSIProjectTeam { get; set; }
        public string RivianPM { get; set; }
        public string RivianDM { get; set; }
        public string fos_url_id { get; set; }
        public string Quarter { get; set; }
        public string Year { get; set; }

       // [JsonPropertyName("Ancillary Install Date")]
        public string Ancillary_Install_Date { get; set; }
        public string Workstation_Install_Date { get; set; }
        public string Email_fromSSIProjectTeam { get; set; }
    }
}
