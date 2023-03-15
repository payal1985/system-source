using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchIntegrationAPI.Models
{
    public class EmailConfigs
    {
        [JsonProperty]
        public string FromMail { get; private set; }

        [JsonProperty]
        public string ToMail { get; private set; }

        [JsonProperty]
        public string ToMailBusiness { get; private set; }        

        [JsonProperty]
        public string Host { get; private set; }

        [JsonProperty]
        public int Port { get; private set; }

        [JsonProperty]
        public string DisplayName { get; private set; }

        [JsonProperty]
        public string Subject { get; private set; }

        [JsonProperty]
        public string Application { get; private set; }

    }
}