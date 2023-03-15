using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RivianAirtableIntegrationApi.DBModels
{
    [Table("requests")]
    public class Requests
    {
        [Key]
        public int request_id { get; set; }
        public int client_id { get; set; }
        //public string? frequest_id1 { get; set; }
        //public string? frequest_id2 { get; set; }
        //public int? location { get; set; }
        public int request_status_id { get; set; }
        public int sub_status { get; set; }
        public int assignee { get; set; }
        public int? ssi_manager { get; set; }
        //public int? foreign_request_type { get; set; }
        public int? request_type { get; set; }
        //public int? request_priority { get; set; }
        public string subject { get; set; }
        public string? description { get; set; }
        //public string? scope { get; set; }
        public string? eu_name { get; set; }
        //public string? eu_phone { get; set; }
        // public string? eu_email { get; set; }
        // public string? euc_name { get; set; }
        //public string? euc_phone { get; set; }
        //public string? euc_email { get; set; }
        //public string? eur_name { get; set; }
        //public string? eur_phone { get; set; }
        //public string? eur_email { get; set; }
       // [Column(TypeName = "datetime")]
        public DateTime? requested_due_date { get; set; }
        public int createid { get; set; }
       // [Column(TypeName = "datetime")]
        public DateTime createdate { get; set; }
        public int createprocess { get; set; }
        public int updateid { get; set; }
       // [Column(TypeName = "datetime")]
        public DateTime updatedate { get; set; }
        public int updateprocess { get; set; }
       // [Column(TypeName = "datetime")]
        public DateTime? request_date { get; set; }
        public bool bid_in_process { get; set; }
        //public string? eu_note { get; set; }
        //public string? euc_note { get; set; }
        //public string? eur_note { get; set; }
        //public string? frequest_id3 { get; set; }
        //public decimal? rom_amt { get; set; }
        //public decimal? bid_amt { get; set; }
        //public DateTime? completed_date { get; set; }
        public int late_code { get; set; }=0;
        //public string? late_reason { get; set; }
        public string? frequest_id4 { get; set; }
        //public string? frequest_id5 { get; set; }
        public string? frequest_id6 { get; set; }
        //public string? frequest_id7 { get; set; }
        //public string? frequest_id8 { get; set; }
        //public string? frequest_id9 { get; set; }
        public string? eup_name { get; set; }
        //public string? eup_phone { get; set; }
        //public string? eup_email { get; set; }
        //public string? eup_note { get; set; }
        //public DateTime? scheduled_date { get; set; }
        public bool hasPunchListItems { get; set; } = false;
       // [Column(TypeName = "datetime")]
        public DateTime? romDateDue { get; set; }
        //public DateTime? romDateComp { get; set; }
        //public DateTime? bidDateDue { get; set; }
        //public DateTime? bidDateComp { get; set; }
        //public DateTime? proposalDateDue { get; set; }
        //public DateTime? proposalDateComp { get; set; }
        //[Column(TypeName = "datetime")]
        public DateTime? installStartDateDue { get; set; }
        //public DateTime? installStartDateComp { get; set; }
        //public DateTime? installCompleteDateDue { get; set; }
        //public DateTime? installCompleteDateComp { get; set; }
        //public DateTime? punchlistDateDue { get; set; }
        //public DateTime? punchlistDateComp { get; set; }
        public int? client_branch_id { get; set; }
        //public string? frequest_id10 { get; set; }
        //public string? frequest_id11 { get; set; }
        //public string? frequest_id12 { get; set; }
        //public string? frequest_id13 { get; set; }
        //public string? frequest_id14 { get; set; }
        //public string? frequest_id15 { get; set; }
        //public string? frequest_id16 { get; set; }
        //public string? frequest_id17 { get; set; }
        //public string? frequest_id18 { get; set; }
        //public string? frequest_id19 { get; set; }
        //public DateTime? moveStartDateDue { get; set; }
        //public DateTime? moveStartDateComp { get; set; }
       // [Column(TypeName = "datetime")]
        public DateTime? occupancyDateDue { get; set; }
        //public DateTime? occupancyDateComp { get; set; }
        //public DateTime? releaseToInvoicingDate { get; set; }
        //public int? estimatedPMHours { get; set; }
        //public DateTime? layoutDateDue { get; set; }
        //public DateTime? layoutDateComp { get; set; }
        //public DateTime? designSpecDateDue { get; set; }
        //public DateTime? designSpecDateComp { get; set; }
        //public DateTime? elecDrawingDateDue { get; set; }
        //public DateTime? elecDrawingDateComp { get; set; }
        //public DateTime? installCadDrawingDateDue { get; set; }
        //public DateTime? installCadDrawingDateComp { get; set; }
        //public string? custFaceComments { get; set; }
        //public DateTime? poOrderDateDue { get; set; }
        //public DateTime? poOrderDateComp { get; set; }
        //public DateTime? clientPoRcvdDateDue { get; set; }
        //public DateTime? clientPoRcvdDateComp { get; set; }
        //public Byte? forecastProability { get; set; }
        //public decimal? forecastAmount { get; set; }
       // [Column(TypeName = "datetime")]
       // public DateTime? progressPaymentDateDue { get; set; }
        //public DateTime? progressPaymentDateComp { get; set; }
        public bool fromStandardsSite { get; set; } =false;
        //public DateTime? nteDateDue { get; set; }
        //public DateTime? nteDateComp { get; set; }
        //public DateTime? dealSummDateDue { get; set; }
        //public DateTime? dealSummDateComp { get; set; }
       // [Column(TypeName = "datetime")]
        public DateTime? installStartDate2Due { get; set; }
        //public DateTime? installStartDate2Comp { get; set; }
        //public DateTime? installStartDate3Due { get; set; }
        //public DateTime? installStartDate3Comp { get; set; }
        public bool isHotList { get; set; } = false;    
        //public DateTime? reviewDateDue { get; set; }
        //public DateTime? reviewDateComp { get; set; }
        //public DateTime? projBankDue { get; set; }
        //public DateTime? projBankComp { get; set; }
        //public int? designer { get; set; }

    }
}
