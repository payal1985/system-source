using AwsS3Download.DBContext;
using AwsS3Download.Repository;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;


var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<AwsInfoContext>(o => o.UseSqlServer(configuration.GetConnectionString("SSIDB")));
builder.Services.AddTransient<IDownloadRepository, DownloadRepository>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

builder.Logging.AddLog4Net("log4net.config",true);
//builder.Services.Configure<FormOptions>(x => x.ValueCountLimit = 1000000);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
  // app.UseSwagger();
   // app.UseSwaggerUI();
}

app.UseCors(x => x
           .AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader()
           );

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
