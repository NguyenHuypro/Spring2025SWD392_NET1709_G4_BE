using CarRescueSystem.BLL.Service.Implement;
using CarRescueSystem.BLL.Service.Interface;
using CarRescueSystem.BLL.Utilities;
using CarRescueSystem.DAL;
using CarRescueSystem.DAL.Data;
using CarRescueSystem.DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Thêm CORS vào services
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policy =>
        {
            policy.WithOrigins("http://localhost:3000") // URL React chạy trên cổng 3000
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

// Setup SQL Server Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Add các dịch vụ
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<UserUtility>();

// Đăng ký DbSeeder vào DI container
builder.Services.AddScoped<DbSeeder>();

// Thêm IHttpContextAccessor
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

var app = builder.Build();


// Sử dụng CORS
app.UseCors("AllowReactApp");

// Bật Swagger
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Car Rescue API V1");
    c.RoutePrefix = "swagger"; // Truy cập trực tiếp tại http://localhost:5210/
});
app.UseRouting();
app.UseHttpsRedirection();
app.MapControllers();
app.Run();
