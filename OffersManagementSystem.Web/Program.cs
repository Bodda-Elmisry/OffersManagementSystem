using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OffersManagementSystem.Application.IData;
using OffersManagementSystem.Application.IRepositories;
using OffersManagementSystem.Infrastructure.Repositories;
using OffersManagementSystem.Infrastructure.Data;
using OffersManagementSystem.Infrastructure.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
    );

// Add Identity services
builder.Services.AddIdentity<AppIdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// Add Dapper
builder.Services.AddScoped(typeof(IAppDbDapper<>), sp =>
{
    var connStr = builder.Configuration.GetConnectionString("DefaultConnection");
    return ActivatorUtilities.CreateInstance(sp, typeof(AppDbDapper<>), connStr ?? "");
});

builder.Services.AddScoped<IOfferRepository, OfferRepository>();

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
