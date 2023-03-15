using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SSInventory.Business;
using SSInventory.Core.Helpers;
using SSInventory.Core.Models;
using SSInventory.Web.Models;
using SSInventory.Web.Services;
using System;
using System.Collections.Generic;
using System.Text;
using SSInventory.Core;
using Microsoft.AspNetCore.Http;
using System.Net;
using Microsoft.Extensions.FileProviders;
using System.IO;
using SSInventory.Web.Services.Aws;
using SSInventory.Web.Services.Email;
using SSInventory.Core.Repositories.Interfaces;
using SSInventory.Core.Repositories;
//using Hangfire;
//using Hangfire.SqlServer;

namespace SSInventory
{
    public class Startup
    {
        private readonly IWebHostEnvironment _env;
        public Startup(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            this.Configuration = builder.Build();

            _env = env;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Register Dependencies
            services.AddDbContext<SSInventoryDbContext>(options => options.UseSqlServer(@Configuration["ConnectionStrings:Default"]));
            services.AddAutoMapper(typeof(MappingHelper));

            services.AddCors();

            // Install Swashbuckle package first
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Files APIs",
                    Description = "SSInventory APIs"
                });
                var filePath = Path.Combine(System.AppContext.BaseDirectory, "SSInventory.Web.xml");
                options.IncludeXmlComments(filePath);
                options.AddSecurityDefinition(name: "Bearer", securityScheme: new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Description = "Enter the Bearer Authorization string as following: `Bearer Generated-JWT-Token`",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                            Reference = new OpenApiReference
                            {
                                Id = "Bearer",
                                Type = ReferenceType.SecurityScheme
                            }
                        },
                        new List<string>()
                    }
                });
            });

            // Config Jwt based authentication
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"])),
                    ClockSkew = TimeSpan.Zero
                };

            });

            services.AddControllers();

            services.AddRazorPages();

            // Register Dependency Injection
            RepositoryInstaller.Register(services);
            ServiceInstaller.Register(services);

            //services.AddHangfire(configuration => configuration
            //      .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
            //      .UseSimpleAssemblyNameTypeSerializer()
            //      .UseRecommendedSerializerSettings());
            //.UseSqlServerStorage(Configuration.GetConnectionString("Default"), new SqlServerStorageOptions
            //{
            //    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
            //    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
            //    QueuePollInterval = TimeSpan.Zero,
            //    UseRecommendedIsolationLevel = true,
            //    DisableGlobalLocks = true
            //}));
            //services.AddHangfireServer();


            services.Configure<CleanupAppSettings>(Configuration.GetSection("ssSettings"));

            // register hosted services
            services.AddHostedService<CleanUpFileService>();

            //services.AddScoped<IAwsDownloadRepository, AwsDownloadRepository>();
            //services.AddScoped<IAwsUploadRepository, AwsUploadRepository>();

            services.AddHttpClient<IAwsS3FileRepository, AwsS3FileRepository>("AwsS3FileRepository", client =>
            {
                client.BaseAddress = new Uri(Configuration["ExternalSystem:S3ServiceDomain"]);
            });

            //services.AddTransient<Aws3Service>();
            services.AddTransient<MailService>();
            services.AddTransient<NetMailSender>();
            services.AddTransient<FileService>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseStaticFiles();

            // Register Log4Net
            loggerFactory.AddLog4Net("log4net.config", true);

            // Enable swagger middleware
            app.UseSwagger(options =>
            {
                options.SerializeAsV2 = true;
            });
            app.UseSwaggerUI((options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                options.RoutePrefix = string.Empty;
            }));

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            var folder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Uploads")),
                RequestPath = new PathString("/Uploads")
            });

            app.UseRouting();

            app.Use(async (context, next) =>
            {
                await next();
                if (context.Response.StatusCode == (int)HttpStatusCode.Unauthorized)
                {
                    var isRefreshToken = context.Request.Headers["refreshToken"].ToString();
                    if (string.IsNullOrWhiteSpace(isRefreshToken))
                    {
                        var tokenHeader = context.Request.Headers["Authorization"].ToString();
                        if (!string.IsNullOrEmpty(tokenHeader))
                        {
                            await context.Response.WriteAsync("InvalidToken");
                        }
                        else
                        {
                            await context.Response.WriteAsync("Anauthorized");
                        }
                    }
                }
            });

            app.UseCors(x => x
                .SetIsOriginAllowed(origin => true)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());

            //app.UseHangfireServer();
            //app.UseHangfireDashboard("/hangfire");

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
            });
        }
    }
}
