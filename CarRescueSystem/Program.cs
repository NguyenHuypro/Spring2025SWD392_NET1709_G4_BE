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
using VNPAY.NET;

var builder = WebApplication.CreateBuilder(args);

// Thêm CORS vào services
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policy =>
        {
            policy.WithOrigins("http://localhost:3000", "http://localhost:5210", "http://localhost:5173")
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials(); // 🔥 Cho phép gửi cookies và headers xác thực
        });
});


// Setup SQL Server Database
//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")),
//    ServiceLifetime.Scoped
//);

//Setup MySQL Server Database
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString,
        ServerVersion.AutoDetect(connectionString), // Tự động nhận diện phiên bản
        b => b.MigrationsAssembly("CarRescueSystem.DAL")
    )
);




Console.WriteLine("Current Connection String: " + builder.Configuration.GetConnectionString("DefaultConnection"));

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

builder.Services.AddScoped<IVehicleService, VehicleService>();
builder.Services.AddScoped<IDashboardService,DashboardService>();
builder.Services.AddScoped<IRescueStationService, RescueStationService>();
builder.Services.AddScoped<IStaffService, StaffService>();
builder.Services.AddScoped<IPackageService, PackageService>();
//builder.Services.AddScoped<IWalletService, WalletService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<IScheduleService,  ScheduleService>();
builder.Services.AddHttpClient<IOsmService, OsmService>();
builder.Services.AddScoped<IVnPayService, VnPayService>();

builder.Services.AddSingleton<IVnpay>(provider =>
{
    var config = provider.GetRequiredService<IConfiguration>();
    var vnpay = new Vnpay();
    vnpay.Initialize(
        config["VNPAY:TmnCode"],
        config["VNPAY:HashSecret"],
        config["VNPAY:Url"], // 🔥 Sửa "BaseUrl" -> "Url"
        config["VNPAY:ReturnUrl"] // 🔥 Đảm bảo đúng key
    );
    return vnpay;
});




//builder.Services.AddScoped<IApplicationDbContext, ApplicationDbContext>();

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
