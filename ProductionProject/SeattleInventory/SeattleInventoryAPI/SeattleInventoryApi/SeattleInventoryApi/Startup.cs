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
using SeattleInventoryApi.DBContext;
using SeattleInventoryApi.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SeattleInventoryApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            LogManager.LoadConfiguration(String.Concat(Directory.GetCurrentDirectory(), "/NLog.config"));
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        //public static void ConfigureLoggerService(this IServiceCollection services)
        //{
        //    services.AddSingleton<ILoggerManagerRepository, LoggerManagerRepository>();
        //}

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            //services.ConfigureLoggerService();
            services.AddControllers();

            services.AddSingleton<ILoggerManagerRepository, LoggerManagerRepository>();

            services.AddDbContext<InventoryContext>(o => o.UseSqlServer(Configuration.GetConnectionString("InventoryDB")));
            services.AddDbContext<SSIRequestContext>(o => o.UseSqlServer(Configuration.GetConnectionString("SsiDB")));

            services.AddTransient<IInventoryRepository, InventoryRepository>();
            services.AddTransient<IInventoryItemRepository, InventoryItemRepository>();
            services.AddTransient<IInventoryItemImageRepository, InventoryItemImageRepository>();
            services.AddTransient<IInventoryItemOrderRepository, InventoryItemOrderRepository>();
            services.AddTransient<IEmailNotificationRepository, EmailNotificationRepository>();
            services.AddTransient<IDonationInventoryRepository, DonationInventoryRepository>();
            services.AddTransient<IClientInventoryRepository, ClientInventoryRepository>();
            services.AddTransient<ILoginRepository, LoginRepository>();

            services.AddTransient<IHDTicketRepository, HDTicketRepository>();

            //services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("v1", new OpenApiInfo { Title = "SeattleInventoryApi", Version = "v1" });
            //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseSwagger();
                //app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SeattleInventoryApi v1"));
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
