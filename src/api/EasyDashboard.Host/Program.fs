namespace EasyDashboard.Host

open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.Configuration
open WorkerService

module Host =
    let configureAppServices (_:HostBuilderContext) (services:IServiceCollection) =
        services.AddHostedService<WorkerService>() |> ignore<IServiceCollection>
        
    let CreateHostBuilder argv : IHostBuilder =
        let builder = Host.CreateDefaultBuilder(argv)
        builder
            .ConfigureServices(configureAppServices)
    
    [<EntryPoint>]
    let main argv =
        let hostBuilder = CreateHostBuilder argv
        hostBuilder.Build().Run()
        0