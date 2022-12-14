using ArtGallery.Core.Constants;
using ArtGallery.Infrastructure.Data;
using ArtGallery.Infrastructure.Data.Models;
using ArtGallery.ModelBinders;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApplicationDbContexts(builder.Configuration);
//builder.Services.AddApplicationIdentity();
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();
//builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
//{
//    options.SignIn.RequireConfirmedAccount = true;
//})
//    .AddRoles<IdentityRole>()
//    .AddEntityFrameworkStores<ApplicationDbContext>();

//builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
})
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews()
     .AddMvcOptions(options =>
     {
         options.ModelBinderProviders.Insert(0, new DecimalModelBinderProvider());
         options.ModelBinderProviders.Insert(1, new DateTimeModelBinderProvider(FormatingConstant.NormalDateFormat));
         options.ModelBinderProviders.Insert(2, new DoubleModelBinderProvider());
     })
    .AddMvcLocalization(LanguageViewLocationExpanderFormat.Suffix);

builder.Services.AddApplicationServices();
//builder.Services.AddApplicationServices(builder.Configuration);

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
    app.UseDeveloperExceptionPage();
}
else
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

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
);

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Welcome}/{id?}");
app.MapControllerRoute(
    name: "default",
    pattern: "Home/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "default",
    pattern: "Store/{action=Explore}/{id?}");
app.MapControllerRoute(
    name: "default",
    pattern: "Cart/{action=Cart}/{id?}");
app.MapRazorPages();

app.Run();
