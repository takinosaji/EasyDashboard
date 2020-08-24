module EasyDashboard.Host.Entry

    open EasyDashboard.CheckEngine.HostBuilder
    open EasyDashboard.Api.HostBuilder
    
    open Microsoft.Extensions.Hosting
    
    [<EntryPoint>]
    let main argv =
        let hostBuilder = Host.CreateDefaultBuilder(argv)
        hostBuilder
            .ConfigureCheckEngine()
            .ConfigureApi()
            .Build()
            .Run()
        0