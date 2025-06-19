using System.Text;
using System.Text.Json.Serialization;
using Application;
using Domain.Permissions;
using Infrastructure.Data;
using log4net.Config;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using WebApi.Common.Filters;
using WebApi.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Đăng ký DbContext sử dụng SQL Server và đọc chuỗi kết nối từ appsettings.json
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetSection("ConnectionString").Value));

// Đăng ký provider log ra console
builder.Logging.AddConsole();

// Cấu hình CORS để cho phép frontend (localhost:3000) gọi tới API này
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder => builder
            .WithOrigins("http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials());
});

// Cấu hình AutoMapper, chỉ định assembly chứa profile để quét
builder.Services.AddAutoMapper(typeof(ApplicationModule).Assembly);

// Cấu hình log4net, đọc file cấu hình log4net.config
var log4netConfigFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log4net.config");
XmlConfigurator.Configure(new FileInfo(log4netConfigFile));

// Tắt các logging provider mặc định và chỉ dùng log4net
builder.Logging.ClearProviders();
builder.Logging.AddLog4Net();

// Cấu hình Swagger để tạo tài liệu API và hỗ trợ JWT trong Swagger UI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standard Authorization header using the Bearer scheme",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer"
    });

    // Cho phép Swagger hiển thị ô nhập token
    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

// Cấu hình JSON serialization để tránh vòng lặp trong quan hệ entity
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.WriteIndented = true;
});

// Cấu hình xác thực JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    var key = Encoding.UTF8.GetBytes(builder.Configuration["JWT:AccessTokenSecret"]!);

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
        ValidAudience = builder.Configuration["JWT:ValidAudience"],
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ClockSkew = TimeSpan.Zero // Không delay thêm khi token gần hết hạn
    };

    options.Events = new JwtBearerEvents
    {
        OnChallenge = context =>
        {
            context.HandleResponse(); // Ngăn JWT middleware trả lỗi mặc định
            throw new UnauthorizedAccessException("Token không được cung cấp hoặc không hợp lệ.");
        },
        OnAuthenticationFailed = context =>
        {
            throw new UnauthorizedAccessException("Xác thực token thất bại. Có thể token đã hết hạn.");
        }
    };
});


// Gọi extension method để đăng ký các service từ tầng Application
builder.Services.AddApplicationServices();

// Cấu hình phân quyền theo claim, tạo policy tương ứng với mỗi permission
builder.Services.AddAuthorization(options =>
{
    foreach (var permission in AppPermissions.All)
    {
        options.AddPolicy(permission, policy =>
            policy.RequireClaim("Permission", permission));
    }
});

// Đăng ký các filter toàn cục, bao gồm ValidateModelStateFilter để tự động kiểm tra ModelState
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidateModelStateFilter>();
});


var app = builder.Build();

// Seed dữ liệu khởi tạo (Permission, Role, Admin user)
using (var scope = app.Services.CreateScope())
{
    await DbSeeder.SeedAsync(scope.ServiceProvider);
}

// Chỉ bật Swagger khi đang ở môi trường Development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Kích hoạt CORS theo policy đã cấu hình
app.UseCors("AllowSpecificOrigin");

// Middleware xử lý exception cho API
app.UseMiddleware<ApiExceptionMiddleware>();

// Tự động chuyển hướng HTTP sang HTTPS
app.UseHttpsRedirection();

// Kích hoạt xác thực và ủy quyền
app.UseAuthentication();
app.UseAuthorization();

// Ánh xạ controller với route
app.MapControllers();

// Chạy ứng dụng
app.Run();
