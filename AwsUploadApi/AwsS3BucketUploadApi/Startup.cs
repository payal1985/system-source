using AwsS3BucketUploadApi.DBContext;
using AwsS3BucketUploadApi.Mappers;
using AwsS3BucketUploadApi.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AwsS3BucketUploadApi
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
            services.AddDbContext<AwsInfoContext>(o => o.UseSqlServer(Configuration.GetConnectionString("SsiDB")));

            services.AddControllers();
            //services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("v1", new OpenApiInfo { Title = "AwsS3BucketUploadApi", Version = "v1" });
            //});

            services.AddSingleton<AwsFeatures>();

            RepositoryMapper.Register(services);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            var virdir = Configuration.GetSection("virdir").Value;

            if (env.IsDevelopment() || env.IsProduction())
            {
                app.UseDeveloperExceptionPage();
                //app.UseSwagger();
                //app.UseSwaggerUI(c => c.SwaggerEndpoint(virdir + "/swagger/v1/swagger.json", "AwsS3BucketUploadApi v1"));
                //////app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AwsS3BucketUploadApi v1"));
            }

            // Register Log4Net
            loggerFactory.AddLog4Net("log4net.config", true);

            app.UseCors(x => x
             .AllowAnyOrigin()
             .AllowAnyMethod()
             .AllowAnyHeader());

            app.UseCustomMiddleware();
            
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
