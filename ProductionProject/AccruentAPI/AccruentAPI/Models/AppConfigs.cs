using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;

namespace AccruentAPI.Models
{
    public class AppConfigs
    {
        private string PathToConfigFolder { get; set; }
        public string ConnectionString { get; set; }
        public string BaseAddressUrl { get; set; }
        public string AuthenticationKey { get; set; }
        public string AttachmentLocation { get; set; }
        public string PortalLinkUrl { get; set; }
        
        public WebApiUrl WebApiUrl { get; private set; }
        
        public EmailConfigs EmailConfigs { get; private set; }
        
            
        public AppConfigs()
        {
            PathToConfigFolder = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,ConfigurationManager.AppSettings["PathToConfigFolder"]));


            var configJsonPath = File.ReadAllText(Path.Combine(PathToConfigFolder, "appsettings.json"));
            var jsonLoader = JsonConvert.DeserializeObject<JsonLoader>(configJsonPath);

            ConnectionString = jsonLoader.ConnectionString;
            AuthenticationKey = jsonLoader.AuthenticationKey;
            AttachmentLocation = jsonLoader.AttachmentLocation;
            PortalLinkUrl = jsonLoader.PortalLinkUrl;
            BaseAddressUrl = jsonLoader.BaseAddressUrl;
            WebApiUrl = jsonLoader.WebApiUrl.GetFullPath();

            EmailConfigs = jsonLoader.EmailConfigs;
        }

    }
}