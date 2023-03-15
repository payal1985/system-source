using GoCanvasAPI.DBContext;
using GoCanvasAPI.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoCanvasAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

             services.AddControllers();
           // services.AddControllersWithViews();
            services.AddDbContext<GoCanvasContext>(o => o.UseSqlServer(Configuration.GetConnectionString("MsGoCanvasDB")));
            //services.AddDbContext<GoCanvasContext>(o => o.UseSqlServer(Configuration.GetConnectionString("MsGoCanvasDB")),ServiceLifetime.Transient);
            services.AddTransient<IFormRepository, FormRepository>();
            services.AddTransient<ISubmissionRepository, SubmissionRepository>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseRouting();

            //app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                //endpoints.MapControllerRoute(
                //   name: "default",
                //   pattern: "{controller=Home}/{action=Index}/{id?}");
            });
     
        }
    }
}
