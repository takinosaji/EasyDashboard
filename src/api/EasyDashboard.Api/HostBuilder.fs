module EasyDashboard.Api.HostBuilder
    
    open Microsoft.AspNetCore.Hosting
    open Microsoft.Extensions.Hosting
    
    open EasyDashboard.Api.Configuration
        
    type IHostBuilder with
        member builder.ConfigureApi() =
            builder.ConfigureWebHostDefaults(
                fun webHostBuilder ->
                    webHostBuilder    
                        .UseStartup<Startup>()
                        .ConfigureLogging configureLogging
                        |> ignore)
