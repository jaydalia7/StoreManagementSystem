using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;
using StoreManagementSystem.Configurations;
using StoreManagementSystem.Data;
using StoreManagementSystem.Mappings;
using StoreManagementSystem.Repository;
using StoreManagementSystem.Repository.IRepository;
using StoreManagementSystem.Services;
using StoreManagementSystem.Services.IServices;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Host.UseSerilog((context,configuration) => configuration.ReadFrom.Configuration(context.Configuration));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IHelperService, HelperService>();
builder.Services.AddTransient<IProductService, ProductService>();
builder.Services.AddTransient<IProductRepository, ProductRepository>();
builder.Services.AddTransient<IPurchaseProductService, PurchaseProductService>();
builder.Services.AddTransient<IPurchaseProductRepository, PurchaseProductRepository>();
builder.Services.AddTransient<ISellProductService, SellProductService>();
builder.Services.AddTransient<ISellProductRepository, SellProductRepository>();
builder.Services.AddAutoMapper(typeof(StoreManagementMapping));
builder.Services.AddHttpContextAccessor();

builder.Services.JWTConfiguration(builder.Configuration);

builder.Services.AddSwaggerConfiguration(builder.Configuration, builder.Environment);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
