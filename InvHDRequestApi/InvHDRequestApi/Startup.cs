using InvHDRequestApi.DBContext;
using InvHDRequestApi.Mappers;
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
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace InvHDRequestApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            LogManager.LoadConfiguration(String.Concat(Directory.GetCurrentDirectory(), "/NLog.config"));
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();

            //services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("v1", new OpenApiInfo { Title = "InvHDRequestApi", Version = "v1" });
            //});

            services.AddDbContext<InventoryContext>(o => o.UseSqlServer(Configuration.GetConnectionString("InventoryDB")));
            services.AddDbContext<SSIDBContext>(o => o.UseSqlServer(Configuration.GetConnectionString("SSIDB")));

            SharedRepositoryMapper.Register(services);
            InventoryRepositoryMapper.Register(services);
            SSIDBRepositoryMapper.Register(services);

            //services.AddHttpClient<IAwsDownloadRepository, AwsDownloadRepository>(client =>
            //{
            //    client.BaseAddress = new Uri(Configuration.GetValue<string>("AwsDownloadUrl"));
            //});

            //services.AddHttpClient<IAwsUploadRepository, AwsUploadRepository>(client =>
            //{
            //    client.BaseAddress = new Uri(Configuration.GetValue<string>("AwsUploadUrl"));
            //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseSwagger();
                //app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "InvHDRequestApi v1"));
            }

            app.UseCors(x => x
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());

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
