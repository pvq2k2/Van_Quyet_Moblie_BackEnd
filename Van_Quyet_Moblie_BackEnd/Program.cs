using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Text;
using System.Text.Json.Serialization;
using Van_Quyet_Moblie_BackEnd.Handle.DTOs;
using Van_Quyet_Moblie_BackEnd.Handle.Response;
using Van_Quyet_Moblie_BackEnd.Helpers;
using Van_Quyet_Moblie_BackEnd.Services.Implement;
using Van_Quyet_Moblie_BackEnd.Services.Interface;

var builder = WebApplication.CreateBuilder(args);
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standard Authorization header using the Bearer scheme",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer"
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();
});
builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          //policy.WithOrigins("http://localhost:5173/").AllowAnyMethod().AllowAnyHeader();
                          policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                      });
});
//builder.Services.AddDbContext<AppDbContext>(options =>
//{
//    options.UseSqlServer(builder.Configuration.GetSection("AppSettings:MyDB").Value);
//});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(builder.Configuration.GetSection("AppSettings:AccessTokenSecret").Value!)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

builder.Services.AddTransient<IAuthService, AuthService>();
builder.Services.AddTransient<IProductTypeService, ProductTypeService>();
builder.Services.AddTransient<IProductService, ProductService>();
builder.Services.AddTransient<IDecentralizationService, DecentralizationService>();
builder.Services.AddTransient<IProductImageService, ProductImageService>();
builder.Services.AddTransient<IProductReviewService, ProductReviewService>();
builder.Services.AddTransient<ICartService, CartService>();
builder.Services.AddTransient<IOrderService, OrderService>();
builder.Services.AddTransient<IVoucherService, VoucherService>();
builder.Services.AddTransient<ISlidesService, SlidesService>();
builder.Services.AddTransient<TokenHelper>();
builder.Services.AddTransient<GHNHelper>();


builder.Services.AddSingleton<ResponseObject<AccountDTO>>();
builder.Services.AddSingleton<ResponseObject<TokenDTO>>();
builder.Services.AddSingleton<ResponseObject<ProductImageDTO>>();
builder.Services.AddSingleton<ResponseObject<ProductTypeDTO>>();
builder.Services.AddSingleton<ResponseObject<ProductDTO>>();
builder.Services.AddSingleton<ResponseObject<ProductReviewDTO>>();
builder.Services.AddSingleton<ResponseObject<DecentralizationDTO>>();
builder.Services.AddSingleton<ResponseObject<CartDTO>>();
builder.Services.AddSingleton<ResponseObject<OrderDTO>>();
builder.Services.AddSingleton<ResponseObject<VoucherDTO>>();
builder.Services.AddSingleton<ResponseObject<SlidesDTO>>();

builder.Services.AddSingleton<CloudinaryHelper>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.WriteIndented = true;
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(MyAllowSpecificOrigins);

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
