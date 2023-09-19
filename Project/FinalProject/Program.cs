using FinalProject.Contexts;
using FinalProject.Core.Entities;
using FinalProject.Extentions;
using FinalProject.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<service1>();
//this one is for user
builder.Services.Configure<IdentityOptions>(option =>
{
    option.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
    option.Lockout.MaxFailedAccessAttempts = 3;
    option.Lockout.AllowedForNewUsers = true;
    option.Password.RequireDigit = false;
    option.Password.RequiredLength = 3;
    option.User.RequireUniqueEmail = true;
    option.SignIn.RequireConfirmedEmail = false;
});


//error
void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    // Other middleware configurations...

    if (env.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
    }
    else
    {
        app.UseExceptionHandler("/Home/Error");
        app.UseHsts();
    }

    // Other middleware configurations...
}


// Add services to the container.
builder.Services.AddControllersWithViews();
//Database
builder.Services.AddDbContext<FinalProjectDatbase>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});
//User
builder.Services.AddIdentity<AppUser, IdentityRole>()
    .AddDefaultTokenProviders() //<-- it must be added for tokens
    .AddEntityFrameworkStores<FinalProjectDatbase>();

builder.Services.AddScoped<MailSender>();
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

app.UseAuthentication();
app.UseAuthorization();


app.UseEndpoints(endpoints =>
{
    endpoints.MapAreaControllerRoute(
        name: "Admin",
        areaName: "Admin",
        pattern: "admin/{controller=Home}/{action=Index}/{id?}"
    );

    // other areas configurations go here 

    endpoints.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}"
          );

    endpoints.MapControllerRoute(
       name: "error",
       pattern: "{*url}",
       defaults: new { controller = "Home", action = "Error" }
   );
});
app.Run();
