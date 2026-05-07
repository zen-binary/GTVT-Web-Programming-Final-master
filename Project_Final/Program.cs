using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Project_Final.Context;
using Project_Final.Services;
using Project_Final.Services.Impl;
using Project_Final.Utils;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

//Logger config
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<DBContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("utc_shop")));
builder.Services.AddScoped<RedisCacheService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICardOrderService, CardOrderService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ISellService, SellService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "localhost:6379"; // Địa chỉ Redis server
    //options.InstanceName = "SampleInstance";  // Tên instance Redis (tuỳ chọn)
});

//security config
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuers = ["http://localhost:5092", "https://localhost:7055"],
            ValidAudiences = ["http://localhost:5092", "https://localhost:7055"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("utc-shop-final-security-secret-key"))
        };
    });
builder.Services.AddAuthorization();

builder.Services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

//register AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Cấu hình Redis Cache


// Config CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});


var app = builder.Build();
HttpContextHelper.Configure(app.Services);

// Sử dụng CORS
app.UseCors("AllowAll");

//app.UseMiddleware<ExceptionHandleMiddleware>();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Admin/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints => endpoints.MapControllers());

app.MapControllerRoute(
    name: "home",
    pattern: "Admin/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "overview",
    pattern: "Admin/{controller=Overview}/{action=Index}/{id?}");

// Route Category
app.MapControllerRoute(
    name: "category",
    pattern: "Admin/{controller=Category}/{action=Index}/{id?}");

// Route Product
app.MapControllerRoute(
    name: "product",
    pattern: "Admin/{controller=Product}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "createProduct",
    pattern: "Admin/{controller=Product}/Create");

app.MapControllerRoute(
    name: "editProduct",
    pattern: "Admin/{controller=Product}/Edit/{id}");

app.MapControllerRoute(
    name: "card",
    pattern: "Admin/{controller=Card}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "bill",
    pattern: "Admin/{controller=Bill}/{action=Index}/{id?}");


app.MapGet("/", () => Results.Redirect("/Admin/Overview"));


app.Run();
