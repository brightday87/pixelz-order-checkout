using Microsoft.EntityFrameworkCore;
using Pixelz.OrderCheckout.Api.Models;
using Serilog;
using Pixelz.OrderCheckout.Api.Bussiness.Interface;
using Pixelz.OrderCheckout.Api.Bussiness.Implement;
using Pixelz.OrderCheckout.Api.Configurations;
using Microsoft.AspNetCore.Identity.UI.Services;
using Pixelz.OrderCheckout.Api.Bussiness;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//config SeriLog
Log.Logger = new LoggerConfiguration()
        .ReadFrom.Configuration(builder.Configuration, sectionName: "Serilog")
        .Enrich.FromLogContext()
        .Enrich.WithEnvironmentName()
        .Enrich.WithMachineName()
        .Enrich.WithEnvironmentUserName()
        .Enrich.WithClientIp()
        .Enrich.WithProcessId()
        .Enrich.WithProcessName()
        .Enrich.WithThreadId()
        .Enrich.WithThreadName()
        .CreateLogger();

// Đăng ký DbContext
builder.Services.AddDbContext<PixelzDbContext>(options =>
{
    var cnnString = builder.Configuration.GetConnectionString("PixelzDbConnection");
    options.UseSqlServer(cnnString);
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
});

//Đăng ký Custom Settings
builder.Services.Configure<ApiConfiguration>(
    builder.Configuration.GetSection("ApiConfiguration")
);

// Đăng ký tầng Bussiness
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IProductionService, ProductionService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();