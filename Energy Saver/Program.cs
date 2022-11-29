using Auth0.AspNetCore.Authentication;
using Energy_Saver.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Energy_Saver.DataSpace;
using Microsoft.AspNetCore.Identity;
using NLog.Web;
using NLog;

var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("init main");

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Host.UseNLog();

builder.Services.AddDbContext<EnergySaverTaxesContext>(options =>
    options.UseNpgsql(builder.Configuration["DatabaseConnectionString"]));

builder.Services.AddAuth0WebAppAuthentication(options =>
{
    options.Domain = builder.Configuration["Auth0:Domain"];
    options.ClientId = builder.Configuration["Auth0:ClientId"];
    options.Scope = "openid profile email";
});

// Add services to the container.
builder.Services.AddRazorPages(options =>
{
    options.Conventions.AuthorizePage("/Input");
    options.Conventions.AuthorizePage("/Delete");
    options.Conventions.AuthorizePage("/Details");
    options.Conventions.AuthorizePage("/Edit");
    options.Conventions.AuthorizePage("/Statistics");
    options.Conventions.AuthorizePage("/Account/Logout");
    options.Conventions.AuthorizePage("/Account/Profile");
}).AddNToastNotifyToastr(new NToastNotify.ToastrOptions
{
    ProgressBar = true,
    TimeOut = 5000
});

builder.Services.AddChartServices();
builder.Services.AddSuggestionServices();
builder.Services.AddNotificationServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseNToastNotify();

app.MapRazorPages();

app.Run();

LogManager.Shutdown();