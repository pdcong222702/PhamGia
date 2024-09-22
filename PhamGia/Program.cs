using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Configuration;
using PhamGia;
using PhamGia.Common.Implement;
using PhamGia.Common.Interface;
using PhamGia.Data;
using PhamGia.PhamGiaLib;
using PhamGia.PhamGiaLib.impl;
using Radzen;
using Syncfusion.Blazor;

var builder = WebApplication.CreateBuilder(args);
var Configuration = builder.Configuration;
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options => {
    options.LoginPath = "/login";
});
// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<DialogService>();
builder.Services.AddSingleton<WeatherForecastService>();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://vapi.vnappmob.com/") });
builder.Services.AddScoped<ApiService>();
builder.Services.AddSingleton<ISerilogProvider>(new Serilog(Configuration, null));
builder.Services.AddSingleton<ICommon, Common>();
builder.Services.AddSingleton<IErrorHandle, ErrorHandle>();
builder.Services.AddSingleton<IDataProvider, DataProvider>();
builder.Services.AddSingleton<IBDSContext, DBSContext>();
builder.Services.AddScoped<ExcelService>();
builder.Services.AddSyncfusionBlazor();
//builder.Services.AddSyncfusionBlazor(options => { options.IgnoreScriptIsolation = true; });
//Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("YOUR_LICENSE_KEY");

var app = builder.Build();
app.UseStaticFiles();
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

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
