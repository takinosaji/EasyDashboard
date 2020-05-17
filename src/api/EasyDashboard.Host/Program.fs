namespace EasyDashboard

open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.Configuration

module Host =
    let configureAppServices (hostContext:HostBuilderContext) (services:IServiceCollection) =
        services.AddHostedService<WorkerService>() |> ignore<IServiceCollection>
        
    let configureAppConfiguration (hostContext:HostBuilderContext) (config:IConfigurationBuilder) =  
        config
            .AddJsonFile("appsettings.json",false,true)
            .AddJsonFile(sprintf "appsettings.%s.json" hostContext.HostingEnvironment.EnvironmentName ,true)
            .AddEnvironmentVariables() |> ignore
        
    let CreateHostBuilder argv : IHostBuilder =
        let builder = Host.CreateDefaultBuilder(argv)
        builder
            .ConfigureAppConfiguration(configureAppConfiguration)
            .ConfigureServices(configureAppServices)
    
    [<EntryPoint>]
    let main argv =
        let hostBuilder = CreateHostBuilder argv
        hostBuilder.Build().Run()
        0