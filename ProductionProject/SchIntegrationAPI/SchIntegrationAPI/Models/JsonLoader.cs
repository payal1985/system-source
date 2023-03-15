using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace SchIntegrationAPI.Models
{
    [JsonObject(ItemRequired = Required.Always)]
    public class JsonLoader
    {
        [JsonProperty]
        public string ConnectionString { get; private set; }
        [JsonProperty]
        public string BaseAddressUrl { get; set; }

        [JsonProperty]
        public string AuthenticationKey { get; set; }
        [JsonProperty]
        public string AttachmentLocation { get; set; }

        [JsonProperty]
        public string PortalLinkUrl { get; set; }
        
        [JsonProperty]
        public WebApiUrl WebApiUrl { get; private set; }

        [JsonProperty]
        public EmailConfigs EmailConfigs { get; private set; }
    }
}