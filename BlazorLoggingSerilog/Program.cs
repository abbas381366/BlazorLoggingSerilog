using BlazorLoggingSeriLog;
using BlazorLoggingSeriLog.Components;
using BlazorLoggingSeriLog.DbModels;
using Microsoft.EntityFrameworkCore;
using Serilog;


var logconfig = new LoggerConfiguration()
    .Filter.ByExcluding(eve =>
    {
        var propSchema = eve.Properties.GetValueOrDefault("Scheme");
        if (propSchema != null)
        {
            if (propSchema.ToString() == "\"http\"") return true;
        }
        return false;
    })
    .Enrich.FromLogContext()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File(@"C:\Users\abbas\Desktop\log.txt", rollingInterval:RollingInterval.Minute)
    .WriteTo.Seq("http://localhost:5341/")
    .CreateLogger();

Log.Logger = logconfig;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddSerilog();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddAuthentication()
    .AddCookie("Cookie", opt =>
    {
        opt.LoginPath = "/login";
    });
// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddDbContextFactory<MyDbcontext>((service, option) =>
{
    var conf = service.GetRequiredService<IConfiguration>();
    string ConnectionString = conf.GetConnectionString("MyDB");
    option.UseSqlServer(ConnectionString);
    option.LogTo(Log.Logger.Information,minimumLevel:LogLevel.Information);
    option.EnableDetailedErrors();
    option.EnableSensitiveDataLogging();
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}

app.UseStaticFiles();
app.UseAntiforgery();
app.UseAuthentication();
app.UseAuthorization();
app.UseLoggerMiddleware();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
