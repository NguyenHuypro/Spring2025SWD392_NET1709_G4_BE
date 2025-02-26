using CarRescueSystem.BLL.Service.Implement;
using CarRescueSystem.BLL.Service.Interface;
using CarRescueSystem.BLL.Utilities;
using CarRescueSystem.Common.Settings;
using CarRescueSystem.DAL;
using CarRescueSystem.DAL.Data;
using CarRescueSystem.DAL.Repository;
using CarRescueSystem.DAL.Repository.Implement;
using CarRescueSystem.DAL.Repository.Interface;
using CarRescueSystem.DAL.UnitOfWork;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Thêm CORS vào services
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policy =>
        {
            policy.WithOrigins("http://localhost:3000","http://localhost:5210") // URL React chạy trên cổng 3000 , 5210 chay local
                  .AllowAnyHeader()
                  .AllowAnyMethod();
                  
        });
});

// Setup SQL Server Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")
    ));

// Cấu hình Authentication với JWT
var secretKey = Encoding.UTF8.GetBytes(JwtSettingModel.SecretKey);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(secretKey),
            ValidateIssuer = true,
            ValidIssuer = JwtSettingModel.Issuer,
            ValidateAudience = true,
            ValidAudience = JwtSettingModel.Audience,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });

// Thêm Authorization dựa trên Role
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("UserOnly", policy => policy.RequireRole("User"));
});

// Add Swagger với JWT Authentication
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Car Rescue API", Version = "v1" });

    // Thêm nút "Authorize" trong Swagger UI để nhập JWT Token
    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Nhập token theo định dạng: Bearer {your JWT token}",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
        }
    };

    options.AddSecurityDefinition("Bearer", securityScheme);
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { securityScheme, new string[] { } }
    });
});

// Thêm Global Authorization (Tất cả API yêu cầu đăng nhập, trừ khi được đánh dấu [AllowAnonymous])
builder.Services.AddControllers(options =>
{
    var policy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
    options.Filters.Add(new AuthorizeFilter(policy));
});

// Đăng ký dịch vụ (DI Container)
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<IServiceRescueService, ServiceRescueService>();
builder.Services.AddScoped<IVehicleRepository, VehicleRepository>();
builder.Services.AddScoped<IVehicleService, VehicleService>();
builder.Services.AddScoped<UserUtility>();
builder.Services.AddAutoMapper(typeof(VehicleProfile));
builder.Services.AddScoped<DbSeeder>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();



var app = builder.Build();

// Sử dụng CORS
app.UseCors("AllowReactApp");

app.UseRouting();

// Bật Authentication và Authorization
app.UseAuthentication();
app.UseAuthorization();

// Bật Swagger
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Car Rescue API V1");
    c.RoutePrefix = "swagger"; // Truy cập trực tiếp tại http://localhost:5210/swagger
});


app.UseHttpsRedirection();
app.MapControllers();




app.Run(); 
