using Microsoft.AspNetCore.Localization;
using Microsoft.JSInterop;
using PointOfSaleClient.Models;
using PointOfSaleClient.Services;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddDevExpressBlazor();
builder.Services.AddHttpClient();
builder.Services.AddMemoryCache();
builder.Services.AddSingleton<IItemService, ItemService>();
builder.Services.AddSingleton<IModifierService, ModifierService>();
builder.Services.AddSingleton<IUserService, UserService>();
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.Configure<RequestLocalizationOptions>(options => {
    options.DefaultRequestCulture = new RequestCulture("es");
    var supportedCultures = new List<CultureInfo>() { new CultureInfo("es") };
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});
builder.Services.Configure<DevExpress.Blazor.Configuration.GlobalOptions>(options => {
    options.BootstrapVersion = DevExpress.Blazor.BootstrapVersion.v5;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseRequestLocalization();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();