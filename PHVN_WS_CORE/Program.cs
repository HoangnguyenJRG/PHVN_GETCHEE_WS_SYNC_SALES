using PHVN_WS_CORE;
using PHVN_WS_CORE.SERVICES;
using PHVN_WS_CORE.SHARED.Apis;
using PHVN_WS_CORE.Workers;
using Serilog;

IHost host = Host.CreateDefaultBuilder(args)
    .UseWindowsService()
    .ConfigureAppConfiguration((builderContext, config) =>
    {
        config
            .SetBasePath(System.AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Production"}.json", true)
            .AddEnvironmentVariables()
            .Build();
    })
    .ConfigureServices((hostContext, services) =>
    {
        services.AddHostedService<Worker>();
        services.AddScoped<IAPIClientService, APIClientService>();
        services.AddWorkders(hostContext.Configuration);
        services.AddServices(hostContext.Configuration);
        
        Log.Logger = new LoggerConfiguration()
                     .ReadFrom.Configuration(hostContext.Configuration)
                     .WriteTo.Console()
                     .WriteTo.Map("name", "base", (name, wt) => wt.File(path: $"{System.AppDomain.CurrentDomain.BaseDirectory}\\Logs\\{DateTime.Now.ToString("yyyyMM")}\\{name}_.txt",
                                                                         rollingInterval: RollingInterval.Day,
                                                                         shared: true))
                     .CreateLogger();
    })
    .UseSerilog()
    .Build();

await host.RunAsync();
