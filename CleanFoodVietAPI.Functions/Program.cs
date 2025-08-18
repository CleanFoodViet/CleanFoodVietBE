using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using CleanFoodVietAPI.Data.Entities;

var ip = await new HttpClient().GetStringAsync("https://api.ipify.org");
Console.WriteLine($"App public IP: {ip}");

var host = new HostBuilder()
    // hook up the Functions runtime
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureAppConfiguration(cfg =>
    {
        cfg.AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
           .AddEnvironmentVariables();
    })
    .ConfigureServices((context, services) =>
    {
        // Read the env-var / Values entry directly
        var conn = context.Configuration["DefaultConnection"];
        if (string.IsNullOrWhiteSpace(conn))
            throw new InvalidOperationException("DefaultConnection is not set");

        services.AddDbContext<CleanFoodVietDbContext>(opts =>
            opts.UseMySql(conn, ServerVersion.AutoDetect(conn)));
    })
    .Build();

host.Run();