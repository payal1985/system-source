using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace SchIntegrationAPI
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        public static Logger logger = LogManager.GetCurrentClassLogger();
        protected void Application_Start()
        {

            logger.Info("Application Start");

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            //BundleConfig.RegisterBundles(BundleTable.Bundles);

            GlobalConfiguration.Configuration.Formatters.JsonFormatter.MediaTypeMappings.Add(new QueryStringMapping("json", "true", "application/json"));

        }

        protected void Application_Error(Object sender, EventArgs e)
        {
            var exception = Server.GetLastError();
            if (exception == null)
                return;

            string Message = exception.Message + " | " + exception.StackTrace;
            //var logger = LogManager.GetCurrentClassLogger();

            logger.Info(Message);

            Server.ClearError();
        }
    }
}
