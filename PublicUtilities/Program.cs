using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PublicUtilities.Data;
using PublicUtilities.Interface;
using PublicUtilities.Models;
using PublicUtilities.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IIndicatorsRepository, IndicatorsRepository>();
builder.Services.AddScoped<IStatisticRepository, StatisticRepository>();

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>();
builder.Services.AddMemoryCache();
builder.Services.AddSession();
//builder.Services.AddSession(options =>
//{
//    options.IdleTimeout = TimeSpan.FromMinutes(30); // Set your desired session timeout
//    options.Cookie.HttpOnly = true;
//    options.Cookie.IsEssential = true;
//});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
       .AddCookie();

var app = builder.Build();

if (args.Length == 1 && args[0].ToLower() == "seeddata")
{
    await Seed.SeedData(app);
    //Seed.SeedData(app);
}

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
app.UseAuthentication();
app.UseAuthorization();

app.UseSession();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
