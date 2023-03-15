using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace AccruentAPI.Models
{
    [JsonObject(ItemRequired = Required.Always)]
    public class WebApiUrl
    {
        [JsonProperty]

        public string GetRequests { get; set; }
        [JsonProperty]
        public string GetComments { get; set; }
        [JsonProperty]
        public string PostRequests { get; set; }

        [JsonProperty]
        public string GetFileRequests { get; set; }

        [JsonProperty]
        public string PostFileRequests { get; set; }
        public WebApiUrl GetFullPath()
        {
            foreach (PropertyInfo p in this.GetType().GetProperties())
            {
                var dirPath = ((string)p.GetValue(this));
                //if (Directory.Exists(dirPath))
                    p.SetValue(this, dirPath);
                //else
                //    throw new DirectoryNotFoundException($"directory not found: {dirPath}");
            }

            return this;
        }
    }
}