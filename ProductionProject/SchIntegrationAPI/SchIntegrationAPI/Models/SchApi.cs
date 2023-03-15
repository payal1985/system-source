using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchIntegrationAPI.Models
{
    public class SchApi
    {
        public string number { get; set; }
        public CtvScCallerModel CtvScCallerModel { get; set; }
        public string priority { get; set; }
        public string state { get; set; }
        public string x_ctv_sc_category { get; set; }
        public string u_building { get; set; }

        public UFloorModel UFloorModel { get; set; }
        public URoomModel URoomModel { get; set; }

        public string x_ctv_sc_location_description { get; set; }
        public string due_date { get; set; }
        public string short_description { get; set; }
        public string description { get; set; }

        public string comments { get; set; }

        public List<CommentModel> CommentModels { get; set; }
        public string work_notes { get; set; }

        public string close_notes { get; set; }

        public string x_ctv_sc_close_code { get; set; }

        public string sys_id { get; set; }

        public string sys_created_by { get; set; }

        public string sys_created_on { get; set; }

        public string commentjson { get; set; }
    }

    public class CtvScCallerModel
    {
        public string name { get; set; }
        public string email { get; set; }
        public string title { get; set; }
        public string phone { get; set; }
    }

    public class UFloorModel
    {
        public string ualtfullname { get; set; }
    }
    public class URoomModel
    {
        public string ualtfullname { get; set; }
    }

    public class CommentModel
    {
        public string element { get; set; }
        public string element_id { get; set; }
        public string name { get; set; }
        public string sys_created_by { get; set; }
        public string sys_created_on { get; set; }
        public string sys_id { get; set; }
        public string sys_tags { get; set; }
        public string value { get; set; }

    }

    public class AttachmentModel
    {
      public string size_bytes { get; set; }
      public string file_name           { get; set; }
      public string sys_mod_count       { get; set; }
      public string average_image_color { get; set; }
      public string image_width         { get; set; }
      public string sys_updated_on      { get; set; }
      public string sys_tags            { get; set; }
      public string table_name          { get; set; }
      public string sys_id              { get; set; }
      public string image_height        { get; set; }
      public string sys_updated_by      { get; set; }
      public string download_link       { get; set; }
      public string content_type        { get; set; }
      public string sys_created_on      { get; set; }
      public string size_compressed     { get; set; }
      public string compressed          { get; set; }
      public string state               { get; set; }
      public string table_sys_id        { get; set; }
      public string chunk_size_bytes    { get; set; }
      public string hash                { get; set; }
      public string sys_created_by { get; set; }
    }
}