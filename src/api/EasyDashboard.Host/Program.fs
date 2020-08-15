module EasyDashboard.Host.Entry

    open Microsoft.Extensions.DependencyInjection
    open Microsoft.Extensions.Hosting
    open EasyDashboard.Host.WorkerService

    let configureAppServices (_:HostBuilderContext) (services:IServiceCollection) =
        services.AddHostedService<WorkerService>() |> ignore
        
    let CreateHostBuilder argv : IHostBuilder =
        let builder = Host.CreateDefaultBuilder(argv)
        builder
            .ConfigureServices(configureAppServices)
    
    [<EntryPoint>]
    let main argv =
        let hostBuilder = CreateHostBuilder argv
        hostBuilder.Build().Run()
        0