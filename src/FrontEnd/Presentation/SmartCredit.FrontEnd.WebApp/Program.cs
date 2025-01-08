using FluentValidation.AspNetCore;
using FluentValidation;
using SmartCredit.FrontEnd.WebApp.Contracts;
using SmartCredit.FrontEnd.WebApp.Filters;
using SmartCredit.FrontEnd.WebApp.Helpers;
using SmartCredit.FrontEnd.WebApp.Services;
using DinkToPdf;
using DinkToPdf.Contracts;

var builder = WebApplication.CreateBuilder(args);
ConfigurationHelper.Initialize(builder.Configuration);

// Add services to the container.
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(new CustomAuthorize());
}).AddRazorRuntimeCompilation();

builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

builder.Services.AddHttpClient();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<CustomAuthorize>(_ => new CustomAuthorize());

var architectureFolder = (IntPtr.Size == 8) ? "64 bit" : "32 bit";
var wkHtmlToPdfPath = Path.Combine(builder.Environment.ContentRootPath, $"wkhtmltox\\v0.12.4\\{architectureFolder}\\libwkhtmltox");
CustomAssemblyLoadContext context = new CustomAssemblyLoadContext();
context.LoadUnmanagedLibrary(wkHtmlToPdfPath);
builder.Services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));


var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=CreditCards}/{action=Index}/{id?}");

app.Run();
