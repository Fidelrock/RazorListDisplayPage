using RazorTableDemo.Services;
using RazorTableDemo.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// Configure application settings
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

// Register services with Singleton lifetime for better performance
// These services are stateless and can be safely shared across requests
builder.Services.AddSingleton<ITaxAuthorityService, TaxAuthorityService>();
builder.Services.AddSingleton<IICItemMapService, ICItemMapService>();
builder.Services.AddSingleton<ISalesInvoiceService, SalesInvoiceService>();
builder.Services.AddSingleton<IETIMSEntityAttributeService, ETIMSEntityAttributeService>();
builder.Services.AddSingleton<IETIMSequenceService, ETIMSequenceService>();

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

app.UseAuthorization();

app.MapRazorPages();

app.Run();
