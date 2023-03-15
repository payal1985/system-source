using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoCanvasAPI.ViewModel
{
    public class RootObject
    {
        [JsonProperty("forms")]
        public Forms Forms { get; set; }
    }
    public class Forms
    {
        [JsonProperty("form")] 
        public List<Form> Form { get; set; }
    }

    public class Form
    {
        
        [JsonProperty("id")]
        public string Id { get; set; }

     
        [JsonProperty("originatinglibrarytemplateid")]
        public string OriginatingLibraryTemplateId { get; set; }

       
        [JsonProperty("guid")]
        public string GUID { get; set; }

       
        [JsonProperty("name")]
        public Name Name { get; set; }


        [JsonProperty("status")]
        public Status Status { get; set; }


       
        [JsonProperty("version")]
        public Version Version { get; set; }

    }


 
    public class Name
    {
        [JsonProperty("text")]
        public string text { get; set; }
    }

    public class Status
    {
        [JsonProperty("text")]
        public string text { get; set; }
    }

    public class Version
    {
        [JsonProperty("text")]
        public string text { get; set; }
    }

}
