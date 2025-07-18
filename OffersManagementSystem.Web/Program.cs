using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OffersManagementSystem.Application.IData;
using OffersManagementSystem.Application.IRepositories;
using OffersManagementSystem.Infrastructure.Repositories;
using OffersManagementSystem.Infrastructure.Data;
using OffersManagementSystem.Infrastructure.Identity;
using OffersManagementSystem.Infrastructure.Services;
using OffersManagementSystem.Application.IServices;
using OffersManagementSystem.Application.Settings;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using OffersManagementSystem.Infrastructure.Identity.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
    );

// Add Identity services
builder.Services.AddIdentity<AppIdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// Add Dapper
builder.Services.AddScoped(typeof(IAppDbDapper<>), typeof(AppDbDapper<>));
//builder.Services.AddScoped(typeof(IAppDbDapper<>), sp =>
//{
//    var connStr = builder.Configuration.GetConnectionString("DefaultConnection");
//    return ActivatorUtilities.CreateInstance(sp, typeof(AppDbDapper<>), connStr ?? "");
//});

// Get JWT Settings from appsettings.json
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
builder.Services.Configure<JwtSettings>(jwtSettings);

var key = Encoding.UTF8.GetBytes(jwtSettings["Key"] ?? "");

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
        ValidIssuer = jwtSettings["Issuer"],
        ValidateAudience = true,
        ValidAudience = jwtSettings["Audience"],
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ClockSkew = TimeSpan.Zero
    };
});

//Add Repositories
builder.Services.AddScoped<IOfferRepository, OfferRepository>();

// Add Services
builder.Services.AddScoped<IOfferService, OfferService>();
builder.Services.AddScoped<ITokenService, TokenService>();




// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
