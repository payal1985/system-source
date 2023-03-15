using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.IO;

namespace SSInventory
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
               .ConfigureWebHostDefaults(webBuilder =>
               {
                   webBuilder
                   .ConfigureKestrel(options =>
                   {
                       options.Limits.MaxRequestBodySize = long.MaxValue;
                   })
                   .UseWebRoot(Directory.GetCurrentDirectory())
                   .UseIISIntegration()
                   .UseStartup<Startup>();
               }).ConfigureLogging(builder =>
               {
                   builder.SetMinimumLevel(LogLevel.Information);
                   builder.AddLog4Net("log4net.config");
               });
    }
}
