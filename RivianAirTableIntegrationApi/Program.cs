using Microsoft.EntityFrameworkCore;
using RivianAirtableIntegrationApi.DBContext;
using RivianAirtableIntegrationApi.Repository;
using RivianAirtableIntegrationApi.Repository.Interfaces;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<SSIDBContext>(o => o.UseSqlServer(configuration.GetConnectionString("SSIDB")));
builder.Services.AddTransient<IRequestsRepository, RequestsRepository>();
builder.Services.AddTransient<IEmailNotificationRepository, EmailNotificationRepository>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Logging.AddLog4Net("log4net.config", true);

builder.Services.AddSwaggerGen();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
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
