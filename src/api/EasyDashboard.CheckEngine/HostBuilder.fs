module EasyDashboard.CheckEngine.HostBuilder
    
    open EasyDashboard.CheckEngine
    
    open Microsoft.Extensions.DependencyInjection
    open Microsoft.Extensions.Hosting
            
    let configureAppServices (_:HostBuilderContext) (services: IServiceCollection) =
        services
            .AddHttpClient()
            .AddHostedService<EngineService>() |> ignore
            
    type IHostBuilder with
        member builder.ConfigureCheckEngine() =
            builder.ConfigureServices configureAppServices           

    

    
    