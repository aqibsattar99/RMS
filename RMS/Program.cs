using RMS.Models;

using Microsoft.EntityFrameworkCore;
using Rotativa.AspNetCore;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// To Get the Session Data Been Sent to Whole Website
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();


// Add session service
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Set session timeout as needed
    options.Cookie.HttpOnly = true; // Security: Prevent client-side access to session cookie
    options.Cookie.IsEssential = true; // Make session cookie essential
});


var config = builder.Configuration;
builder.Services.AddDbContext<dbRMSContext>(options =>
    options.UseSqlServer(config.GetConnectionString("DB")));

//builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));


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

// Add session middleware
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=index}/{id?}");
app.UseRotativa();
app.Run();
